using System.Collections.Generic;
using System.Linq;
using Library.Core.Models;
// using Library.Data;
using Library.Web.Extensions;
using Library.Web.Models;
using Library.Web.Models.PublisherViewModels;
using Library.Web.Models.TermSetViewModels;
using Library.Repo;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Services
{
    public class PublisherService {
        LibraryDbContext _context;
        public PublisherService(LibraryDbContext context){
            _context = context;
        }
        private IQueryable<Publisher> Publishers => _context.Publishers.Include(p => p.Books);
        
        public IQueryable<Publisher> GetAllPublishers() {
            return Publishers;          
        }
        public IQueryable<Publisher> GetPublisherById(int id) => Publishers.Where(p => p.Id == id);
    }
}