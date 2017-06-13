using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Announcement: BaseEntity {
        [Required]
        public string Title { get; set; }
        public string Introduction { get; set; }
        [Required]
        public string Text { get; set; }        
        [Required]
        public DateTime ReleaseAt { get; set; }
        [Required]
        public DateTime ExpiresAt { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Term Category { get; set; }
    }
}