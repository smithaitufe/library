using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Variant: BaseEntity
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public int Pages { get; set; }
        [Required]
        public int FormatId { get; set; }      
        [Required] 
        public int GrantId { get; set; }
        [Required]
        public int YearId { get; set; }
        [Required]
        public int DaysAllowedId { get; set; }
        public int CollectionModeId { get; set; }
        public int FineId { get; set; }        
        // Navigation Properties
        [ForeignKey("DaysAllowedId")]
        public Term DaysAllowed { get; set; }               
        [ForeignKey("CollectionModeId")]
        public Term CollectionMode { get; set; }        
        [ForeignKey("FineId")]
        public Term Fine { get; set; }
        [ForeignKey("GrantId")]
        public Term Grant { get; set; }
        [ForeignKey("YearId")]
        public Term Year { get; set; }       
        [ForeignKey("FormatId")]
        public Term Format { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }        
        public ICollection<CheckOut> CheckOuts { get; set; }
        public ICollection<Edition> Editions { get; set; }
        public ICollection<Volume> Volumes { get; set; }      
        public ICollection<VariantPrice> PricesLink { get; set; }  
        public ICollection<VariantLanguage> Languages { get; set; }
        public ICollection<VariantLocation> VariantLocations { get; set; }
        
        public ICollection<Inventory> Inventories { get; set; }
    }
}