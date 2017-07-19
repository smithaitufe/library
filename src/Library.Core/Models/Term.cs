using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Term:BaseEntity
    {
        [Required]
        public int TermSetId { get;set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [ForeignKey("TermSetId")]
        public virtual TermSet TermSet { get; set; }

        public ICollection<Book> CategoryBooks { get; set; }
        public ICollection<Book> GenreBooks { get; set; }

        public ICollection<Variant> DaysAllowedVariants { get; set; }        
        public ICollection<Variant> FineVariants { get; set; }
        public ICollection<Variant> GrantVariants { get; set; }
        public ICollection<Variant> YearVariants { get; set; }
        public ICollection<Variant> FormatVariants { get; set; }
        public ICollection<Variant> LanguageVariants { get; set; }

        public ICollection<CheckOut> CheckOutRequestedDays { get; set; }
        public ICollection<CheckOut> CheckOutApprovedDays { get; set; }
        public ICollection<VariantCopy> AvailabilityVariantCopies { get; set; }
        public ICollection<VariantCopy> SourceVariantCopies { get; set; }
        public ICollection<VariantPrice> VariantPrices { get; set; }









        
    }
}