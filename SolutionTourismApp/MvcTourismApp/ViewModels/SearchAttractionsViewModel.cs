using LibraryTourismApp;

namespace MvcTourismApp.ViewModels
{
    public class SearchAttractionsViewModel
    {
        public int? SearchNameById { get; set; }
        public string? SearchCity { get; set; }
        public string? SearchCounty { get; set; }
        public int? SearchThemeId { get; set; }
        public AttractionTypes? SearchAttractionType { get; set; }

        public List<Attraction>? SearchResult { get; set; }

    }
}
