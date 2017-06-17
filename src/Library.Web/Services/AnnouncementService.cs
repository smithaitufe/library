using System;
using System.Linq;
using Library.Core.Models;
// using Library.Data;
using Library.Web.Models;
using Library.Repo;

namespace Library.Web.Services
{
    public class AnnouncementService {
        LibraryDbContext _context;

        public AnnouncementService(LibraryDbContext context) {
            _context = context;
        }

        public IQueryable<Announcement> Announcements => _context.Announcements;
        public IQueryable<Announcement> GetAll() => Announcements;
        public IQueryable<Announcement> GetById(int id) => Announcements.Where(a => a.Id == id);
        public IQueryable<Announcement> GetByCategoryId(int categoryId) => Announcements.Where(a => a.CategoryId == categoryId);
        public IQueryable<Announcement> GetRecent() => Announcements.Where(a => DateTime.Compare(a.ReleaseAt, DateTime.Now.Date) > 0);
    }
}