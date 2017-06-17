using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Library.Web.Models.BootstrapModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.StateViewModels
{
    public class StateEditorViewModel
    {
        public int Id { get; set; }
        [Display(Name="Country")]
        [Required(ErrorMessage="{0} is required")]
        public int? CountryId { get; set; }
        [Display(Name="Name")]
        [Required(ErrorMessage="{0} is required")]
        public string Name { get; set; }
        [Display(Name="Abbreviation")]
        [Required(ErrorMessage="{0} is required")]
        public string Abbreviation { get; set; }
        public ICollection<SelectListItem> Countries { get; set; }

        public EditorAttributes EditorAttributes { get; set; }
        
    }
}