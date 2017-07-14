using System.Security.Claims;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Extensions;
using Library.Web.Models.PostViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Areas.Members.Controllers
{
    [Area(SiteAreas.Members)]
    [Authorize(Policy="MemberOnly")]
    public class CommentController: Controller {
        LibraryDbContext _context;
        private readonly CommentService commentService;
        public CommentController(LibraryDbContext context){
            _context = context;
            commentService = new CommentService(context);
        }
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult Create([FromBody] PostCommentViewModel model) {
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out int userId);    
            model.CommenterId = userId;
            // model.Status = CommentStatus.InReview;            
            var comment = model.MapToComment();
            commentService.AddComment(comment);
            return Json(new {Successful = true, Comment = comment});
        }
    }
}