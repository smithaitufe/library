using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.AddressViewModels
{
    public class AddressEditorViewModel
    {
        [Display(Name="Address")]
        [Required(ErrorMessage="{0} is requried")]
        public string Line { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }
        [Display(Name="Country/Location")]
        public int? CountryId { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
        
    }
}