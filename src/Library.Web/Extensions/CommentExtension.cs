using Library.Web.Models;
using Library.Web.Models.PostViewModels;

namespace Library.Web.Extensions {
    public static class CommentExtension {
        public static Comment MapToComment(this PostCommentViewModel model) {
            return new Comment {
                CommenterId = model.CommenterId,
                Text = model.Text,
                PostId = model.PostId,
                ParentId = model.ParentId,
                Status = model.Status
            };
        }
    }
}