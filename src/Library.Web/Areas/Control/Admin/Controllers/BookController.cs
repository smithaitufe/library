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
    [Authorize(Policy="AdministratorOnly")]
    [Route("Admin/Book")]
    public class BookController: Controller
    {
        private readonly LibraryDbContext _context;
        private readonly IHostingEnvironment _environment;
        private readonly ImageUploadSettings _imageUploadSettings;
        private readonly BookService bookService ;
        private readonly TermService termService;
        private readonly PublisherService publisherService;
        private readonly IMapper _mapper;
        public BookController(LibraryDbContext context, IHostingEnvironment environment, IOptions<ImageUploadSettings> imageUploadSettings, IMapper mapper)
        {
            _context = context;
            _environment = environment;
            _imageUploadSettings = imageUploadSettings.Value;
            _mapper = mapper;
            bookService = new BookService(context);
            termService = new TermService(context);
            publisherService  = new PublisherService(context);            
        }
        [HttpGet("")]
        public IActionResult Index(SortFilterPageOptions sortFilterPageOptions, SearchBookOptions searchOptions) 
        {      
            // var books = bookService.SortFilterPage(sortFilterPageOptions).ToList();
            // var model = new BookListViewModel(books, searchOptions, sortFilterPageOptions);
            // model.Locations = _context.Locations.OrderBy(t=>t.Name).MapToSelectList().ToList();
            // return View("~/Areas/Control/Admin/Views/Book/Index.cshtml", model);              

            var books = bookService.GetAllBooks().ToList().OrderBy(b=> b.Title).OrderByDescending(b=>b.InsertedAt).ToList();            
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
        public async Task<IActionResult> Create([Bind("Title,SubTitle,Edition,Volume,Description,Pages,ISBN,Image,PublisherId,SelectedAuthorIds,CategoryId,FormatId,LanguageId,GenreId,YearId,CollectionModeId,ConditionId,PriceId,GrantId,LocationId,AvailabilityId,SourceId,FineId,DaysAllowedId, PublisherId")]BookEditorViewModel model) 
        {
            var imageService = new ImageService(_context, _environment, _imageUploadSettings);
            if(!ModelState.IsValid){ 
                PopulateDropdowns(model);            
                return View(model);
            }

            var book = model.MapToBook();   
            var cover = await imageService.SaveToDirectory(model.Image);             
            book.Cover = cover;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();  
                        
            var variant = model.MapToVariant();
            variant.BookId = book.Id;
            _context.Variants.Add(variant);
            await _context.SaveChangesAsync();
            var variantPrice = new VariantPrice 
            { 
                VariantId = variant.Id, 
                ConditionId = model.ConditionId,  
                PriceId = model.PriceId
            };
            _context.VariantPrices.Add(variantPrice);                        
            await _context.SaveChangesAsync();

            if(model.SelectedAuthorIds.Any()) {
                foreach (var authorId in model.SelectedAuthorIds) {
                    int.TryParse(authorId, out int id);
                    var bookAuthor = new BookAuthor { AuthorId = id, BookId = book.Id };
                    _context.BookAuthors.Add(bookAuthor);  
                    await _context.SaveChangesAsync();                      
                }
            }


            // Add the location            

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
            book.SelectedAuthorIds = bookService.GetBookAuthors(id).Select( a => a.Id.ToString()).ToArray<string>();            
            PopulateDropdowns(book);
            return View(book);
        }
        [HttpPost("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, BookEditorViewModel model) 
        {
            if(ModelState.IsValid){
                if(model.Image.Length > 0){
                    var imageService = new ImageService(_context, _environment, _imageUploadSettings);
                    var image = imageService.SaveToDirectory(model.Image).Result;
                }
                var bookService = new BookService(_context);
                var book = model.MapToBook();
                _context.Books.Add(book);
                await _context.SaveChangesAsync();  
                model.Id = book.Id;                                           
                _context.Variants.Add(model.MapToVariant());                               
                if(model.SelectedAuthorIds.Any()) {                  
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
            if(book == null)
            {
                return NotFound();
            }
            var bookDetails = new BookViewModel ()
            {
                Id = book.Id,
                Title = book.Title,
                SubTitle = book.SubTitle,
                // ISBN = book.ISBN,
                Category = book.Category,
                Publisher = book.Publisher,
                Authors = book.AuthorsLink.Select(al => al.Author).ToList(), 
                Cover = book.Cover                               
            };
       
            return View("Views/Basic/Index", bookDetails);
        }

        [HttpGet("{id:int}/Basic/Edit")]
        public async Task<IActionResult> EditBookBasic(int id)
        {
            var book = await bookService.GetBookById(id).SingleOrDefaultAsync();
            if(book == null)
            {
                return NotFound();
            }

            var model = new BookBasicEditorViewModel()
            {
                
                Id = book.Id,
                GenreId = book.GenreId,                
                Title = book.Title,
                SubTitle = book.SubTitle,
                // ISBN = book.ISBN,
                CategoryId = book.CategoryId,
                PublisherId = book.PublisherId,                 
                Cover = book.Cover,
            };
            await PopulateDropdowns(model);
            return View("Views/Basic/Edit", model);
        }

        [HttpPost("{id:int}/Basic/Edit")]
        public async Task<IActionResult> EditBookBasic(int id, BookBasicEditorViewModel model)
        {
            
            if(!ModelState.IsValid)
            {
                await PopulateDropdowns(model);
                return View("Views/Basic/Edit", model);
            }
            var book = await bookService.GetBookById(id).SingleOrDefaultAsync();
            if(book == null)
            {
                return NotFound();
            }
            book = _mapper.Map<BookBasicEditorViewModel, Book>(model);
            _context.Attach(book);
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BookController.BookBasic));

            
        }
        [HttpGet("GetSearchContent")]
        public IActionResult GetSearchContent(SortFilterPageOptions options) 
        {
            var bookFilterService = new BookFilterDropdownService(_context);
            return Json(bookFilterService.GetFilterDropdownValues((BooksFilterBy)options.FilterBy));
        }
        

        [HttpGet("{id:int}/Types")]
        public async Task<IActionResult> BookTypes(int id)
        {
            var bookTypeListing = new BookTypeListingViewModel();
            bookTypeListing.Variants = await bookService.GetAllBookVariants(id).ToListAsync();
            return View("Views/Type/Index", bookTypeListing);
        }
        [HttpGet("{id:int}/Types/Create")]
        public async Task<IActionResult> CreateBookType(int id)
        {
            var model = new BookTypeEditorViewModel();
            await PopulateDropdowns(model);

            return View("Views/Type/Create", model);
        }
        [HttpPost("{id:int}/Types/Create")]
        public async Task<IActionResult> CreateBookType(int id, BookTypeEditorViewModel model)
        {
            if(!ModelState.IsValid)
            {
                await PopulateDropdowns(model);
                return View("Views/Type/Create");
            }
            var variant = _mapper.Map<BookTypeEditorViewModel, Variant>(model, opts => opts.BeforeMap((s,d) => s.Id = null ));
            _context.Variants.Add(variant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BookController.BookTypes));
        }

        [HttpGet("{id:int}/Types/{typeId:int}/Edit")]
        public async Task<IActionResult> EditBookType(int id, int typeId)
        {            
            var variant = await GetVariant(id,typeId);
            var model = _mapper.Map<Variant, BookTypeEditorViewModel>(variant);
            await PopulateDropdowns(model);
            return View("Views/Type/Edit", model);
        }
        [HttpPost("{id:int}/Types/{typeId:int}/Edit")]
        public async Task<IActionResult> EditBookType(int id, int typeId, BookTypeEditorViewModel model)
        {
            var variant = await GetVariant(id, typeId);
            if(!ModelState.IsValid)
            {
                return View("Views/Types/Edit", model);
            }
            // variant = _mapper.Map<BookTypeEditorViewModel, Variant>(model);
            // _context.Attach(variant);
            // await _context.SaveChangesAsync();
            
            variant.FormatId = model.FormatId;
            variant.Pages = model.Pages;
            variant.YearId = model.YearId;
            variant.DaysAllowedId = model.DaysAllowedId;
            variant.CollectionModeId = model.CollectionModeId;
            variant.FineId = model.FineId;
            _context.Entry(variant).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BookController.BookTypes));
        }

        [HttpGet("{id:int}/Types/{typeId:int}/Locations")]
        public async Task<IActionResult> BookLocations (int id, int typeId)
        {
            var locationListing = new BookLocationListingViewModel();
            var variant = await GetVariant(id, typeId);
            if(variant == null)
            {
                return NotFound();                
            }
            locationListing.Variant = variant;
            locationListing.Locations = variant.VariantLocations.ToList();
            return View("Views/Location/Index", locationListing);
        }
        [HttpGet("{id:int}/Types/{typeId:int}/Locations/Create")]
        public async Task<IActionResult> CreateBookLocation(int id, int typeId)
        {
            var variant = await GetVariant(id, typeId);
            if(variant == null)
            {
                return NotFound();
            }
            var model = new BookLocationEditorViewModel()
            {
             EditorAttributes = new EditorAttributes { ActionUrl = "CreateBookLocation", Caption = "Add Location" }   
            };
            model.Variant = variant;
            model.VariantId = variant.Id;
            await PopulateDropdowns(model);
            return View("Views/Location/Create", model);
        }
        [HttpPost("{id:int}/Types/{typeId:int}/Locations/Create")]
        public async Task<IActionResult> CreateBookLocation(int id, int typeId, BookLocationEditorViewModel model)
        {
            var variant = await GetVariant(id, typeId);
            if(!ModelState.IsValid)
            {
                model.Variant = variant;
                model.VariantId = variant.Id;
                await PopulateDropdowns(model);
                return View("Views/Location/Create", model);
            }
            var variantLocation = _mapper.Map<BookLocationEditorViewModel, VariantLocation>(model, opts => opts.BeforeMap((s,d) => { s.Id = null; }));
            _context.VariantLocations.Add(variantLocation);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(BookController.BookLocations));
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
            if(book == null)
            {
                return NotFound();
            }

            var model = new BookAuthorEditorViewModel();
            
            model.Book = book;
            var currentAuthors = _context.BookAuthors.Where(b => b.BookId == book.Id).Select(b=>b.Author).ToList();
            var authors = _context.Authors.ToList();
            model.Authors = authors.Except(currentAuthors).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = $"{a.LastName}, {a.FirstName}" } ).ToList();

            return View("Views/Author/Create", model);
        }

        [HttpPost("{id:int}/Authors/Create")]
        public async Task<IActionResult> CreateBookAuthors(int id, BookAuthorEditorViewModel model) 
        {
            var book = await bookService.GetBookById(id).SingleOrDefaultAsync();
            if(book == null)
            {
                return NotFound();
            }
            if(!ModelState.IsValid)
            {                
                model.Book = book;
                return View("Views/Author/Create", model);
            }            
            foreach(var authorId in model.SelectedAuthorIds){
               _context.BookAuthors.Add(new BookAuthor { BookId = book.Id, AuthorId = int.Parse(authorId) });
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BookController.BookAuthors));
        }
        [HttpGet("{id:int}/Authors/{bookAuthorId:int}/Delete")]
        public async Task<IActionResult> DeleteBookAuthor(int id, int bookAuthorId)
        {
            var bookAuthor = await _context.BookAuthors
            .Include(ba=>ba.Author)
            .Include(ba=>ba.Book)
            .Where(ba=>ba.BookId == id && ba.Id == bookAuthorId).SingleOrDefaultAsync();

            if(bookAuthor == null)
            {
                return NotFound();
            }
            var model = new DeleteBookAuthorViewModel { Id = bookAuthor.Id, Book = bookAuthor.Book, Author = bookAuthor.Author};
            
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
        

        // [HttpGet("{id}/Types/{typeId:int}/Editions")]
        // public async Task<IActionResult> BookEditions(int id, int typeId) 
        // {
        //     var variant = await GetVariant(id, typeId);

        //     var model = new BookEditionListingViewModel();
        //     model.Variant = variant;
        //     model.Editions = variant.Editions.ToList();
        //     return View("Views/Edition/Index", model);
        // }

        // [HttpGet("{id:int}/Types/{typeId:int}/Editions/Create")]
        // public async Task<IActionResult> CreateBookEdition(int id, int typeId) 
        // {
        //     var variant = await GetVariant(id, typeId);            
        //     if(variant == null)
        //     {
        //         return NotFound();
        //     }
        //     var model = new BookEditionEditorViewModel() 
        //     { 
        //         VariantId = variant.Id, 
        //         Variant = variant, 
        //         EditorAttributes = new EditorAttributes { ActionUrl = "CreateBookEdition", Caption = "Create Book Edition"} 
        //     };            
        //     return View("Views/Edition/Create", model);
        // }
        // [HttpPost("{id:int}/Types/{typeId:int}/Editions/Create")]
        // public async Task<IActionResult> CreateBookEdition(int id, int typeId, BookEditionEditorViewModel model) 
        // {
        //     var variant = await GetVariant(id, typeId);
        //     if(variant == null)
        //     {
        //         return NotFound();
        //     }

        //     if(!ModelState.IsValid)
        //     {
        //         model.Variant = variant;
        //         model.EditorAttributes = new EditorAttributes { ActionUrl = "CreateBookEdition", Caption = "Create Book Edition"};
        //         return View("Views/Edition/Create", model);
        //     }

        //     var edition = _mapper.Map<BookEditionEditorViewModel, Edition>(model, opts => opts.BeforeMap((s,d) => { s.Id = null; } ));
        //     _context.Editions.Add(edition);
        //     await _context.SaveChangesAsync();

        //     return RedirectToAction(nameof(BookController.BookEditions));            
        // }

        // [HttpGet("{id:int}/Types/{typeId:int}/Editions/{editionId:int}/Edit")]
        // public async Task<IActionResult> EditBookEdition(int id, int typeId, int editionId) 
        // {
        //     var variant = await GetVariant(id, typeId);
        //     if(variant == null)
        //     {
        //         return NotFound();
        //     }
        //     var edition = variant.Editions.Where(e=>e.Id == editionId).SingleOrDefault();
        //     if(edition == null)
        //     {
        //         return NotFound();
        //     }
        //     var model = _mapper.Map<Edition, BookEditionEditorViewModel>(edition);
        //     model.EditorAttributes = new EditorAttributes { ActionUrl = "EditBookEdition"};                      
        //     return View("Views/Edition/Edit", model);
        // }

        // [HttpPost("{id:int}/Types/{typeId:int}/Editions/{editionId:int}/Edit")]
        // public async Task<IActionResult> EditBookEdition(int id, int typeId, int editionId, BookEditionEditorViewModel model) 
        // {
        //     var variant =  await GetVariant(id, typeId);
        //     if(variant == null)
        //     {
        //         return NotFound();
        //     }

        //     if(!ModelState.IsValid)
        //     {
        //         model.Variant = variant;
        //         model.EditorAttributes = new EditorAttributes { ActionUrl = "EditBookEdition"};
        //         return View("Views/Edition/Edit", model);
        //     }
        //     var edition = _context.Set<Edition>().SingleOrDefault(e=>e.Id == editionId);
        //     // edition = _mapper.Map<BookEditionEditorViewModel, Edition>(model, opts=>opts.AfterMap((s,d)=> {
        //     //     d.UpdatedAt = DateTime.Now;
        //     //     d.Id = editionId;
        //     // }));
        //     // _context.Editions.Attach(edition);
        //     edition.Name = model.Name;
        //     edition.UpdatedAt = DateTime.Now;
        //     _context.Entry(edition).State = EntityState.Modified;
        //     await _context.SaveChangesAsync();

        //     return RedirectToAction(nameof(BookController.BookEditions));            
        // }
        // [HttpGet("{id:int}/Types/{typeId:int}/Volumes")]
        // public async Task<IActionResult> BookVolumes(int id, int typeId)
        // {
        //     var variant = await GetVariant(id, typeId);
        //     var model = new BookVolumeListingViewModel();
        //     model.Variant = variant;
        //     model.Volumes = variant.Volumes.ToList();
        //     return View("Views/Volume/Index", model);
        // }

        // [HttpGet("{id:int}/Types/{typeId:int}/Volumes/Create")]
        // public async Task<IActionResult> CreateBookVolume(int id, int typeId)
        // {
        //     var variant = await GetVariant(id, typeId);
        //     if(variant == null)
        //     {
        //         return NotFound();
        //     }
        //     var model = new BookVolumeEditorViewModel() 
        //     { 
        //         VariantId = variant.Id, 
        //         Variant = variant, 
        //         EditorAttributes = new EditorAttributes { ActionUrl = "CreateBookVolume", Caption = "Create Volume"} 
        //     };            
        //     return View("Views/Volume/Create", model);
        // }
        
        // [HttpPost("{id:int}/Types/{typeId:int}/Volumes/Create")]
        // public async Task<IActionResult> CreateBookVolume(int id, int typeId, BookVolumeEditorViewModel model)
        // {
        //     var variant = await GetVariant(id, typeId);
        //     if(variant == null)
        //     {
        //         return NotFound();
        //     }

        //     if(!ModelState.IsValid)
        //     {
        //         model.Variant = variant;
        //         model.VariantId = variant.Id;
        //         return View("Views/Volume/Create", model);
        //     }

        //     var volume = _mapper.Map<BookVolumeEditorViewModel, Volume>(model, opts => opts.BeforeMap((s,d) => { s.Id = null; }));
        //     _context.Volumes.Add(volume);
        //     await _context.SaveChangesAsync();

        //     return RedirectToAction(nameof(BookController.BookVolumes));            
        // }

        // [HttpGet("{id:int}/Types/{typeId:int}/Volumes/{volumeId:int}/Edit")]
        // public async Task<IActionResult> EditBookVolume(int id, int typeId, int volumeId) 
        // {
        //     var variant = await GetVariant(id, typeId);
        //     if(variant == null)
        //     {
        //         return NotFound();
        //     }
        //     var volume = variant.Volumes.Where(v=>v.Id == volumeId).SingleOrDefault();
        //     if(volume == null)
        //     {
        //         return NotFound();
        //     }

        //     var model = _mapper.Map<Volume, BookVolumeEditorViewModel>(volume);
        //     model.EditorAttributes = new EditorAttributes { ActionUrl = "EditBookVolume", Caption = "Edit Volume"};                      
        //     return View("Views/Volume/Edit", model);
        // }
        // [HttpPost("{id:int}/Types/{typeId:int}/Volumes/{volumeId:int}/Edit")]
        // public async Task<IActionResult> EditBookVolume(int id, int typeId, int volumeId, BookVolumeEditorViewModel model) 
        // {
        //     var variant = await GetVariant(id, typeId);
        //     if(variant == null)
        //     {
        //         return NotFound();
        //     }

        //     if(!ModelState.IsValid)
        //     {
        //         model.Variant = variant;                
        //         return View("Views/Volume/Edit", model);
        //     }

        //     //var volume = _mapper.Map<BookVolumeEditorViewModel, Volume>(model, opts=>opts.BeforeMap((s,d)=> s.Id = volumeId));
        //     // _context.Volumes.Attach(volume);

        //     var volume = _context.Set<Volume>().SingleOrDefault(v=>v.Id == volumeId);
        //     volume.Name = model.Name;
        //     volume.UpdatedAt = DateTime.Now;            
        //     _context.Entry(volume).State = EntityState.Modified;
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(BookController.BookVolumes));            
        // }
        
        [HttpPost("{id}/DeleteBookLocation")]
        public IActionResult DeleteBookLocation(int id) 
        {
            var location = _context.VariantLocations.Find(id);
            if(location == null) return Json(new DeleteStatusViewModel{ Successful = false });
            bookService.DeleteBookLocation(location);
            return Json(new StatusViewModel{ Successful = true });
        }
        

        private void PopulateDropdowns(BookEditorViewModel model) 
        {            
            var authorService = new AuthorService(_context);
            
            model.BookFormats = termService.GetTermsBySet("book-format").OrderBy(t=>t.Name).MapToSelectList();
            model.Languages = termService.GetTermsBySet("book-language").OrderBy(t=>t.Name).MapToSelectList();
            model.Categories = termService.GetTermsBySet("book-category").OrderBy(t=>t.Name).MapToSelectList();
            model.Prices = termService.GetTermsBySet("book-price").OrderBy(t=>t.Name).MapToSelectList();
            model.Years = termService.GetTermsBySet("book-year").OrderBy(t=>t.Name).MapToSelectList();
            model.CollectionModes = termService.GetTermsBySet("book-collection-mode").OrderBy(t=>t.Name).MapToSelectList();
            model.Locations = termService.GetTermsBySet("book-location").OrderBy(t=>t.Name).MapToSelectList();
            model.Locations = _context.Locations.OrderBy(l => l.Name).MapToSelectList().ToList();
            model.Grants = termService.GetTermsBySet("book-sale-grant").OrderBy(t=>t.Name).MapToSelectList();
            model.Days = termService.GetTermsBySet("book-days-allowed").OrderBy(t=>t.Name).MapToSelectList(1);
            model.Availabilities = termService.GetTermsBySet("book-availability").OrderBy(t=>t.Name).MapToSelectList();
            model.Genres = termService.GetTermsBySet("genre").OrderBy(t=>t.Name).MapToSelectList();
            model.Publishers = publisherService.GetAllPublishers().OrderBy(t=>t.Name).MapToSelectList().ToList();
            model.Authors = authorService.GetAllAuthors().MapToSelectList();
            model.Conditions = termService.GetTermsBySet("book-condition").OrderBy(t=>t.Name).MapToSelectList();
            model.Fines = termService.GetTermsBySet("book-fine").OrderBy(t=>t.Name).MapToSelectList();
            model.BookSources = termService.GetTermsBySet("book-source").OrderBy(t=>t.Name).MapToSelectList();
        }

        private async Task PopulateDropdowns(BookLocationEditorViewModel model) 
        {
            model.Sources = termService.GetTermsBySet("book-source").OrderBy(t=>t.Name).MapToSelectList();
            model.Availables = termService.GetTermsBySet("book-availability").OrderBy(t=>t.Name).MapToSelectList();
            var existingLocations = model.Variant.VariantLocations.Select(vl => vl.Location).ToList();
            var allLocations = await _context.Locations.ToListAsync();
            model.Locations = allLocations.Except(existingLocations).MapToSelectList().ToList();
        }
        private async Task PopulateDropdowns(BookBasicEditorViewModel model)
        {
            model.Publishers = await publisherService.GetAllPublishers().OrderBy(t=>t.Name).MapToSelectList().ToListAsync();
            model.Genres = termService.GetTermsBySet("genre").OrderBy(t=>t.Name).MapToSelectList();
            model.Categories = termService.GetTermsBySet("book-category").OrderBy(t=>t.Name).MapToSelectList();                       
        }
        private async Task PopulateDropdowns(BookTypeEditorViewModel model)
        {
            var formats = termService.GetTermsBySet("book-format").ToList();
            var currentFormats = await _context.Variants.Select(v => v.Format).ToListAsync();
            var unUsedFormats = formats.Except(currentFormats).ToList();
            // unUsedFormats.Add(model.Variant.Format);            
            model.BookFormats = unUsedFormats.MapToSelectList();
            model.Years = termService.GetTermsBySet("book-year").OrderByDescending(t=>t.Name).MapToSelectList();
            model.Days = termService.GetTermsBySet("book-days-allowed").OrderByDescending(t=>t.Name).MapToSelectList();            
            model.CollectionModes = termService.GetTermsBySet("book-collection-mode").OrderBy(t=>t.Name).MapToSelectList();
            model.Fines = termService.GetTermsBySet("book-fine").OrderBy(t=>t.Name).MapToSelectList();
        }
        private async Task<Variant> GetVariant(int id, int typeId)
        {
            return await bookService
            .GetBookByVariantId(id)
            .Where(v=>v.BookId == id)
            .SingleOrDefaultAsync();
        }

        
    }
}