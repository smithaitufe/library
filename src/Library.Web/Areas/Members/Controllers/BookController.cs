using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Extensions;
using Library.Web.Models.BookViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Areas.Members.Controllers
{
    [Area(SiteAreas.Members)]
    [Authorize(Policy="MemberOnly")]
    public class BookController: Controller
    {
        private LibraryDbContext context;   
        private int userId;
        private readonly TermService termService;
        public BookController(LibraryDbContext context, IHttpContextAccessor httpContextAccessor){
            this.context = context;  
            termService = new TermService(context);                      
            userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        public IActionResult Index(SortFilterPageOptions options) {            
            var bookService = new BookService(context);
            var books = bookService.GetCheckOutBooksForUser(userId).MapToCheckedBookViewModel().ToList();
            return View(books);
        }
        public IActionResult Borrow(SortFilterPageOptions SortFilterOptions, SearchBookOptions SearchOptions, int? page) { 
            var bookService = new BookService(context);   
            var query = bookService.GetAllVariants();
            if(!string.IsNullOrEmpty(SearchOptions.Phrase)) {
                page = 1;
                query = bookService.GetBooksByTitle(query, SearchOptions.Phrase);
            }
            if(!string.IsNullOrEmpty(SearchOptions.Location)) {
                query = bookService.GetBooksByLocationId(query, int.Parse(SearchOptions.Location));
            }
            var borrowedVariants = bookService.GetCheckOutBooks("Approved").Select(c => c.Variant).ToList();
            query = query.Except(borrowedVariants);
            var bookList = query.MapToBookViewModel().ToList();
            var model = new BookListViewModel(bookList, SearchOptions, SortFilterOptions);             
            
            PopulateBookListViewDropdowns(model); 
            return View(model);
            
        }

        [HttpPost]
        public JsonResult Borrow(List<BookViewModel> books) {            
            var status = termService.GetTermsBySet("checkout-status").Where(t => t.Name.ToLower().Equals("pending")).SingleOrDefault();
            foreach(var model in books) {
                if(model.Checked){
                    var checkOut = new CheckOut(userId, status.Id){ PatronId = userId, VariantId = model.VariantId };
                    context.CheckOuts.Add(checkOut);
                }
            }
            context.SaveChanges();
            return Json(new { books = books } );
        }

        [HttpPost]
        public IActionResult Checkout(List<BookViewModel> books) {
            var status = termService.GetTermsBySet("checkout-status").Where(t => t.Name.ToLower().Equals("pending")).SingleOrDefault();
            foreach(var model in books) {
                if(model.Checked){
                    var checkOut = new CheckOut(userId, status.Id) { PatronId = userId, VariantId = model.VariantId };
                    context.CheckOuts.Add(checkOut);
                }
            }
            context.SaveChanges();
            return RedirectToAction(nameof(BookController.Index));
        }
        
        [HttpGet]
        public IActionResult Return () {            
            var model = new ReturnBookViewModel();
            model.CheckOutBooks = GetCheckOutBooks(userId).Where(co => co.Returned == false).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Return(ReturnBookViewModel model) {
            var checkOut = context.CheckOuts.Find(model.SelectedCheckOutId);
            if(checkOut == null){
                return  NotFound();
            } 
            checkOut.Returned = true;
            checkOut.ReturnedDate = DateTime.UtcNow;
            context.Entry(checkOut).State = EntityState.Modified;
            context.SaveChanges();
            model.CheckOutBooks = GetCheckOutBooks(userId).Where(co => co.Returned == false).ToList();
            return View(model);
        }
        public IActionResult Catalogue(int? CatalogueId, int? GenreId){
            var bookService = new BookService(context);
            var catalogueModel = new BookCatalogueViewModel();
            PopulateCatalogueViewDropdowns(catalogueModel);
            catalogueModel.Books = bookService.GetAllBooks().MapToBookViewModel().ToList();
            return View(catalogueModel);
        }
        public IActionResult GetBooksByCategory(int categoryId) {
            var bookService = new BookService(context);
            var query =  bookService.GetBooksByCategory(categoryId);
            var books =query.MapToBookViewModel().ToList();
            return PartialView("_CategoryBooks", books);
        }
        [HttpGet]
        public IActionResult GetSearchContent(SortFilterPageOptions options) {
            var bookFilterService = new BookFilterDropdownService(context);
            return Json(bookFilterService.GetFilterDropdownValues((BooksFilterBy)options.FilterBy));
        }
        [HttpGet]
        public IActionResult GetBookPreview(int id) {
            // var query = context.Variants
            // .Include(v=>v.Book)
            // .GroupBy(v => v.Book.ISBN)
            // .Select( g => new { ISBN = g.Key, Variant = g.Select(r => r).FirstOrDefault()})
            // .Select( gr => gr.Variant).ToList();            
        
            var bookService = new BookService(context);
            var query = bookService.GetBookByVariantId(id);
            var result = query.FirstOrDefault();
            var model = query.MapToBookPreviewViewModel().FirstOrDefault();            
            return PartialView("_Preview", model);
        }
        
        private IList<CheckedBookViewModel> GetCheckOutBooks(int userId) {
            var bookService = new BookService(context);
            var books = bookService.GetCheckOutBooksForUser(userId).MapToCheckedBookViewModel().ToList(); 
            return books;
        }

        private void PopulateCatalogueViewDropdowns(BookCatalogueViewModel model) {
            var termService = new TermService(context);
            model.Categories = termService.GetTermsBySet("book-category").MapToSelectList();
            model.Genres = termService.GetTermsBySet("genre").MapToSelectList();
        }
        private void PopulateBookListViewDropdowns(BookListViewModel model) {
            model.SearchOptions.Locations = context.Locations.MapToSelectList().ToList();
        }
    }
}