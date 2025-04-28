using LibraryTourismApp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcTourismApp.Data;
using System.Security.Claims;

namespace MvcTourismApp.Models
{
    public class AppUserRepo : IAppUserRepo
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _database;
        private readonly UserManager<AppUser> _userManager;

        public AppUserRepo(ApplicationDbContext database, IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager)
        {
            _database = database;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public string GetLoggedInUserId()
        {
            string userId = _userManager.GetUserId(_contextAccessor.HttpContext.User);
            return userId;
        }

        public Tourist GetTouristById(string id)
        {
            return _database.Tourist.Find(id);
        }

        public Moderator GetModeratorById(string id)
        {
            return _database.Moderator.Find(id);
        }

        public List<Tourist> GetAllTourists()
        {
            List<Tourist> allTourists = _database.Tourist.Include(t => t.ReviewsCreated).Include(t => t.AttractionsAdded).ToList();
            return allTourists;
        }
    }
}
