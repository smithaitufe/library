using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Library.Core.Models;
using Library.Web.Code;
using Library.Web.Extensions;
using Library.Web.Models.BookViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Library.Web.Models.StatusViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using Library.Web.Models.BootstrapModels;
using Microsoft.AspNetCore.Http;
using System;
using Library.Repo;

namespace Library.Web.Areas.Control.Admin.Controllers
{
    [Area(SiteAreas.Admin)]
    [Authorize(Policy = "AdministratorOnly")]
    [Route("Admin/Book")]
    public class BookController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly IHostingEnvironment _environment;
        private readonly ImageUploadSettings _imageUploadSettings;
        private readonly BookService bookService;
        private readonly TermService termService;
        private readonly PublisherService publisherService;
        private readonly IMapper _mapper;
        private readonly ImageService imageService;
        public BookController(LibraryDbContext context, IHostingEnvironment environment, IOptions<ImageUploadSettings> imageUploadSettings, IMapper mapper)
        {
            _context = context;
            _environment = environment;
            _imageUploadSettings = imageUploadSettings.Value;
            _mapper = mapper;
            bookService = new BookService(context);
            termService = new TermService(context);
            publisherService = new PublisherService(context);
            imageService = new ImageService(_context, _environment, _imageUploadSettings);
        }
        [HttpGet("")]
        public IActionResult Index(SortFilterPageOptions sortFilterPageOptions, SearchBookOptions searchOptions)
        {
            // var books = bookService.SortFilterPage(sortFilterPageOptions).ToList();
            // var model = new BookListViewModel(books, searchOptions, sortFilterPageOptions);
            // model.Locations = _context.Locations.OrderBy(t=>t.Name).MapToSelectList().ToList();
            // return View("~/Areas/Control/Admin/Views/Book/Index.cshtml", model);              

            var books = bookService.GetAllBooks().ToList().OrderBy(b => b.Title).OrderByDescending(b => b.InsertedAt).ToList();
            var bookListing = new BookListingViewModel(books, searchOptions, sortFilterPageOptions);
            return View(bookListing);
        }


