using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class CheckOutStatus: BaseEntity {
        public int? ParentId { get; set; }
        [Required]
        public string Name { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public bool Out { get; set; } = false;

        [ForeignKey("ParentId")]
        public CheckOutStatus Parent { get; set; }

        public ICollection<CheckOutState> CheckOutsLink { get; set; }
    }
}