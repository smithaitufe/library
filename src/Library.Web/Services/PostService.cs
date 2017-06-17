using System.Linq;
using Library.Core.Models;
using Library.Data;
using Library.Models;
using Library.Repo;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Services
{
    public class PostService {
        public LibraryDbContext _context;
        public PostService(){}
        public PostService(LibraryDbContext context) {
            _context = context;
        }
        private IQueryable<Post> Posts() { 
            return _context.Posts
            .Include(post => post.Author)
            .Include(post => post.Club)
            .Include(post => post.Category)
            .Include(post => post.Comments)            
            .ThenInclude(comment => comment.Commenter);
        }
        public IQueryable<Post> GetAllPosts(){
            return Posts();
        }
        public IQueryable<Post> GetAllPostsForUser(int userId) => Posts().Where(post => post.AuthorId == userId);
        public IQueryable<Post> GetPostById(int id) => Posts().Where(post => post.Id == id);
        public IQueryable<Post> GetPostsByClub(int clubId) => Posts().Where(post => post.ClubId == clubId);

        public void AddPost(Post post) {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }
        public void EditPost(Post post){
            _context.Entry(post).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}