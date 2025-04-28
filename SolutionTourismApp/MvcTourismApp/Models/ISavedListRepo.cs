using LibraryTourismApp;

namespace MvcTourismApp.Models
{
    public interface ISavedListRepo
    {
        public List<SavedList> GetAllSavedListsForUser(string userId);

        public SavedList FindSavedList(int savedListId);

        public int AddSavedList(SavedList savedList);

        public void UpdateSavedList(SavedList savedList);
        public void DeleteAttractionOnSavedList(AttractionSavedList attractionSavedList);
        public AttractionSavedList FindAttractionSavedList(int attractionSavedListId);
        public int AddAttractionToSavedList(AttractionSavedList attractionSavedList);
        public void DeleteSavedList(SavedList savedList);
    }
}
