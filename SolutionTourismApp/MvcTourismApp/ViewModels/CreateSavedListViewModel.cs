using System.ComponentModel.DataAnnotations;

namespace MvcTourismApp.ViewModels
{
    public class CreateSavedListViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name needs to be at least 1 character", MinimumLength = 1)]
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
