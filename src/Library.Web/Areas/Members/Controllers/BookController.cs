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
        private LibraryDbContext _context;   
        private int userId;
        private readonly TermService termService;
        private readonly BookService bookService;
        public BookController(LibraryDbContext context, IHttpContextAccessor httpContextAccessor){
            _context = context;  
            termService = new TermService(context);         
            bookService = new BookService(context);
            userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        public IActionResult Index(SortFilterPageOptions options) {            
            var books = bookService.GetCheckOutBooksForUser(userId).MapToCheckedBookViewModel().ToList();
            return View(books);
        }
        public IActionResult Borrow(SortFilterPageOptions SortFilterOptions, SearchBookOptions SearchOptions, int? page) { 
               
            // var query = bookService.GetAllVariants();

            // if(!string.IsNullOrEmpty(SearchOptions.Phrase)) {
            //     page = 1;
            //     query = bookService.GetBooksByTitle(query, SearchOptions.Phrase);
            // }
            // if(!string.IsNullOrEmpty(SearchOptions.Location)) {
            //     query = bookService.GetBooksByLocationId(query, int.Parse(SearchOptions.Location));
            // }
            // var borrowedVariants = bookService
            // .GetCheckOutBooks("Borrow Approved")
            // .Select(c => c.VariantCopy.Variant)
            // .ToList();
            // query = query.Except(borrowedVariants);
            // var bookList = query.MapToBookViewModel().ToList();
            // var model = new BookListViewModel(bookList, SearchOptions, SortFilterOptions);             
            
            // PopulateBookListViewDropdowns(model); 
            // return View(model);
            
            var bookList = bookService
            .GetAllVariantCopies()
            .Where(c=>c.Out == false)
            .Select(c => c.Variant)
            .Distinct()
            .MapToBookViewModel().ToList();    

            var model = new BookListViewModel(bookList, SearchOptions, SortFilterOptions); 
            PopulateBookListViewDropdowns(model); 

            return View(model);

            
        }

        [HttpPost]
        public async Task<IActionResult> Borrow(List<BookViewModel> books) {            
            var statusId = (await _context.CheckOutStatuses
            .Where(cs=> cs.Name.Equals("Borrow Initiated"))
            .FirstOrDefaultAsync()).Id;

            foreach(var model in books) {
                if(model.Checked){                    
                    var checkOut = new CheckOut(userId, statusId)
                    { 
                        PatronId = userId, 
                        VariantId = model.VariantId 
                    };
                    _context.CheckOuts.Add(checkOut);
                }
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(BookController.Index));
        }        
        [HttpGet]
        public async Task<IActionResult> Return () {            
            var model = new ReturnBookViewModel();
            await GetCheckOutBooksAsync(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Return(ReturnBookViewModel model) {
            var checkOut = _context.CheckOuts.Find(model.SelectedCheckOutId);
            if(checkOut == null) return  NotFound();
            var checkOutState = new CheckOutState
            {
                StatusId = _context.CheckOutStatuses.Where(cs => cs.Name.ToLower().Equals("return initiated")).FirstOrDefault().Id
            };
            checkOut.CheckOutStates.Add(checkOutState);
            _context.Entry(checkOut).State = EntityState.Modified;
            _context.SaveChanges();
            await GetCheckOutBooksAsync(model);
            return View(model);
        }
        private async Task GetCheckOutBooksAsync(ReturnBookViewModel model)
        {
            var query = bookService.GetCheckOutBooks("borrow approved");
            query = query.Where(co=>co.PatronId == userId);            
            model.CheckOutBooks  = await query.MapToCheckedBookViewModel().ToListAsync();
        }
        public IActionResult Catalogue(int? CatalogueId, int? GenreId){            
            var catalogueModel = new BookCatalogueViewModel();
            PopulateCatalogueViewDropdowns(catalogueModel);
            catalogueModel.Books = bookService.GetAllBooks().MapToBookViewModel().ToList();
            return View(catalogueModel);
        }
        public IActionResult GetBooksByCategory(int categoryId) {            
            var query =  bookService.GetBooksByCategory(categoryId);
            var books =query.MapToBookViewModel().ToList();
            return PartialView("_CategoryBooks", books);
        }
        [HttpGet]
        public IActionResult GetSearchContent(SortFilterPageOptions options) {
            var bookFilterService = new BookFilterDropdownService(_context);
            return Json(bookFilterService.GetFilterDropdownValues((BooksFilterBy)options.FilterBy));
        }
        [HttpGet]
        public IActionResult GetBookPreview(int id) {
            // var query = context.Variants
            // .Include(v=>v.Book)
            // .GroupBy(v => v.Book.ISBN)
            // .Select( g => new { ISBN = g.Key, Variant = g.Select(r => r).FirstOrDefault()})
            // .Select( gr => gr.Variant).ToList();            
        
            
            var query = bookService.GetBookByVariantId(id);
            var result = query.FirstOrDefault();
            var model = query.MapToBookPreviewViewModel().FirstOrDefault();            
            return PartialView("_Preview", model);
        }
        


        private void PopulateCatalogueViewDropdowns(BookCatalogueViewModel model) {            
            model.Categories = termService.GetTermsBySet("book-category").MapToSelectList();
            model.Genres = termService.GetTermsBySet("genre").MapToSelectList();
        }
        private void PopulateBookListViewDropdowns(BookListViewModel model) {
            model.SearchOptions.Locations = _context.Locations.MapToSelectList().ToList();
        }
    }
}