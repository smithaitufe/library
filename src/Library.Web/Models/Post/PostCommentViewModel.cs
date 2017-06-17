using System.ComponentModel.DataAnnotations;
using Library.Code;

namespace Library.Models.PostViewModels
{
    public class PostCommentViewModel {
        public int CommenterId { get; set; }
        public int PostId { get; set; }   
        public int? ParentId { get; set; }  
        [DataType(DataType.Text)]   
        public string Text { get; set; }        
        public CommentStatus Status { get; set; }
    }
}