using LibraryTourismApp;
using Microsoft.EntityFrameworkCore;
using MvcTourismApp.Data;

namespace MvcTourismApp.Models
{
    public class AttractionRepo : IAttractionRepo
    {
        private ApplicationDbContext _database;

        public AttractionRepo(ApplicationDbContext database)
        {
            _database = database;
        }

        public void UpdateAttraction(Attraction attraction)
        {
            _database.Attraction.Update(attraction);
            _database.SaveChanges();
        }

        public Attraction FindAttraction(int attractionId)
        {
            Attraction attraction = _database.Attraction.Include(a => a.Themes).ThenInclude(at => at.Theme).Include(a => a.Reviews).ThenInclude(ar => ar.Tourist).Include(a => a.TouristWhoAdded).Include(a => a.ModeratorWhoMadeDecision).Where(a => a.AttractionId == attractionId).FirstOrDefault();
            return attraction;
        }

        public List<Attraction> GetAllAttractions()
        {
            List<Attraction> allAttractions = _database.Attraction.Include(a => a.SavedLists).Include(a => a.Themes).ThenInclude(at => at.Theme).Include(a => a.Reviews).Include(a => a.TouristWhoAdded).ToList();
            return allAttractions;
        }

        public List<Attraction> GetAllAttractionsWithReviews()
        {
            List<Attraction> allAttractionsWithReviews = GetAllAttractions().Where(a => a.Reviews.Any()).ToList();
            return allAttractionsWithReviews;
        }

        public List<Attraction> GetAllPendingAttractions()
        {
            List<Attraction> allPendingAttractions = GetAllAttractions().Where(a => a.Status == AttractionStatus.Pending).ToList();
            return allPendingAttractions;
        }

        public List<Attraction> GetAllDeniedAttractions()
        {
            List<Attraction> allPendingAttractions = GetAllAttractions().Where(a => a.Status == AttractionStatus.Denied).ToList();
            return allPendingAttractions;
        }

        public List<Theme> GetAllThemes()
        {
            List<Theme> allThemes = _database.Theme.Where(t => t.AttractionList.Any()).ToList();
            return allThemes;
        }

        public int AddAttraction(Attraction attraction)
        {
            int attractionId = 0;
            _database.Attraction.Add(attraction);

            try
            {
                _database.SaveChanges();
                attractionId = attraction.AttractionId;
            }
            catch (DbUpdateException dbUpdateException)
            {
                attractionId = -1;
                string errorMessage = dbUpdateException.Message;
            }

            return attractionId;
        }

        public int AddAttractionTheme(AttractionTheme attractionTheme)
        {
            int attractionThemeId = 0;
            _database.AttractionTheme.Add(attractionTheme);

            try
            {
                _database.SaveChanges();
                attractionThemeId = attractionTheme.AttractionThemeId;
            }
            catch (DbUpdateException dbUpdateException)
            {
                attractionThemeId = -1;
                string errorMessage = dbUpdateException.Message;
            }

            return attractionThemeId;
        }

        public void DeleteAttraction(Attraction attraction)
        {
            _database.Attraction.Remove(attraction);
            _database.SaveChanges();
        }

        public void DeleteAttractionTheme(AttractionTheme attractionTheme)
        {
            _database.AttractionTheme.Remove(attractionTheme);
            _database.SaveChanges();
        }

        public Theme FindTheme(int themeId)
        {
            Theme? theme;

            theme = _database.Theme.Where(t => t.ThemeId == themeId).FirstOrDefault();

            return theme;
        }
    }
}
