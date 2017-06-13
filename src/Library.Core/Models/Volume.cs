using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Volume:BaseEntity
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        public int VariantId { get; set; }
        [ForeignKey("VariantId")]
        public Variant Variant { get; set; }
        
    }
}