using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class ClubGenre: BaseEntity {
        public int ClubId { get; set; }
        public int GenreId { get; set; }
        [ForeignKey("ClubId")]
        public Club Club { get; set; }
        [ForeignKey("GenreId")]
        public Term Genre { get; set; }
    }
}