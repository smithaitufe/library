using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Web.Code;
using Library.Core.Models;
using Library.Web.Extensions;
using Library.Web.Models.AuthorViewModels;
using Library.Web.Models.BookViewModels;
using Library.Repo;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Services
{
    public class BookService
    {
        LibraryDbContext _context;
        public BookService(LibraryDbContext context) {
            _context = context;            
        }
        private IQueryable<Book> Books => _context.Books
            .Include(b => b.Cover)
            .Include(b=>b.Genre)
            .Include(b=>b.Category)
            .Include(b=>b.Publisher)
            .Include(b=>b.BookAuthors)
            .ThenInclude(al=>al.Author)
            .Include(b=>b.Variants)
            .ThenInclude(v => v.Format)
            .Include(b=>b.Variants)
            .ThenInclude(v => v.Year)
            .Include(b=>b.Variants)
            .ThenInclude(v => v.CollectionMode)                
            .Include(b=>b.Variants)
            .ThenInclude(v => v.Fine)
            .Include(b=>b.Variants)
            .ThenInclude(v => v.DaysAllowed)
            .Include(b=>b.Variants)
            .ThenInclude(v => v.VariantCopies);

        private IQueryable<Variant> Variants => _context.Variants
            .Include(v => v.Format)
            .Include(v => v.Year)
            .Include(v => v.CollectionMode)                
            .Include(v => v.Fine)
            .Include(v => v.DaysAllowed)
            .Include(v => v.VariantCopies)
            .ThenInclude( l =>l.Availability)
            .Include(v => v.VariantCopies)
            .ThenInclude(l => l.Source)
            .Include(v => v.VariantCopies)
            .ThenInclude(vl => vl.Location)
            .Include(v => v.Grant)
            .Include(v => v.Book)
            .ThenInclude(b => b.Genre)
            .Include(v => v.Book)
            .ThenInclude(book => book.BookAuthors)
            .ThenInclude(authorLink => authorLink.Author)
            .Include(v => v.VariantPrices)
            .ThenInclude( pl => pl.Price)
            .Include(v => v.VariantPrices)
            .ThenInclude( pl => pl.Condition)
            .Include(v => v.Book)
            .ThenInclude(b=>b.Cover);
        
        private IQueryable<CheckOut> CheckOuts => _context.CheckOuts
                .Include(co => co.Patron)
                .Include( c => c.RequestedDays)
                .Include( c => c.ApprovedDays)
                .Include(co => co.VariantCopy)
                .ThenInclude(vc=>vc.Location)
                .Include(co => co.VariantCopy)
                .ThenInclude(vc=>vc.Source)                
                .Include( c => c.VariantCopy)
                .ThenInclude(vc => vc.Variant)
                .ThenInclude(v=>v.Format)
                .Include(co => co.CheckOutStates)
                .ThenInclude(cos => cos.Status)
                .ThenInclude(s => s.Parent)
                .Include(co => co.CheckOutStates)
                .ThenInclude(cos => cos.ModifiedBy)
                .Include(c => c.VariantCopy)
                .ThenInclude(vc => vc.Variant)
                .ThenInclude(bv => bv.Book)
                .ThenInclude(b => b.BookAuthors)
                .ThenInclude(al => al.Author)
                .Include(c => c.VariantCopy)
                .ThenInclude(vc => vc.Variant)
                .ThenInclude(bv => bv.Format)
                .Include(c => c.VariantCopy)
                .ThenInclude(vc => vc.Variant)
                .ThenInclude(bv => bv.Book)
                .ThenInclude(b => b.Publisher)                
                .Include(c => c.VariantCopy)
                .ThenInclude(vc => vc.Variant)  
                .ThenInclude(bv => bv.Book)              
                .ThenInclude(bv => bv.Genre);

                // .ThenInclude(v => v.DaysAllowed)
                // .Include(co => co.Variant)
                // .ThenInclude(v => v.VariantCopies)
                // .ThenInclude(l => l.Availability)
                // .Include(co => co.Variant)
                // .ThenInclude(v => v.VariantCopies)
                // .ThenInclude(l => l.Source)
                // .Include(co => co.Variant)
                // .ThenInclude(v => v.VariantCopies)
                // .ThenInclude(l => l.Location)
                // .Include(co => co.Variant)
                // .ThenInclude(v => v.Grant)
                // .Include(co => co.Variant)                
                // .ThenInclude(v => v.Book)
                // .ThenInclude(b => b.Genre)
                // .Include(co => co.Variant)
                // .ThenInclude(v => v.Book)
                // .ThenInclude(b => b.BookAuthors)
                // .ThenInclude(al => al.Author)
                // .Include(co => co.Variant)
                // .ThenInclude(v => v.Book)
                // .ThenInclude(b => b.Publisher)
                // .Include(co => co.Variant)
                // .ThenInclude(v => v.PricesLink)
                // .ThenInclude( pl => pl.Price)
                // .Include(co => co.Variant)
                // .ThenInclude(v => v.PricesLink)
                // .ThenInclude( pl => pl.Condition);
        
        private IQueryable<VariantCopy> VariantCopies => 
            _context.VariantCopies
            .Include(vc=>vc.Location)
            .Include(vc=>vc.Source)                                                        
            .Include(vc=>vc.Variant)
            .ThenInclude(v=>v.Book)
            .ThenInclude(b=>b.Cover)
            .Include(vc=>vc.Variant)
            .ThenInclude(v=>v.Book)
            .ThenInclude(b=>b.BookAuthors)
            .ThenInclude(ba=>ba.Author)
            .Include(vc=>vc.Variant)
            .ThenInclude(v=>v.Book)
            .ThenInclude(b=>b.Publisher)
            .Include(vc => vc.Variant)
            .ThenInclude(v=>v.Format)
            ;
        
        
        public IQueryable<Book> GetAllBooks()
        {
            var books = from b in Books 
                        select b;
            return books;
        }
        public IQueryable<Book> GetBookById(int id)
        {
            return Books.Where(b => b.Id == id);
        }
        public IQueryable<Variant> GetAllVariants() 
        {
            return Variants;
        }
        public IQueryable<VariantCopy> GetAllVariantCopies()
        {
            return VariantCopies;
        }
        public IQueryable<Variant> GetAllBookVariants(int bookId) 
        {
            return Variants.Where(b => b.BookId == bookId);
        }
        public IQueryable<Variant> GetBookByVariantId(int id) 
        {
            return Variants.Where(v => v.Id == id);                   
        }
        public IQueryable<Variant> GetBooksByLocationId(IQueryable<Variant> variants, int locationId) 
        {
            return variants.Where(v => v.VariantCopies.Where( l => l.LocationId == locationId).Any());
        }
        public IQueryable<Variant> GetBooksByTitle(IQueryable<Variant> variants, string title) 
        {
            return variants.Where(v => v.Book.Title.ToLower().Contains(title.ToLower()));
        }
        public IQueryable<Variant> GetBooksByCategory(int categoryId) {
            return Variants.Where( v => v.Book.CategoryId == categoryId);
        }
        public IQueryable<BookViewModel> SearchBooks(string phrase) {
            var query = from v in _context.Variants select v;
            if(!string.IsNullOrEmpty(phrase))
                query = query.Where(v => v.Book.Title.Contains(phrase));
            return query.MapToBookViewModel();
        }
        public IQueryable<BookViewModel> SortFilterPage(SortFilterPageOptions options) {
            var booksQuery = _context.Variants
            .AsNoTracking()
            .MapToBookViewModel()
            .OrderBooksBy(options.OrderByOptions)
            .FilterBooksBy(options.FilterBy, options.FilterValue);
            options.SetupRestOfDto(booksQuery);            
            return booksQuery;
        }

        public IQueryable<CheckOut> GetCheckOutBooks(string status = null){        
            var query = from co in CheckOuts select co;
            if(!string.IsNullOrEmpty(status)) {
                query = query
                .Where(c => c.CheckOutStates
                    .OrderByDescending(cs=>cs.Id).Take(1)
                    .Where(cos => cos.Status.Name.ToLower().Equals(status.ToLower()))                    
                    .Any()
                );
            }            
            return query;        
        }
        public IQueryable<CheckOut> GetCheckoutsForUser(int userId) 
        {
            var query = from x in CheckOuts select x;
            return query.AsNoTracking().Where(co => co.PatronId == userId);
        }
        public List<AuthorViewModel> GetBookAuthors(int id) 
        {
            return Variants.Where(bv => bv.Id == id).ToList()
                            .SelectMany(bv => bv.Book.BookAuthors.Select(al => al.Author).MapToAuthorViewModel())
                            .ToList();              
        }
        public async void DeleteAuthorsByBookId(int bookId) 
        {
            var bookAuthors = _context.BookAuthors.Where(ba => ba.BookId == bookId).ToList();
            _context.BookAuthors.RemoveRange(bookAuthors);
            await _context.SaveChangesAsync();
        }
        public void AddVariant() {

        }

        public IQueryable<VariantCopy> GetLocationById(int id)  {
            return _context.VariantCopies.Where(vl => vl.LocationId == id);
        }
        public void AddBookLocation(VariantCopy variantCopy) {
            _context.VariantCopies.Add(variantCopy);
            _context.SaveChanges();
        }
        public void DeleteBookLocation(VariantCopy variantCopy){
            _context.VariantCopies.Remove(variantCopy);
            _context.SaveChanges();
        }


        public async Task<string> GetSerialNo(int locationId, int categoryId) {            
            var location = await _context.Locations.FirstOrDefaultAsync(l => l.Id == locationId);
            var query = from vl in _context.VariantCopies
            .Include(vl => vl.Variant)
            .ThenInclude(v => v.Book) select vl;
            var total = query.ToList();
            
            var no = await query
            .Where(vl => vl.LocationId == locationId && vl.Variant.Book.CategoryId == categoryId)
            .CountAsync();
            
            no = no + 1;
            var serialNo = no.ToString().PadLeft(4, '0');
            return $"{location.Code}{categoryId}{serialNo}";
        }

    }
}