using LibraryTourismApp;

namespace MvcTourismApp.ViewModels
{
    public class ListAllTouristsViewModel
    {
        public string? TouristId { get; set; }
        public List<Tourist> ListOfTourists { get; set; }

        public Review? RecentReview { get; set; }
    }
}
