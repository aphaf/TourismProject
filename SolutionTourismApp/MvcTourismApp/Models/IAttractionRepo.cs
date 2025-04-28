using LibraryTourismApp;

namespace MvcTourismApp.Models
{
    public interface IAttractionRepo
    {
        public List<Attraction> GetAllAttractions();
        public List<Attraction> GetAllAttractionsWithReviews();
        public List<Attraction> GetAllDeniedAttractions();
        public List<Attraction> GetAllPendingAttractions();
        public Attraction FindAttraction(int attractionId);
        public int AddAttraction(Attraction attraction);
        public void UpdateAttraction(Attraction attraction);
        public void DeleteAttraction(Attraction attraction);

        //Theme / AttractionTheme is within the AttractionRepo because there was not any additional needs for creating individual repos for these classes
        public Theme FindTheme(int themeId);
        public List<Theme> GetAllThemes();
        public int AddAttractionTheme(AttractionTheme attractionTheme);
        public void DeleteAttractionTheme(AttractionTheme attractionTheme);
    }
}
