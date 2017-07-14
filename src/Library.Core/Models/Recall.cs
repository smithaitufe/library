using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Recall:BaseEntity {
        [ForeignKey("CheckOut")]
        public int CheckOutId { get; set; }
        public CheckOut CheckOut { get; set; }
        public DateTime RecallDate { get; set; } = DateTime.Now;     
        [ForeignKey("RecalledByUser")]   
        public long RecalledByUserId { get; set;}        
        public User RecalledBy { get; set; }        
        
        
    }
}