using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]        
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password confirmation did not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 11)]
        public string PhoneNumber { get; set; }        
        [Display(Name="I hereby agree to abide by the rules & regulations of the Library")]
        [Required]
        public bool Agreed { get; set; }

        [Display(Name="Contact Address")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Line { get; set; }
        
        [Display(Name="City/Town")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string City { get; set; }

        [Display(Name="State")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int StateId { get; set; }
        public ICollection<SelectListItem> States { get; set; } = new HashSet<SelectListItem>();

        public bool RegistrationSuccessful { get; set; } = false;


    }
}
