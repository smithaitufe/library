using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Library.Models.AddressViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.AccountViewModels {
    public class EditPatronProfileViewModel {
        [Display(Name="First Name"), Required(ErrorMessage="{0} cannot be empty"), StringLength(50, ErrorMessage="{0} must be between {2} to {1} long", MinimumLength = 2)]        
        public string FirstName { get; set; }
        [Display(Name="Last Name"), Required(ErrorMessage="{0} cannot be empty"), StringLength(50, ErrorMessage="{0} must be between {2} to {1} long", MinimumLength = 2)]        
        public string LastName { get; set; }
        public string LibraryNo { get; set; }        
        public string PhoneNumber { get; set; }
        
        
        // [Display(Name="Address"), Required(ErrorMessage="{0} cannot be empty")]        
        // public string Line { get; set; }
        // [Display(Name="City"), Required(ErrorMessage="{0} cannot be empty")]        
        // public string City { get; set; }
        // [Display(Name="State"), Required(ErrorMessage="{0} was not selected")]        
        // public int StateId { get; set; }
        // public ICollection<SelectListItem> States = new HashSet<SelectListItem>();        

        public CreateEditAddressViewModel Address { get; set; } = new CreateEditAddressViewModel();
        
    }
}