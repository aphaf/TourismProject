using LibraryTourismApp;
using System.ComponentModel.DataAnnotations;

namespace MvcTourismApp.ViewModels
{
    public class AddAttractionViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name needs to be at least 5 characters", MinimumLength = 5)]
        public string? Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
        [StringLength(200, ErrorMessage = "Description needs to be at least 1 characters", MinimumLength = 1)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public AttractionTypes? TypeOfAttraction { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Street is required")]
        [StringLength(200, ErrorMessage = "Street needs to be at least 1 character", MinimumLength = 1)]
        public string? Street { get; set; }
                
        [Required(AllowEmptyStrings = false, ErrorMessage = "City is required")]
        [StringLength(200, ErrorMessage = "City needs to be at least 1 character", MinimumLength = 1)]
        public string? City { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "ZipCode is required")]
        [StringLength(200, ErrorMessage = "ZipCode needs to be at least 1 character", MinimumLength = 1)]
        public string? ZipCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "State is required")]
        [StringLength(200, ErrorMessage = "State needs to be at least 5 characters, please do not use an abbreviation", MinimumLength = 5)]
        public string? State { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "County is required")]
        [StringLength(200, ErrorMessage = "County needs to be at least 1 character", MinimumLength = 1)]
        public string? County { get; set; }        


        public string? Website { get; set; }
        public List<int>? ThemeIds { get; set; }
    }
}
