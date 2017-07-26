using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Library.Core.Models
{
    public class User : IdentityUser<long>
    {
        public string LibraryNo { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        public Image Photo { get; set; }
        public Nullable<DateTime> BirthDate { get; set; }
        public DateTime InsertedAt { get; private set;} = DateTime.Now;
        public bool ChangePasswordFirstTimeLogin { get; set; } = false;
        public bool Approved { get; set; }
        public bool Suspended { get; set; } = false;
        public bool Locked { get; set; } = false;
        public bool ExpiresAt { get; set; }
        public DateTimeOffset ApprovedAt { get; set; }
        [NotMapped]
        public string FullName => $"{LastName} {FirstName}";

        public ICollection<UserLocation> LocationsLink { get; set; } = new HashSet<UserLocation>();
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public ICollection<Comment> Comments { get; set; }
        public ICollection<UserAddress> UserAddresses { get; set; } = new HashSet<UserAddress>();
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Reservation> ReservationBookings { get; set; }
        public ICollection<CheckOutState> CheckOutStates { get; set; }
        public ICollection<Recall> RecalledBooks { get; set; }    
        
    }
}
