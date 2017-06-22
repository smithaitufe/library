using System.Linq;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
// using Library.Web.Data;
using Library.Web.Extensions;
using Library.Web.Models.AuthorViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Areas.Admin.Controllers
{
    [Area(SiteAreas.Admin)]
    [Authorize(Policy="AdministratorOnly")]
    public class AuthorController: Controller
    {
        LibraryDbContext _context;
        public AuthorController(LibraryDbContext context) {
            _context = context;
        }
        public IActionResult Index() {
            var authorListing = new AuthorListingViewModel();
            authorListing.Authors  = _context.Authors.OrderBy(a => a.Id).OrderBy(a => a.LastName).MapToAuthorViewModel().ToList();            
            return View(authorListing);
        }

        public IActionResult Create(){
            var model = new CreateEditAuthorViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(CreateEditAuthorViewModel model){
            if(!ModelState.IsValid)
                return View(model);  
                
            var author = model.MapToAuthor();
            _context.Add(author);
            _context.SaveChanges();
            return RedirectToAction(nameof(AuthorController.Index));
        }
        public IActionResult Edit(int id){            
            var model = _context.Authors.Where(a => a.Id == id).MapToCreateEditAuthorViewModel();
            if(model == null){
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(int id, CreateEditAuthorViewModel model){
            if(!ModelState.IsValid)
                return View(model);            
            var author = model.MapToAuthor();
            _context.Entry(author).State = EntityState.Modified;
            _context.SaveChanges();
            return View();
        }
        [HttpGet]
        public IActionResult AddEditAuthor(long? id) {
            var model = new CreateEditAuthorViewModel();
            if(id.HasValue){
                Author author = _context.Set<Author>().SingleOrDefault(a => a.Id == id.Value);
                if(author != null) {
                    model.Id = author.Id;
                    model.FirstName = author.FirstName;
                    model.LastName = author.LastName;
                    model.Email = author.Email;
                    model.PhoneNumber = author.PhoneNumber;
                }
            }
            return PartialView("_AddEditAuthor", model);
        }
        [HttpPost]
        public IActionResult AddEditAuthor(long? id, CreateEditAuthorViewModel model) {
            if(!ModelState.IsValid)
                return PartialView("_AddEditAuthor", model);
                
            bool isNew = !id.HasValue;
            var author = isNew ? new Author() : _context.Set<Author>().SingleOrDefault(a => a.Id == id.Value);
            author.LastName = model.LastName;
            author.FirstName = model.FirstName;
            author.PhoneNumber = model.PhoneNumber;
            author.Email = model.Email;
            if(isNew){
                _context.Add(author);
            }else{
                _context.Entry(author).State = EntityState.Modified;
            }                
            _context.SaveChanges();
            return RedirectToAction(nameof(AuthorController.Index));
        }

    }
}