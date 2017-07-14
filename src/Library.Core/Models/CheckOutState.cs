using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class CheckOutState: BaseEntity {        
        public int CheckOutId { get; set; }
        public int StatusId {get; set; }
        public long ModifiedByUserId  { get; set; }
           
        [ForeignKey("StatusId")]
        public CheckOutStatus Status { get; set; }
        [ForeignKey("CheckOutId")]
        public CheckOut CheckOut { get; set; }
        [ForeignKey("ModifiedByUserId")]
        public User ModifiedBy { get; set; }
    }
}