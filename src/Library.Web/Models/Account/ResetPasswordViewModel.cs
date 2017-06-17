using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        public bool IsUserExisting { get; set; } = false;
        [Display(Name="Phone Number")]
        [Required(ErrorMessage="{0} cannot be blank")]
        [StringLength(11)]
        [RegularExpression(@"^?([0][789][01])?([0-9]{8})$", ErrorMessage = "Not a valid Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
