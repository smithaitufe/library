using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Web.Models.AccountViewModels
{
    public class SignupListingViewModel
    {        
        public IList<SignupViewModel> Signups { get; set; } = new List<SignupViewModel>();
        
    }
}