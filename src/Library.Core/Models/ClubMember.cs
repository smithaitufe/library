using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class ClubMember: BaseEntity
    {
        [ForeignKey("User")]
        public long UserId { get; set; }
        public int ClubId { get; set; }        
        public User User { get; set; }
        [ForeignKey("ClubId")]
        public Club Club { get; set; }
    }
}