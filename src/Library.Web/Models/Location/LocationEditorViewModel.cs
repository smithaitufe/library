using System.ComponentModel.DataAnnotations;
using Library.Models.BootstrapModels;

namespace Library.Models.LocationViewModels
{
    public class LocationEditorViewModel
    {
        public int? Id { get; set; }
        
        [Display(Name="Location Name")]
        [Required(ErrorMessage="{0} cannot be empty")]
        public string Name { get; set; }
        [Display(Name="Location Code")]
        [Required(ErrorMessage="{0} cannot be empty")]
        public string Code { get; set; }

        public EditorAttributes EditorAttributes { get; set; }
        
    }
}