using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class VariantLocation: BaseEntity {
        [Required]   
        public int VariantId { get; set; }
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int AvailabilityId { get; set; }
        [Required]     
        public int SourceId { get; set; }  
        [MaxLength(20)]       
        public string SerialNo { get; set; } 
        public bool Visible { get; set; } = true;


        [ForeignKey("VariantId")]
        public Variant Variant { get; set; }
        [ForeignKey("LocationId")]
        public Location Location { get; set; }
        [ForeignKey("AvailabilityId")]
        public Term Availability { get; set; }
        [ForeignKey("SourceId")]
        public Term Source { get; set; }        
    }
}