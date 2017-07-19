using System;
using System.Collections.Generic;
// using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class CheckOut: BaseEntity
    {
        [Required]
        [ForeignKey("Patron")]
        public long PatronId { get; set; }
        public  User Patron { get; set; }
        [ForeignKey("Variant")]
        public int VariantId { get; set; }
        public Variant Variant { get; set; }
        [ForeignKey("VariantCopy")]
        public int? VariantCopyId { get; set; }
        public VariantCopy VariantCopy { get; set; }
        [Required]       
        public int RequestedDaysId { get; set; }
        public int? ApprovedDaysId { get; set; }
        [ForeignKey("RequestedDaysId")]
        public Term RequestedDays { get; set; }
        [ForeignKey("ApprovedDaysId")]
        public Term ApprovedDays { get; set; }        
        public ICollection<CheckOutState> CheckOutStates { get; set; }
        public bool Active { get; set; } = true;        
        public CheckOut() {}
        public CheckOut (long modifiedByUserId, int statusId ) {
            CheckOutStates = new HashSet<CheckOutState>() 
            {
                new CheckOutState { ModifiedByUserId = modifiedByUserId, StatusId = statusId }
            };
        }
        
        public ICollection<Recall> RecalledBooks { get; set; }

    }
}