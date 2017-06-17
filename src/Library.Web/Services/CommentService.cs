using System.Linq;
using Library.Core.Models;
using Library.Data;
using Library.Models;
using Library.Repo;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Services
{
    public class CommentService {
        private readonly LibraryDbContext _context;
        public CommentService(LibraryDbContext context) {
            _context = context;
        }
        private IQueryable<Comment> Comments() {
            return from comments in _context.Comments
                .Include(c => c.Post)
                .Include(c => c.Commenter)
                .Include( c => c.Parent) select comments;
        }
        public void AddComment(Comment comment) {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }
        public void EditComment(Comment comment) {
            _context.Entry(comment).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void DeleteComment(int id) {
            var comment = _context.Comments.Find(id);
            if(comment !=  null) {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }            
        }
    }
}