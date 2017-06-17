using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Models.AccountViewModels
{
    public class CreateUserViewModel
    {
        [Display(Name="First Name")]
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Display(Name="Last Name")]
        [Required]
        [StringLength(50)]
        public string  LastName { get; set; }
        [Display(Name="Email")]
        [Required]
        [EmailAddress]        
        public string Email { get; set; }
        [Display(Name="Phone Number")]
        [Required]
        [StringLength(11)]
        [RegularExpression(@"^?([0][789][01])?([0-9]{8})$", ErrorMessage = "Not a valid Phone number")]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [Display(Name="First Login")]
        public bool ChangePasswordFirstTimeLogin { get; set; } = true;     
        [Display(Name="Location")]
        [Required(ErrorMessage="{0} is required")]
        public int LocationId { get; set; }   
        public ICollection<SelectListItem> Locations { get; set; }
        [Display(Name="User Role")]
        [Required(ErrorMessage="{0} was not assigned")]
        public string Role { get; set; }

        [Display(Name="Roles")]
        [Required]
        public string[] SelectedRoles { get; set; }
        
        public ICollection<SelectListItem> Roles { get; set; } = new HashSet<SelectListItem>();
        public bool SelectedRoleCount { get { return Roles.ToList().Count() > 0; } }
        [Display(Name="User Claims")]
        public List<SelectListItem> UserClaims { get; set; } = new List<SelectListItem>();

    }
}