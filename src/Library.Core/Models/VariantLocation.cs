using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class VariantLocation: BaseEntity {
        [Required]
        [ForeignKey("Variant")]
        public int VariantId { get; set; }
        [Required]
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        [Required]
        [ForeignKey("Availability")]
        public int AvailabilityId { get; set; }
        [Required]     
        [ForeignKey("Source")]
        public int SourceId { get; set; }  
        [MaxLength(40)]       
        public string SerialNo { get; set; } 
        public bool Out { get; set; } = false;        
        public bool Visible { get; set; } = true;


        
        public Variant Variant { get; set; }        
        public Location Location { get; set; }        
        public Term Availability { get; set; }
        public Term Source { get; set; }        
    }
}