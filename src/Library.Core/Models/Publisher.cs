using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Models
{
    public class Publisher:BaseEntity
    {     
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}