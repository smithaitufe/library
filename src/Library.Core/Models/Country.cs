using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Models
{
    public class Country: BaseEntity { 
        [Required]
        [MaxLength(70)]
        public string Name { get; set; }
        [MaxLength(10)]
        public string TelephoneCode { get; set; }
        [MaxLength(5)]
        public string Abbreviation { get; set; }
        public string Icon { get; set;}

        public ICollection<Address> Addresses { get; set; }
    }
}