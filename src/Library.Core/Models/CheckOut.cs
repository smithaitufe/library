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
        public int PatronId { get; set; }
        [Required]
        public int VariantId { get; set; }
        [Required]
        public int LocationId { get; set; }
        public bool Returned { get; set; } = false;
        [DataType(DataType.Date)]
        public Nullable<DateTime> ReturnedDate { get; set; }
        [ForeignKey("VariantId")]
        public Variant Variant { get; set; }
        [ForeignKey("PatronId")]
        public  User Patron { get; set; } 
        [Required]       
        public int RequestedDaysId { get; set; }
        public int? ApprovedDaysId { get; set; }
        [ForeignKey("RequestedDaysId")]
        public Term RequestedDays { get; set; }
        [ForeignKey("ApprovedDaysId")]
        public Term ApprovedDays { get; set; }
        [ForeignKey("LocationId")]
        public Term Location { get; set; }
        public ICollection<CheckOutState> CheckOutStates { get; set; }
        public CheckOut() {}
        public CheckOut (int modifiedByUserId, int statusId ) {
            CheckOutStates = new HashSet<CheckOutState>() {
                new CheckOutState { ModifiedByUserId = modifiedByUserId, StatusId = statusId }
            };
        }
        public ICollection<Recall> RecalledBooks { get; set; }

    }
}