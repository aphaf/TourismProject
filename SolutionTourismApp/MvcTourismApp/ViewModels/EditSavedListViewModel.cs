using System.ComponentModel.DataAnnotations;

namespace MvcTourismApp.ViewModels
{
    public class EditSavedListViewModel
    {
        public int SavedListId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty.")]
        [StringLength(200, ErrorMessage = "Name needs to be at least 1 character.", MinimumLength = 1)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}
