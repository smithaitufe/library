using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class VariantLanguage: BaseEntity {
        public int LanguageId { get; set; }
        public int VariantId { get; set; }

        [ForeignKey("LanguageId")]
        public Term Language { get; set; }
        [ForeignKey("BookVariantId")]
        public Variant Variant { get; set; }

    }
}