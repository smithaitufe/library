using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models.SessionViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("Enter Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Keep me signed in")]
        public bool RememberMe { get; set; }


    }
}
