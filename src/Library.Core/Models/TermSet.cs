using System.ComponentModel.DataAnnotations;

namespace Library.Core.Models
{
    public class TermSet:BaseEntity
    {   
        [Required]     
        [MaxLength(255)]
        public string Name { get; set; }
    }
}