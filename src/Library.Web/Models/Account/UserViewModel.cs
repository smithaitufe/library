using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.AccountViewModels
{
    public class UserViewModel {
        [Display(Name = "User Id")]
        public long UserId { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get { return LastName + " " + FirstName; } }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime InsertedAt { get; set; }
        public ICollection<UserLocation> Locations { get; set; }
        public ICollection<RoleViewModel> Roles { get; set; }
        [Display(Name = "User Claims")]
        public List<SelectListItem> UserClaims { get; set; }   
        
    }
}