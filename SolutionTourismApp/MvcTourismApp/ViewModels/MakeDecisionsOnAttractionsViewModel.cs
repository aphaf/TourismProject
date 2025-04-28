using LibraryTourismApp;

namespace MvcTourismApp.ViewModels
{
    public class MakeDecisionsOnAttractionsViewModel
    {
        public List<Attraction> PendingAttractions { get; set; }
        public List<int> AttractionIds { get; set; }
        public List<string> Decisions {  get; set; }
    }
}
