using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        // [Required]
        // [EmailAddress]
        // public string Email { get; set; }
        [Display(Name="Phone Number")]
        [Required(ErrorMessage="{0} cannot be blank")]
        [StringLength(11)]
        [RegularExpression(@"^?([0][789][01])?([0-9]{8})$", ErrorMessage = "Not a valid Phone number")]
        public string PhoneNumber { get; set; }
    }
}
