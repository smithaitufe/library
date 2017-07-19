using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;

namespace Library.Web.Models.ShelfViewModels
{
    public class ShelfEditorViewModel
    {
        [Display(Name="Location")]
        [Required(ErrorMessage="{0} is required")]
        public int? LocationId { get; set; }
        public ICollection<Location> Locations { get; set; }
        [Display(Name="Name")]
        [Required(ErrorMessage="{0} is required")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}