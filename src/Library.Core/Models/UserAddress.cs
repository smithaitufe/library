using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class UserAddress {
        [ForeignKey("User")]
        public int  UserId { get; set; }
        [ForeignKey("Address")]        
        public int AddressId { get; set; }        
        public User User { get; set; }
        public Address Address { get; set; }
    }
}