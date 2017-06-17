using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models.AccountViewModels
{
    public class ChangePasswordFirstTimeLoginViewModel
    {
        public string UserName { get; set; }
        public string  OldPassword { get; set; }
        public string Role { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}