        [HttpGet("Create")]
        public IActionResult Create()
        {
            var model = new BookEditorViewModel();
            PopulateDropdowns(model);
            return View(model);
        }
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        //([Bind("Title,SubTitle,Edition,Volume,Description,Pages,ISBN,Image,PublisherId,SelectedAuthorIds,CategoryId,FormatId,LanguageId,GenreId,YearId,CollectionModeId,ConditionId,PriceId,GrantId,LocationId,AvailabilityId,SourceId,FineId,DaysAllowedId, PublisherId,Quantity")]
        public async Task<IActionResult> Create(BookEditorViewModel model)
        {

            if (!ModelState.IsValid)
            {
                PopulateDropdowns(model);
                return View(model);
            }

            var book = model.MapToBook();

            var cover = await imageService.SaveToDirectory(model.Image);
            book.Cover = cover;

            var variant = model.MapToVariant();

            for (int i = 0; i < model.Quantity; i++)
            {
                variant.VariantCopies.Add(new VariantCopy
                {
                    LocationId = model.LocationId,
                    AvailabilityId = model.AvailabilityId,
                    SourceId = model.SourceId,
                    SerialNo = $"{variant.Id}{Common.RandomString(10)}",
                    ShelfId = model.ShelfId
                });
            }

            var variantPrice = new VariantPrice
            {
                VariantId = variant.Id,
                ConditionId = model.ConditionId,
                PriceId = model.PriceId
            };

            variant.VariantPrices.Add(variantPrice);

            if (model.SelectedAuthorIds.Any())
            {
                foreach (var authorId in model.SelectedAuthorIds)
                {
                    int.TryParse(authorId, out int id);
                    var bookAuthor = new BookAuthor { AuthorId = id };
                    book.BookAuthors.Add(bookAuthor);
                }
            }

            book.Variants.Add(variant);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(BookController.Index));

        }
        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            var book = bookService.GetBookByVariantId(id).MapToBookViewModel().SingleOrDefault();
            return View(book);
        }
        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var imageService = new ImageService(_context);
            var book = bookService.GetBookByVariantId(id).SingleOrDefault().MapToBookEditorViewModel();
            var bookId = (int)book.Id;
            book.SelectedAuthorIds = bookService.GetBookAuthors(id).Select(a => a.Id.ToString()).ToArray<string>();
            PopulateDropdowns(book);
            return View(book);
        }
        [HttpPost("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, BookEditorViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image.Length > 0)
                {
                    var imageService = new ImageService(_context, _environment, _imageUploadSettings);
                    var image = imageService.SaveToDirectory(model.Image).Result;
                }
                var bookService = new BookService(_context);
                var book = model.MapToBook();
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                model.Id = book.Id;
                _context.Variants.Add(model.MapToVariant());
                if (model.SelectedAuthorIds.Any())
                {
                    foreach (var authorId in model.SelectedAuthorIds)
                    {
                        var bookAuthor = new BookAuthor { AuthorId = int.Parse(authorId), BookId = book.Id };
                        _context.BookAuthors.Add(bookAuthor);
                    }
                }
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(BookController.Index));
            }
            PopulateDropdowns(model);
            return View(model);
        }
        [HttpGet("{id:int}/Basic")]
        public async Task<IActionResult> BookBasic(int id)
        {
            var book = await bookService.GetBookById(id).SingleOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }
            var bookDetails = new BookViewModel()
            {
                Id = book.Id,
                Title = book.Title,
                SubTitle = book.SubTitle,
                Category = book.Category,
                Publisher = book.Publisher,
                Authors = book.BookAuthors.Select(al => al.Author).ToList(),
                Cover = book.Cover
            };

            return View("Views/Basic/Index", bookDetails);
        }

        [HttpGet("{id:int}/Basic/Edit")]
        public async Task<IActionResult> EditBookBasic(int id)
        {
            var book = await bookService.GetBookById(id).AsNoTracking().SingleOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }
            var model = new BookBasicEditorViewModel()
            {
                Id = book.Id,
                GenreId = book.GenreId,
                Title = book.Title,
                SubTitle = book.SubTitle,
                Description = book.Description,
                CategoryId = book.CategoryId,
                PublisherId = book.PublisherId,
                Cover = book.Cover
            };
            await PopulateDropdowns(model);
            return View("Views/Basic/Edit", model);
        }        
        [HttpPost("{id:int}/Basic/Edit")]
        public async Task<IActionResult> EditBookBasic(int id, BookBasicEditorViewModel model)
        {

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(model);
                return View("Views/Basic/Edit", model);
            }
            var book = await bookService.GetBookById(id).SingleOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }
            
            if (model.Image != null && model.Image.Length > 0)
            {
                if(model.Cover != null && !string.IsNullOrEmpty(model.Cover.Path)){
                    var file = new FileInfo(book.Cover.Path);
                    file.Delete();
                    _context.Images.Remove(book.Cover);
                }                
                var cover = await imageService.SaveToDirectory(model.Image);
                book.Cover = cover;
            }

            if (await TryUpdateModelAsync<Book>(book, string.Empty,
                b => b.Title, b => b.SubTitle, b=>b.Description, b => b.CategoryId, b => b.GenreId,
                b => b.PublisherId, b => b.NoInSeries, b => b.Series
            ))
            {
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(BookController.BookBasic));

        }
        [HttpGet("GetSearchContent")]
        public IActionResult GetSearchContent(SortFilterPageOptions options)
        {
            var bookFilterService = new BookFilterDropdownService(_context);
            return Json(bookFilterService.GetFilterDropdownValues((BooksFilterBy)options.FilterBy));
        }

        [HttpGet("GetShelfByLocation/{locationId:int}")]
        public IActionResult GetShelfByLocation(int locationId)
        {
            var shelves = _context.Shelves.OrderBy(s=>s.Name).Where(s => s.LocationId == locationId).ToList();
            return Json(shelves);

        }


        [HttpGet("{id:int}/Types")]
        public async Task<IActionResult> BookTypes(int id, BookTypeLocationSearchViewModel BookTypeLocationSearch)
        {
            var bookTypeListing = new BookTypeListingViewModel();
            var bookTypeLocationSearch = new BookTypeLocationSearchViewModel{
                Locations = _context.Locations.OrderBy(l=>l.Name).ToList()
            };
            bookTypeListing.BookTypeLocationSearch = bookTypeLocationSearch;
            bookTypeListing.BookId = id;
            bookTypeListing.Variants = new List<Variant>();

            if(BookTypeLocationSearch.LocationId.HasValue){
                bookTypeListing.Variants = await bookService
                .GetAllBookVariants(id)
                .Where(v => v.VariantCopies.Where( vc => vc.LocationId == BookTypeLocationSearch.LocationId.Value).Any()
                ).ToListAsync();
                ViewBag.LocationId = BookTypeLocationSearch.LocationId.Value;
            }
            ViewBag.TotalFormats = await termService.GetTermsBySet("book-format").CountAsync();
            return View("Views/Type/Index", bookTypeListing);
        }
        [HttpGet("{id:int}/Locations/{locationId:int}/Types/Create")]
        public async Task<IActionResult> CreateBookType(int id, int locationId)
        {
            var location = await _context.Locations.Include(l=>l.VariantCopies).FirstOrDefaultAsync(l=>l.Id == locationId);            
            if(location == null)
            {
                return NotFound();
            }
            var model = new BookTypeEditorViewModel();
            await SetupCreateBookType(id, location, model);
            return View("Views/Type/Create", model);
        }


        [HttpPost("{id:int}/Locations/{locationId:int}/Types/Create")]
        public async Task<IActionResult> CreateBookType(int id, int locationId, BookTypeEditorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var location = await _context.Locations.Include(l=>l.VariantCopies).FirstOrDefaultAsync(l=>l.Id == locationId);            
                await SetupCreateBookType(id,location,model);
                return View("Views/Type/Create");
            }
            var variant = _mapper.Map<BookTypeEditorViewModel, Variant>(model, opts => opts.BeforeMap((s, d) => s.Id = null));            
            variant.VariantCopies.Add(new VariantCopy
                {
                    LocationId = model.LocationId,
                    AvailabilityId = model.AvailabilityId,
                    SourceId = model.SourceId,
                    SerialNo = $"{variant.Id}{Common.RandomString(10)}",
                    ShelfId = model.ShelfId
            });
            _context.Variants.Add(variant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BookController.BookTypes));
        }

        private async Task SetupCreateBookType(int id, Location location, BookTypeEditorViewModel model)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b=>b.Id == id);
            model.LocationId = location.Id;
            model.Location = location;
            model.BookId = id;
            model.Book = book;
            await PopulateDropdowns(model);
            await NormalizeTypes(id, location.Id, model, false);
        }

        [HttpGet("{id:int}/Types/{typeId:int}/Edit")]
        public async Task<IActionResult> EditBookType(int id, int typeId)
        {
            var variant = await GetVariant(id, typeId);
            var model = _mapper.Map<Variant, BookTypeEditorViewModel>(variant);
            await PopulateDropdowns(model);
            await NormalizeTypes(id, model, true);
            return View("Views/Type/Edit", model);
        }
        [HttpPost("{id:int}/Types/{typeId:int}/Edit")]
        public async Task<IActionResult> EditBookType(int id, int typeId, BookTypeEditorViewModel model)
        {
            var variant = await GetVariant(id, typeId);
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(model);
                await NormalizeTypes(typeId, model, true);
                return View("Views/Types/Edit", model);
            }
            variant.FormatId = model.FormatId;
            variant.Pages = model.Pages.Value;
            variant.YearId = model.YearId;
            variant.DaysAllowedId = model.DaysAllowedId;
            variant.CollectionModeId = model.CollectionModeId;
            variant.FineId = model.FineId;
            _context.Entry(variant).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BookController.BookTypes));
        }

        // [HttpGet("{id:int}/Types/{typeId:int}/Locations")]
        // public async Task<IActionResult> BookLocations(int id, int typeId)
        // {
        //     var locationListing = new BookLocationListingViewModel();
        //     var variant = await GetVariant(id, typeId);
        //     if (variant == null)
        //     {
        //         return NotFound();
        //     }
        //     locationListing.Variant = variant;
        //     locationListing.Copies = variant.VariantCopies.ToList();
        //     return View("Views/Location/Index", locationListing);
        // }
        // [HttpGet("{id:int}/Types/{typeId:int}/Locations/Create")]
        // public async Task<IActionResult> CreateBookLocation(int id, int typeId)
        // {
        //     var variant = await GetVariant(id, typeId);
        //     if (variant == null)
        //     {
        //         return NotFound();
        //     }
        //     var model = new BookLocationEditorViewModel()
        //     {
        //         EditorAttributes = new EditorAttributes { ActionUrl = "CreateBookLocation", Caption = "Add Location" }
        //     };
        //     model.Variant = variant;
        //     model.VariantId = variant.Id;
        //     await PopulateDropdowns(model);
        //     return View("Views/Location/Create", model);
        // }
        // [HttpPost("{id:int}/Types/{typeId:int}/Locations/Create")]
        // public async Task<IActionResult> CreateBookLocation(int id, int typeId, BookLocationEditorViewModel model)
        // {
        //     var variant = await GetVariant(id, typeId);
        //     if (!ModelState.IsValid)
        //     {
        //         model.Variant = variant;
        //         model.VariantId = variant.Id;
        //         await PopulateDropdowns(model);
        //         return View("Views/Location/Create", model);
        //     }
        //     var variantCopy = _mapper.Map<BookLocationEditorViewModel, VariantCopy>(model, opts => opts.BeforeMap((s, d) => { s.Id = null; }));
        //     _context.VariantCopies.Add(variantCopy);
        //     await _context.SaveChangesAsync();

        //     return RedirectToAction(nameof(BookController.BookLocations));
        // }

        [HttpGet("{id:int}/Types/{typeId:int}/Prices")]
        public async Task<IActionResult> BookPrices(int typeId)
        {
            return View();
        }

        [HttpGet("{id:int}/Authors")]
        public IActionResult BookAuthors(int id)
        {
            var bookAuthorListing = new BookAuthorListingViewModel();
            var bookAuthors = _context.BookAuthors
            .Include(b => b.Author)
            .Include(b => b.Book)
            .Where(b => b.BookId == id)
            .ToList();
            bookAuthorListing.BookAuthors = bookAuthors;
            return View("Views/Author/Index", bookAuthorListing);
        }
        [HttpGet("{id:int}/Authors/Create")]
        public async Task<IActionResult> CreateBookAuthors(int id)
        {
            var book = await bookService.GetBookById(id).SingleOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }

            var model = new BookAuthorEditorViewModel();

            model.Book = book;
            var currentAuthors = _context.BookAuthors.Where(b => b.BookId == book.Id).Select(b => b.Author).ToList();
            var authors = _context.Authors.ToList();
            model.Authors = authors.Except(currentAuthors).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = $"{a.LastName}, {a.FirstName}" }).ToList();

            return View("Views/Author/Create", model);
        }

        [HttpPost("{id:int}/Authors/Create")]
        public async Task<IActionResult> CreateBookAuthors(int id, BookAuthorEditorViewModel model)
        {
            var book = await bookService.GetBookById(id).SingleOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                model.Book = book;
                return View("Views/Author/Create", model);
            }
            foreach (var authorId in model.SelectedAuthorIds)
            {
                _context.BookAuthors.Add(new BookAuthor { BookId = book.Id, AuthorId = int.Parse(authorId) });
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BookController.BookAuthors));
        }
        [HttpGet("{id:int}/Authors/{bookAuthorId:int}/Delete")]
        public async Task<IActionResult> DeleteBookAuthor(int id, int bookAuthorId)
        {
            var bookAuthor = await _context.BookAuthors
            .Include(ba => ba.Author)
            .Include(ba => ba.Book)
            .Where(ba => ba.BookId == id && ba.Id == bookAuthorId).SingleOrDefaultAsync();

            if (bookAuthor == null)
            {
                return NotFound();
            }
            var model = new DeleteBookAuthorViewModel { Id = bookAuthor.Id, Book = bookAuthor.Book, Author = bookAuthor.Author };

            return View("Views/Author/Delete", model);
        }
        [HttpPost("{id:int}/Authors/{bookAuthorId:int}/Delete")]
        public async Task<IActionResult> DeleteBookAuthor(int id, int bookAuthorId, DeleteBookAuthorViewModel model)
        {
            var bookAuthor = await _context.BookAuthors.Where(ba => ba.Id == bookAuthorId && ba.BookId == id).SingleOrDefaultAsync();
            _context.BookAuthors.Remove(bookAuthor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(BookController.BookAuthors));
        }

        [HttpPost("{id}/DeleteBookLocation")]
        public IActionResult DeleteBookLocation(int id)
        {
            var location = _context.VariantCopies.Find(id);
            if (location == null) return Json(new DeleteStatusViewModel { Successful = false });
            bookService.DeleteBookLocation(location);
            return Json(new StatusViewModel { Successful = true });
        }

        private void PopulateDropdowns(BookEditorViewModel model)
        {
            var authorService = new AuthorService(_context);

            model.BookFormats = termService.GetTermsBySet("book-format").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.Languages = termService.GetTermsBySet("book-language").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.Categories = termService.GetTermsBySet("book-category").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.Prices = termService.GetTermsBySet("book-price").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.Years = termService.GetTermsBySet("book-year").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.CollectionModes = termService.GetTermsBySet("book-collection-mode").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();

            model.Locations = _context.Locations.OrderBy(l => l.Name).AsNoTracking().MapToSelectList().ToList();
            model.Grants = termService.GetTermsBySet("book-sale-grant").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.Days = termService.GetTermsBySet("book-days-allowed").AsNoTracking().OrderBy(t => t.Name).MapToSelectList(1);
            model.Availabilities = termService.GetTermsBySet("book-availability").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.Genres = termService.GetTermsBySet("genre").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.Publishers = publisherService.GetAllPublishers().OrderBy(t => t.Name).MapToSelectList().ToList();
            model.Authors = authorService.GetAllAuthors().MapToSelectList();
            model.Conditions = termService.GetTermsBySet("book-condition").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.Fines = termService.GetTermsBySet("book-fine").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.BookSources = termService.GetTermsBySet("book-source").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();

            //box-shadow: inset 0 1px 0 #66bfff;
        }
        private async Task PopulateDropdowns(BookLocationEditorViewModel model)
        {
            model.Sources = termService.GetTermsBySet("book-source").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.Availables = termService.GetTermsBySet("book-availability").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();

            var existingLocations = model.Variant.VariantCopies.Select(vl => vl.Location).ToList();
            var allLocations = await _context.Locations.ToListAsync();

            model.Locations = allLocations.Except(existingLocations).MapToSelectList().ToList();
        }
        private async Task PopulateDropdowns(BookBasicEditorViewModel model)
        {
            model.Publishers = await publisherService.GetAllPublishers().AsNoTracking().OrderBy(t => t.Name).MapToSelectList().ToListAsync();
            model.Genres = termService.GetTermsBySet("genre").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.Categories = termService.GetTermsBySet("book-category").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
        }
        private async Task PopulateDropdowns(BookTypeEditorViewModel model)
        {
            var formats = await termService.GetTermsBySet("book-format").AsNoTracking().ToListAsync();
            model.BookFormats = formats.MapToSelectList();
            model.Years = termService.GetTermsBySet("book-year").AsNoTracking().OrderByDescending(t => t.Name).MapToSelectList();
            model.Days = termService.GetTermsBySet("book-days-allowed").AsNoTracking().OrderByDescending(t => t.Name).MapToSelectList();
            model.CollectionModes = termService.GetTermsBySet("book-collection-mode").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.Fines = termService.GetTermsBySet("book-fine").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.Languages = termService.GetTermsBySet("book-language").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
            model.Availabilities = termService.GetTermsBySet("book-availability").AsNoTracking().OrderBy(t=>t.Name).MapToSelectList();
            model.BookSources = termService.GetTermsBySet("book-source").AsNoTracking().OrderBy(t=>t.Name).MapToSelectList();
            model.Shelves = _context.Shelves.Where(s => s.LocationId == model.LocationId).ToList();
            model.Grants = termService.GetTermsBySet("book-sale-grant").AsNoTracking().OrderBy(t => t.Name).MapToSelectList();
        }
        private async Task<Variant> GetVariant(int id, int typeId)
        {
            return await bookService
            .GetBookByVariantId(id)
            .Where(v => v.BookId == id)
            .SingleOrDefaultAsync();
        }

        private async Task NormalizeTypes(int id, BookTypeEditorViewModel model, bool filter = false)
        {

            var formats = await termService.GetTermsBySet("book-format").ToListAsync();

            var variants = await bookService
                                .GetBookById(id)
                                .Select(b => b.Variants)
                                .SingleOrDefaultAsync();

            var currentFormatIds = variants.Select(b => b.FormatId).ToList();

            var ids = formats.Select(f => f.Id).ToList();
            ids = ids.Except(currentFormatIds).ToList();

            if (filter)
            {
                var filteredId = variants.Where(v => v.Id == model.Id).Select(v => v.FormatId).FirstOrDefault();
                ids.Add(filteredId);
            }

            model.BookFormats = formats.Where(f => ids.Contains(f.Id)).MapToSelectList();
        }

        private async Task NormalizeTypes(int id, int locationId, BookTypeEditorViewModel model, bool filter = false)
        {

            var formats = await termService.GetTermsBySet("book-format").ToListAsync();

            var variants = await bookService
                                .GetBookById(id)
                                .Select(b => b.Variants.Where(v=>v.VariantCopies.Where(vc=>vc.LocationId == locationId).Any()).ToList())
                                .SingleOrDefaultAsync();

            var currentFormatIds = variants.Select(b => b.FormatId).ToList();

            var ids = formats.Select(f => f.Id).ToList();
            ids = ids.Except(currentFormatIds).ToList();

            if (filter)
            {
                var filteredId = variants.Where(v => v.Id == model.Id).Select(v => v.FormatId).FirstOrDefault();
                ids.Add(filteredId);
            }

            model.BookFormats = formats.Where(f => ids.Contains(f.Id)).MapToSelectList();
        }
        
    }
}