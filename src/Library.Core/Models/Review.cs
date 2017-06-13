using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Review: BaseEntity {

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public int Stars { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Comment { get; set; }
        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}