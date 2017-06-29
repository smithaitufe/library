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
        [MaxLength(30)]
        public string ISBN { get; set; }
        [StringLength(50)]
        public string Edition { get; set; }
        [StringLength(50)]
        public string Volume { get; set; }
        [ForeignKey("Language")]
        public int LanguageId { get; set; }        
        [Required]
        public int Pages { get; set; }
        [Required]
        public int FormatId { get; set; }      
        [Required] 
        public int GrantId { get; set; }
        [Required]
        public int YearId { get; set; }
        [Required]
        [ForeignKey("DaysAllowed")]
        public int DaysAllowedId { get; set; }
        [ForeignKey("CollectionMode")]
        public int CollectionModeId { get; set; }
        [ForeignKey("Fine")]
        public int FineId { get; set; }        
        // Navigation Properties
        public Term Language { get; set; }
        public Term DaysAllowed { get; set; }               
        public Term CollectionMode { get; set; }        
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
        public ICollection<VariantPrice> PricesLink { get; set; }          
        public ICollection<VariantLocation> VariantLocations { get; set; }        
        public ICollection<Inventory> Inventories { get; set; }
    }
}