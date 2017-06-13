using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class UserLocation: BaseEntity
    {
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public bool Active { get; set; } = true;
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("LocationId")]
        public Term Location { get; set; }
     

    }
}