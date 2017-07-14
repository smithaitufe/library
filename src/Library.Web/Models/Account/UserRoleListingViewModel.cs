using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Web.Models.AccountViewModels
{
    public class UserRoleListingViewModel {
        
        public int UserId { get; set; }
        public ICollection<Role> Roles { get; set; }
       
    }
}