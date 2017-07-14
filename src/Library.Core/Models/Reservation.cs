using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Reservation: BaseEntity {
        public long PatronId { get; set; }
        public bool Active { get; set; }
        public int NumberOfDaysId { get; set; }
        public int BasisId { get; set; }        
        public long ReservedByUserId { get; set; }

        [ForeignKey("PatronId")]
        public User Patron { get; set; }
        [ForeignKey("ReservedByUserId")]
        public User ReservedBy { get; set; }
        [ForeignKey("BasisId")]
        public Term Basis { get; set; }
    }
}