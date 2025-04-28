using LibraryTourismApp;
using System.ComponentModel.DataAnnotations;

namespace MvcTourismApp.ViewModels
{
    public class SearchReviewsViewModel
    {
        public int? AttractionId { get; set; }
        public string? AttractionName { get; set; }
        public string? AttractionFullAddress { get; set; }

        public string? SearchTouristId { get; set; }
        public int? SearchRating { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SearchStartCreationDate {  get; set; }

        [DataType(DataType.Date)]
        public DateTime? SearchEndCreationDate {  get; set; }
        public List<Review>? SearchResult { get; set; }
    }
}
