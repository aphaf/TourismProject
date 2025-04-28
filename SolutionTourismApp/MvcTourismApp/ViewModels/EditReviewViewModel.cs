using LibraryTourismApp;
using System.ComponentModel.DataAnnotations;

namespace MvcTourismApp.ViewModels
{
    public class EditReviewViewModel
    {
        public int ReviewId { get; set; }


        [Required(ErrorMessage = "Rating is required")]
        public int? Rating { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }


        [DataType(DataType.Date)]
        public DateTime? DateVisited { get; set; }
        public string? Description { get; set; }

        public DateTime? DateCreated { get; set; }
        public string? AttractionName { get; set; }
        public string? AttractionAddress { get; set; }
        public string? TouristFullName { get; set; }
        public string? TouristEmail { get; set; }

    }
}
