using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.AddressViewModels 
{
    public class CreateEditAddressViewModel 
    {        
        public int AddressId { get; set; }
        [Display(Name="Address"), Required(ErrorMessage="{0} cannot be empty")]        
        public string Line { get; set; }
        [Display(Name="City"), Required(ErrorMessage="{0} cannot be empty")]        
        public string City { get; set; }
        [Display(Name="State"), Required(ErrorMessage="{0} was not selected")]        
        public int? StateId { get; set; }
        public ICollection<SelectListItem> States = new HashSet<SelectListItem>(); 
    }
}