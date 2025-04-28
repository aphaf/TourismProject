using LibraryTourismApp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MvcTourismApp.Data;
using MvcTourismApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>
    (options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.User.RequireUniqueEmail = true;
    }
    )
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IAttractionRepo, AttractionRepo>();
builder.Services.AddTransient<IAppUserRepo, AppUserRepo>();
builder.Services.AddTransient<IReviewRepo, ReviewRepo>();
builder.Services.AddTransient<ISavedListRepo, SavedListRepo>();
//builder.Services.AddTransient<IEmailSender, EmailSender>();
//builder.Services.AddTransient<IAttractionEmailSender, AttractionEmailSender>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    //three services: database, app users, roles (for app users)
    var services = scope.ServiceProvider;

    try
    {
        InitialDatabase.SeedDatabase(services);
    }
    catch (Exception serviceException)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(serviceException.Message, "Error while using db, user, or role service");
    }

}//end of scope, connection auto shuts down

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
