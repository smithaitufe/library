using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Inventory: BaseEntity {
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int VariantId { get; set; }
        [Required]
        public int Quantity{ get; set; } = 0 ;
        public int Checkout { get; set; } = 0;

        [ForeignKey("LocationId")]
        public Term Location { get; set; }
        [ForeignKey("VariantId")]
        public Variant Variant { get; set; }

    }
}