using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Models
{
    public class Club:BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public ICollection<ClubGenre> GenresLink { get; set; }
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}