using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Web.Code;
using Library.Core.Models;
// using Library.Data;
using Library.Web.Extensions;
using Library.Web.Models.AuthorViewModels;
using Library.Web.Models.BookViewModels;
using Library.Repo;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Services
{
    public class BookService
    {
        LibraryDbContext context;
        public BookService(LibraryDbContext context) {
            this.context = context;            
        }

        private IQueryable<Book> Books => context.Books
            .Include(b=>b.Genre)
            .Include(b=>b.Category)
            .Include(b=>b.Publisher)
            .Include(b=>b.AuthorsLink)
            .ThenInclude(al=>al.Author)
            .Include(b=>b.Variants);

        private IQueryable<Variant> Variants => context.Variants
            .Include(v => v.Format)
            .Include(v => v.Year)
            .Include(v => v.CollectionMode)                
            .Include(v => v.Fine)
            .Include(v => v.DaysAllowed)
            .Include(v => v.VariantLocations)
            .ThenInclude( l =>l.Availability)
            .Include(v => v.VariantLocations)
            .ThenInclude(l => l.Source)
            .Include(v => v.VariantLocations)
            .ThenInclude(vl => vl.Location)
            .Include(v => v.Grant)
            .Include(v => v.Volumes)
            .Include(v => v.Editions)
            .Include(v => v.Book)
            .ThenInclude(b => b.Genre)
            .Include(v => v.Book)
            .ThenInclude(book => book.AuthorsLink)
            .ThenInclude(authorLink => authorLink.Author)
            .Include(v => v.PricesLink)
            .ThenInclude( pl => pl.Price)
            .Include(v => v.PricesLink)
            .ThenInclude( pl => pl.Condition)
            .Include(v => v.Languages);
        
        private IQueryable<CheckOut> CheckOuts => context.CheckOuts
                .Include(co => co.Patron)
                .Include(co => co.CheckOutStates)
                .ThenInclude(cos => cos.Status)
                .ThenInclude(s => s.Parent)
                .Include(co => co.CheckOutStates)
                .ThenInclude(cos => cos.ModifiedBy)
                .Include(co => co.CheckOutStates)
                .ThenInclude(cos => cos.CheckOut)
                .Include(co => co.Variant)
                .ThenInclude(v => v.Format)
                .Include(co => co.Variant)
                .ThenInclude(v => v.DaysAllowed)
                .Include(co => co.Variant)
                .ThenInclude(v => v.VariantLocations)
                .ThenInclude(l => l.Availability)
                .Include(co => co.Variant)
                .ThenInclude(v => v.VariantLocations)
                .ThenInclude(l => l.Source)
                .Include(co => co.Variant)
                .ThenInclude(v => v.VariantLocations)
                .ThenInclude(l => l.Location)
                .Include(co => co.Variant)
                .ThenInclude(v => v.Grant)
                .Include(co => co.Variant)
                .ThenInclude(v => v.Volumes)
                .Include(co => co.Variant)
                .ThenInclude(v => v.Editions)
                .Include(co => co.Variant)
                .ThenInclude(v => v.Book)
                .ThenInclude(b => b.Genre)
                .Include(co => co.Variant)
                .ThenInclude(v => v.Book)
                .ThenInclude(b => b.AuthorsLink)
                .ThenInclude(al => al.Author)
                .Include(co => co.Variant)
                .ThenInclude(v => v.Book)
                .ThenInclude(b => b.Publisher)
                .Include(co => co.Variant)
                .ThenInclude(v => v.PricesLink)
                .ThenInclude( pl => pl.Price)
                .Include(co => co.Variant)
                .ThenInclude(v => v.PricesLink)
                .ThenInclude( pl => pl.Condition)
                .Include(co => co.Variant)
                .ThenInclude(v => v.Languages);

        
        public IQueryable<Book> GetAllBooks()
        {
            var books =  from b in Books 
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
            return variants.Where(v => v.VariantLocations.Where( l => l.LocationId == locationId).Any());
        }
        public IQueryable<Variant> GetBooksByTitle(IQueryable<Variant> variants, string title) 
        {
            return variants.Where(v => v.Book.Title.ToLower().Contains(title.ToLower()));
        }
        public IQueryable<Variant> GetBooksByCategory(int categoryId) {
            return Variants.Where( v => v.Book.CategoryId == categoryId);
        }
        public IQueryable<BookViewModel> SearchBooks(string phrase) {
            var query = from v in context.Variants select v;
            if(!string.IsNullOrEmpty(phrase))
                query = query.Where(v => v.Book.Title.Contains(phrase));
            return query.MapToBookViewModel();
        }
        public IQueryable<BookViewModel> SortFilterPage(SortFilterPageOptions options) {
            var booksQuery = context.Variants
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
                query = query.Where(c => c.CheckOutStates.Where(cos => cos.Status.Name.ToLower().Equals(status.ToLower())).Any());
            }            
            return query;        
        }
        public IQueryable<CheckOut> GetCheckOutBooksForUser(int userId) 
        {
            var query = from x in CheckOuts select x;
            return query.AsNoTracking().Where(co => co.PatronId == userId);
        }
        public List<AuthorViewModel> GetBookAuthors(int id) 
        {
            return context.Variants
            .Include(bv => bv.Book).ThenInclude(b => b.AuthorsLink).ThenInclude(al => al.Author)
            .Where(bv => bv.Id == id).ToList() //Execute the sql query
            .SelectMany(bv => bv.Book.AuthorsLink.Select(al => al.Author).MapToAuthorViewModel())
            .ToList();              
        }
        public async void DeleteAuthorsByBookId(int bookId) 
        {
            var bookAuthors = context.BookAuthors.Where(ba => ba.BookId == bookId).ToList();
            context.BookAuthors.RemoveRange(bookAuthors);
            await context.SaveChangesAsync();
        }
        public void AddVariant() {

        }

        public IQueryable<VariantLocation> GetLocationById(int id)  {
            return context.VariantLocations.Where(vl => vl.LocationId == id);
        }
        public void AddBookLocation(VariantLocation variantLocation) {
            context.VariantLocations.Add(variantLocation);
            context.SaveChanges();
        }
        public void DeleteBookLocation(VariantLocation variantLocation){
            context.VariantLocations.Remove(variantLocation);
            context.SaveChanges();
        }


        public async Task<string> GetSerialNo(int locationId, int categoryId) {            
            var location = await context.Locations.FirstOrDefaultAsync(l => l.Id == locationId);
            var query = from vl in context.VariantLocations
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