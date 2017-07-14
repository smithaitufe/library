using System.Linq;
using System.Security.Claims;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Extensions;
using Library.Web.Models.PostViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Areas.Members.Controllers
{
    [Area(SiteAreas.Members)]
    [Authorize(Policy="MemberOnly")]
    public class PostController: Controller {
        protected readonly LibraryDbContext _context;
        protected readonly PostService postService;
        protected int userId;
        public PostController(LibraryDbContext context) {
            _context = context;
            postService = new PostService(_context);    
            // userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        public IActionResult Index(int? clubId) {
            var postListing = new PostListingViewModel();
            userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var posts = postService.GetAllPostsForUser(userId).MapToPostViewModel().ToList();            

            postListing.Posts = postService.GetAllPosts().OrderByDescending(p => p.InsertedAt).MapToPostViewModel().ToList();
            return View(postListing);
        }
        public IActionResult Create(){
            var model = new PostMessageViewModel();
            PopulatePostMessageDropdowns(model);
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(PostMessageViewModel model) {
            if(!ModelState.IsValid){ 
                PopulatePostMessageDropdowns(model);
                return View(model);
            }
            var post = model.MapToPost();
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out int authorId);
            post.AuthorId = authorId;
            postService.AddPost(post);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id){  

            var post = postService.GetPostById(id).Where(p => p.AuthorId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));            
            var model = post.MapToPostMessageViewModel().SingleOrDefault();
            if(model == null) return NotFound();
            
            PopulatePostMessageDropdowns(model);
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(int id, PostMessageViewModel model) {
            if(!ModelState.IsValid){ 
                PopulatePostMessageDropdowns(model);
                return View(model);
            }
            var post = model.MapToPost();
            postService.EditPost(post);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Details(int id) {
            var query = postService.GetPostById(id);
            var post = query.SingleOrDefault();            
            if(post == null) return NotFound();            
            if( post.Author.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) ){                
                post.Views++;
                postService.EditPost(post);
            }          
            
            return View(post.MapToPostViewModel());
        }
        
        public IActionResult Delete(int id) {
            var query = postService.GetPostById(id).Where(p => p.AuthorId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var post = query.SingleOrDefault();
            _context.Posts.Remove(post);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        
        private void PopulatePostMessageDropdowns(PostMessageViewModel model){
            var termService = new TermService(_context);
            var clubService = new ClubService(_context);
            model.Categories = termService.GetTermsBySet("post-category").OrderBy(cat => cat.Name).ToList();
            model.Clubs = clubService.GetAllClubs().MapToClubViewModel().ToList();
        }
        

        
    }
}