namespace MvcTourismApp.ViewModels
{
    public class DeleteReviewViewModel
    {
        public int ReviewId { get; set; }
        public string Title { get; set; }
        public string RatingStarIcons { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string AttractionName { get; set; }
        public string AttractionFullAddress { get; set; }
    }
}
