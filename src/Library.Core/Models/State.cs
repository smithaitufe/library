using System.ComponentModel.DataAnnotations;

namespace Library.Core.Models
{
    public class State:BaseEntity {

        public int CountryId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(10)]
        public string Abbreviation { get; set; }

        public Term Country { get; }
    }
}