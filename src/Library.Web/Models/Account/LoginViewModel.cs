using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage="Enter a valid Phone Number or Library ID")]
        [DisplayName("Enter Phone/Library ID")]
        public string ID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Keep me signed in")]
        public bool RememberMe { get; set; }


    }
}