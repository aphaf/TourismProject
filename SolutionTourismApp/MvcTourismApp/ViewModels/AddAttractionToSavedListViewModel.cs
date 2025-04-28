using System.ComponentModel.DataAnnotations;

namespace MvcTourismApp.ViewModels
{
    public class AddAttractionToSavedListViewModel
    {
        [Required(ErrorMessage = "You must choose an attraction.")]
        public int? AttractionId {  get; set; }
        public string? AttractionName { get; set; }
        public string? AttractionAddress { get; set; }

        [Required(ErrorMessage = "You must choose a saved list.")]
        public int? SavedListId { get; set; }

    }
}
