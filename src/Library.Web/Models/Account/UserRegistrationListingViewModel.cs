using System.Collections.Generic;
using Librille.Code;

namespace Library.Models.AccountViewModels
{
    public class UserRegistrationListingViewModel {
        public FilterUserRegistration FilterUserRegistrationData { get; set; } = new FilterUserRegistration();
        public List<UserRegistrationViewModel> Registrations { get; set; }   =  new List<UserRegistrationViewModel>();     
    }
}