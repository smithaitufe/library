// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Library.Core.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class User : IdentityUser<int>
    {
        public string LibraryNo { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        public Image Photo { get; set; }
        public DateTime InsertedAt { get; private set;} = DateTime.Now;
        public bool ChangePasswordFirstTimeLogin { get; set; } = false;
        public bool Approved { get; set; }
        public bool Suspended { get; set; } = false;
        public bool Locked { get; set; } = false;
        [NotMapped]
        public string FullName => $"{LastName} {FirstName}";
        public ICollection<UserLocation> LocationsLink { get; set; } = new HashSet<UserLocation>();
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public ICollection<Comment> Comments { get; set; }
        public ICollection<UserAddress> AddressesLink { get; set; } = new HashSet<UserAddress>();

        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Reservation> ReservationBookings { get; set; }

        public ICollection<CheckOutState> CheckOutStates { get; set; }
        public ICollection<Recall> RecalledBooks { get; set; }
        
    }
}
