using System.ComponentModel.DataAnnotations;

namespace MvcTourismApp.ViewModels
{
    public class DeleteSavedListViewModel
    {
        public int SavedListId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime? DateCreated { get; set; }

        public int? CountOfAttractions { get; set; }
    }
}
