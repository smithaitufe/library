using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Library.Core.Infrastructure;

namespace Library.Core.Models
{
    // public class Role : IdentityRole<long, UserRole, IdentityRoleClaim<long>>
    public class Role : IdentityRole<long>
    {
        public IList<UserRole> Users { get; set; } = new List<UserRole>();
    }
}
