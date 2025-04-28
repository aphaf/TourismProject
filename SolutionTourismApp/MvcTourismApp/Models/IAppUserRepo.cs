using LibraryTourismApp;

namespace MvcTourismApp.Models
{
    public interface IAppUserRepo
    {
        public string GetLoggedInUserId();
        public Tourist GetTouristById(string Id);
        public Moderator GetModeratorById(string Id);
        public List<Tourist> GetAllTourists();
    }
}
