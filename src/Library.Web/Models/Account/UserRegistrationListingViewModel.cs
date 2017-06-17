using System.Collections.Generic;
using Library.Web.Code;

namespace Library.Web.Models.AccountViewModels
{
    public class UserRegistrationListingViewModel {
        public FilterUserRegistration FilterUserRegistrationData { get; set; } = new FilterUserRegistration();
        public List<UserRegistrationViewModel> Registrations { get; set; }   =  new List<UserRegistrationViewModel>();     
    }
}