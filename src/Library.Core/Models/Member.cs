using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Member: BaseEntity
    {
        public int UserId { get; set; }
        public int ClubId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("ClubId")]
        public Club Club { get; set; }
    }
}