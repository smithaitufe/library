using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models.AccountViewModels
{
    public class UserRegistrationViewModel {
        [Display(Name="Id")]
        public int UserId { get; set; }
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [Display(Name="Last Name")]
        public string LastName { get; set; }
        [Display(Name="Full Name")]
        public string FullName { get { return $"{LastName} {FirstName}"; } }
        [Display(Name="Email Address")]
        public string Email { get; set; }
        public DateTime InsertedAt { get; set; }
        public bool Approved { get; set; } = false;

        public string[] Claims { get; set; }
    }   
}