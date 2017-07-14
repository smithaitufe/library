using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class PriceOffer:BaseEntity
    {
        [Required]
        public decimal NewPrice { get; set; }
        [DataType(DataType.MultilineText)]
        public string PromotionalText { get; set; }
        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}