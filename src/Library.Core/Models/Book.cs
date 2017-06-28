using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Book: BaseEntity
    {        
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        [MaxLength(255)]
        public string SubTitle { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }          
        [Required]
        [MaxLength(30)]
        public string ISBN { get; set; }
        [Required]
        [ForeignKey("Genre")]
        public int GenreId { get; set; }        
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; } 
        [Required]
        [ForeignKey("Publisher")]        
        public int PublisherId { get; set; }
        public bool Series { get; set; } = false;
        public int? NoInSeries { get; set; }
        public Image Cover { get; set; }


        // Navigation Properties        
        public Term Genre { get; set; }        
        public Term Category { get; set; }
        public Publisher Publisher { get; set; }
        public ICollection<Variant> Variants { get; set; }
        public ICollection<BookAuthor> AuthorsLink { get; set; }
        

    }
}