using System.ComponentModel.DataAnnotations;
using Library.Core.Models;
using Library.Web.Code;

namespace Library.Web.Models.PostViewModels
{
    public class PostCommentViewModel {
        public int CommenterId { get; set; }
        public int PostId { get; set; }   
        public int? ParentId { get; set; }  
        [DataType(DataType.Text)]   
        public string Text { get; set; }        
        public Term Status { get; set; }
    }
}