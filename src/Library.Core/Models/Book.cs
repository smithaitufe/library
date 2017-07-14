using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Book: BaseEntity
    {  
        public Book () {
            BookAuthors = new HashSet<BookAuthor>();
            Variants = new HashSet<Variant>();
        }      
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        [MaxLength(255)]
        public string SubTitle { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }        
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
        public ICollection<BookAuthor> BookAuthors { get; set; }
        

    }
}