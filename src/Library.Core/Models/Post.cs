using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Post: BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Text { get; set; }
        [Required]
        public int AuthorId { get; set;}
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int ClubId { get; set; }
        public bool Hidden { get; set; } = false;
        public bool Locked { get; set; } = false;
        public int Views { get; set; } = 0;     
        [ForeignKey("AuthorId")]
        public User Author { get; set; }
        [ForeignKey("CategoryId")]
        public Term Category { get; set; }
        [ForeignKey("ClubId")]
        public Club Club { get; set; }

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

    }
}