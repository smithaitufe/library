using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Address: BaseEntity {
        [Required]
        public string Line { get; set; }
        public string City { get; set; }   
        [ForeignKey("State")]     
        public int? StateId { get; set; }        
        public State State { get; set; }
        [ForeignKey("Country")]
        public int? CountryId { get; set; }
        public Country Country { get; set;}
        
        
    }
}