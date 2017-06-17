using System.Collections.Generic;

namespace Library.Models.AccountViewModels
{
    public class UserListingViewModel {

        public string Role { get; set; }
        public ICollection<UserViewModel> Users { get; set; }
        public ICollection<RoleViewModel> Roles { get; set; }
    }
}