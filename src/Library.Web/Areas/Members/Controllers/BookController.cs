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
    [Route("Member/Book")]
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
        [HttpGet("")]
        public async Task<IActionResult> Index(SortFilterPageOptions options) {            
            // var books = await bookService
            // .GetCheckoutsForUser(userId)
            // .MapToCheckoutViewModel()
            // .ToListAsync();

            var checkouts = _context.CheckOuts            
            .Include(c=>c.Variant).ThenInclude(v=>v.Book).ThenInclude(b=>b.BookAuthors).ThenInclude(ba=>ba.Author)
            .Include(c=>c.Variant).ThenInclude(v=>v.Book).ThenInclude(b=>b.Publisher)
            .Include(c=>c.Variant).ThenInclude(v=>v.Format)
            .Include(c=>c.Variant).ThenInclude(v=>v.Book)
            .Include(c=>c.VariantCopy)
            .Include(c=>c.CheckOutStates)
            .Where(co=>co.PatronId == userId);

            var listing = new CheckoutListingViewModel();
            listing.Checkouts = await checkouts.MapToCheckoutViewModel().ToListAsync();

            return View(listing);
        }
        [HttpGet("Borrow")]
        public async Task<IActionResult> Borrow(SortFilterPageOptions SortFilterOptions, SearchBookOptions SearchOptions, int? page) { 
                        
            var books = new List<BookViewModel>();


            if(!string.IsNullOrEmpty(SearchOptions.Location))
            {
                var query = bookService
                .GetAllVariantCopies()
                .Where(c=>c.Out == false && c.LocationId == int.Parse(SearchOptions.Location))
                .Select(c => c.Variant);
                //query = bookService.GetBooksByLocationId(query, int.Parse(SearchOptions.Location));
                if(!string.IsNullOrEmpty(SearchOptions.Phrase)) {
                    page = 1;
                    query = bookService.GetBooksByTitle(query, SearchOptions.Phrase);
                }
                books = query
                .Distinct()
                .MapToBookViewModel().ToList(); 

                var days = await _context.Terms.Where(t=>t.TermSet.Name.Equals("book-days-allowed")).ToListAsync();
                foreach(var book in books)   {
                    book.Days = days;
                }
            }

            var model = new BookListViewModel(books, SearchOptions, SortFilterOptions); 
            PopulateBookListViewDropdowns(model); 

            return View(model);

            
        }

        [HttpPost("Borrow")]
        public async Task<IActionResult> Borrow(List<BookViewModel> books) {            
            var statusId = (await _context.CheckOutStatuses
            .Where(cs=> cs.Name.Equals("Borrow Initiated"))
            .FirstOrDefaultAsync()).Id;

            foreach(var model in books) {
                if(model.Checked){                    
                    var checkOut = new CheckOut(userId, statusId)
                    { 
                        PatronId = userId, 
                        VariantId = model.VariantId,
                        RequestedDaysId = model.RequestedDaysId 
                    };
                    _context.CheckOuts.Add(checkOut);
                }
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(BookController.Index));
        }        
        [HttpGet("Return")]
        public async Task<IActionResult> Return () {            
            var model = new ReturnBookViewModel();
            await GetCheckOutBooksAsync(model);
            return View(model);
        }

        [HttpPost("Return")]
        public async Task<IActionResult> Return(ReturnBookViewModel model) {
            var checkOut = await _context.CheckOuts.Include(c=>c.CheckOutStates).Where(c=>c.Id == model.SelectedCheckOutId).SingleOrDefaultAsync();
            if(checkOut == null) return  NotFound();
            var checkOutState = new CheckOutState
            {
                StatusId = _context.CheckOutStatuses.Where(cs => cs.Name.ToLower().Equals("return initiated")).FirstOrDefault().Id,
                ModifiedByUserId = userId
            };
            checkOut.CheckOutStates.Add(checkOutState);
            _context.Entry(checkOut).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await GetCheckOutBooksAsync(model);
            return View(model);
        }

        private async Task GetCheckOutBooksAsync(ReturnBookViewModel model)
        {
            var query = bookService.GetCheckOutBooks("borrow approved");
            query = query.Where(co=>co.PatronId == userId);            
            model.Checkouts  = await query.MapToCheckoutViewModel().ToListAsync();
        }
        [HttpGet("Catalogue")]
        public IActionResult Catalogue(int? CatalogueId, int? GenreId){            
            var catalogueModel = new BookCatalogueViewModel();
            PopulateCatalogueViewDropdowns(catalogueModel);
            catalogueModel.Books = bookService.GetAllBooks().MapToBookViewModel().ToList();
            return View(catalogueModel);
        }
        [HttpGet("GetBooksByCategory/{categoryId:int}")]
        public IActionResult GetBooksByCategory(int categoryId) {            
            var query =  bookService.GetBooksByCategory(categoryId);
            var books =query.MapToBookViewModel().ToList();
            return PartialView("_CategoryBooks", books);
        }
        [HttpGet("GetSearchContent")]
        public IActionResult GetSearchContent(SortFilterPageOptions options) {
            var bookFilterService = new BookFilterDropdownService(_context);
            return Json(bookFilterService.GetFilterDropdownValues((BooksFilterBy)options.FilterBy));
        }
        [HttpGet("Preview/{id:int}")]
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