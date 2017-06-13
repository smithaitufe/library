namespace Library.Core.Models
{
    public class VariantPrice: BaseEntity {
        public int VariantId { get; set; }
        public int PriceId { get; set; }
        public int ConditionId { get; set; }

        public Variant Variant { get; set; }
        public Term Price { get; set; }
        public Term Condition { get; set; }
    }
}