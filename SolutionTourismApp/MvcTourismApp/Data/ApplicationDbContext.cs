using LibraryTourismApp;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MvcTourismApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Moderator> Moderator { get; set; }
        public DbSet<Tourist> Tourist { get; set; }
        public DbSet<SavedList> SavedList { get; set; }
        public DbSet<Attraction> Attraction { get; set; }
        public DbSet<AttractionSavedList> AttractionSavedList { get; set; }
        public DbSet<Theme> Theme { get; set; }
        public DbSet<AttractionTheme> AttractionTheme { get; set; }
        public DbSet<Review> Review { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AttractionTheme>()
                .HasAlternateKey(at => new { at.AttractionId, at.ThemeId });
            //composite key for attractiontheme

            builder.Entity<AttractionSavedList>()
                .HasAlternateKey(asl => new { asl.AttractionId, asl.SavedListId });
            //composite key for attractionsavedlist


        }
    }
}
