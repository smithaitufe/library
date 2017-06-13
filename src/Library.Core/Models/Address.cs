using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Address: BaseEntity {
        [Required]
        public string Line { get; set; }
        public string City { get; set; }
        [Required]
        [ForeignKey("State")]
        public int StateId { get; set; }        
        public Term State { get; set; }
    }
}