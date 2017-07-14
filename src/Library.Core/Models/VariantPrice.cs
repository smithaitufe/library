using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class VariantPrice: BaseEntity {
        [ForeignKey("Variant")]
        public int VariantId { get; set; }
        [ForeignKey("Price")]
        public int PriceId { get; set; }
        [ForeignKey("Condition")]
        public int ConditionId { get; set; }

        public Variant Variant { get; set; }
        public Term Price { get; set; }
        public Term Condition { get; set; }
    }
}