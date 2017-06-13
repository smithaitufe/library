using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Comment: BaseEntity
    {  
        [Required]
        [ForeignKey("Commenter")]
        public int CommenterId { get; set; }
        [Required]   
        [ForeignKey("Post")]  
        public int PostId { get; set; }
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Text { get; set; }
        public int StatusId { get; set; }

        public Post Post { get; set; }        
        public Comment Parent { get; set; }        
        public User Commenter { get; set; }
        public Term Status { get; set; }
        
        
    }
}