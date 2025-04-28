using System.ComponentModel.DataAnnotations;

namespace MvcTourismApp.ViewModels
{
    public class AddReviewViewModel
    {
        public int AttractionId {  get; set; }
        public string? AttractionName { get; set; }
        public string? AttractionAddress { get; set; }


        [Required(ErrorMessage = "You must enter a review rating.")]
        public int? Rating { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You must enter a review title.")]
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateVisited { get; set; }

        public string? Description { get; set; }
    }
}
