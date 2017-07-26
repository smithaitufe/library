using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Variant: BaseEntity
    {
        public Variant(){
            VariantCopies = new HashSet<VariantCopy>();
            VariantPrices = new HashSet<VariantPrice>();
        }
        [Required]
        [ForeignKey("Book")]
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
        [ForeignKey("Format")]
        public int FormatId { get; set; }      
        [Required]
        [ForeignKey("Grant")] 
        public int GrantId { get; set; }
        [Required]
        [ForeignKey("Year")]
        public int YearId { get; set; }
        [Required]
        [ForeignKey("DaysAllowed")]
        public int DaysAllowedId { get; set; }
        [ForeignKey("CollectionMode")]
        public int CollectionModeId { get; set; }
        [ForeignKey("Fine")]
        public int FineId { get; set; }
        public string CallNumber { get; set; }        
        // Navigation Properties
        public Term Language { get; set; }
        public Term DaysAllowed { get; set; }               
        public Term CollectionMode { get; set; }        
        public Term Fine { get; set; }        
        public Term Grant { get; set; }        
        public Term Year { get; set; }        
        public Term Format { get; set; }
        public Book Book { get; set; }        
        public ICollection<CheckOut> CheckOuts { get; set; }           
        public ICollection<VariantPrice> VariantPrices { get; set; }          
        public ICollection<VariantCopy> VariantCopies { get; set; }        
        public ICollection<Inventory> Inventories { get; set; }



    }
}