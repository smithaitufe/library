using System.Linq;
using Library.Web.Models;
using Library.Web.Models.BookViewModels;
using Library.Web.Models.AuthorViewModels;
using Microsoft.EntityFrameworkCore;
using Library.Web.Code;
using System;
using Library.Core.Models;

namespace Library.Web.Extensions
{
    public static class BookExtension
    {
        public static Book MapToBook(this BookEditorViewModel model)
        {
            var book = new Book
            {
                Id = model.Id,
                Title = model.Title,
                SubTitle = model.SubTitle,                
                Description = model.Description,
                Series = model.Series,
                NoInSeries = model.NoInSeries,
                CategoryId = model.CategoryId,
                GenreId = model.GenreId,
                PublisherId = model.PublisherId
            };
            return book;
        }
        public static Variant MapToVariant(this BookEditorViewModel model)
        {
            var variant = new Variant
            {
                BookId = (int)model.Id,
                FormatId = model.FormatId,
                CollectionModeId = model.CollectionModeId,
                GrantId = model.GrantId,
                DaysAllowedId = model.DaysAllowedId,
                FineId = model.FineId,
                YearId = model.YearId,
                Pages = model.Pages,
                ISBN = model.ISBN,
                Edition = model.Edition,
                Volume = model.Volume,
                LanguageId = model.LanguageId,
            };
            return variant;
        }       
        
        public static VariantPrice MapToVariantPrice(this BookEditorViewModel model) => new VariantPrice { VariantId = (int)model.VariantId, PriceId = model.PriceId, ConditionId = model.ConditionId };
        public static VariantCopy MapToVariantCopy(this BookEditorViewModel model) => new VariantCopy { VariantId = (int)model.VariantId, LocationId = model.LocationId, AvailabilityId = model.AvailabilityId, SourceId = model.SourceId };
        public static IQueryable<VariantCopyViewModel> MapToVariantCopyViewModel(this IQueryable<VariantCopy> variantCopy) {
            var query = variantCopy.Select(vl => new VariantCopyViewModel {
                Id = vl.Id,
                VariantId = vl.VariantId,
                LocationId = vl.LocationId,
                SourceId = vl.SourceId,
                AvailabilityId = vl.AvailabilityId,
                Location = vl.Location,
                Source = vl.Source.MapToTermViewModel(),
                Availability = vl.Availability.MapToTermViewModel()
            });
            return query;
        }
        public static IQueryable<BookViewModel> MapToBookViewModel(this IQueryable<Book> books)
        {
            return books.Select(
                book => new BookViewModel
                {
                    Title = book.Title,
                    Description = book.Description,
                    GenreId = book.GenreId,
                    Genre = book.Genre,                    
                    CategoryId = book.CategoryId,
                    Authors = book.BookAuthors.Select(al => al.Author).ToList()
                }
            );
        }
        public static IQueryable<BookViewModel> MapToBookViewModel(this IQueryable<Variant> variants)
        {
            return variants.Select(v => new BookViewModel
                {
                    Id = v.Book.Id,
                    Title = v.Book.Title,
                    SubTitle = v.Book.SubTitle,                    
                    Description = v.Book.Description,
                    GenreId = v.Book.GenreId,
                    Genre = v.Book.Genre,
                    Publisher = v.Book.Publisher,
                    Authors = v.Book.BookAuthors.Select(al => al.Author).ToList(),
                    CategoryId = v.Book.CategoryId,
                    Category = v.Book.Category,
                    Cover = v.Book.Cover,
                    VariantId = v.Id,
                    ISBN = v.ISBN,
                    YearId = v.YearId,
                    Year = v.Year,
                    GrantId = v.GrantId,
                    Grant = v.Grant,
                    CollectionModeId = v.CollectionModeId,
                    CollectionMode = v.CollectionMode,
                    Prices = v.VariantPrices,
                    FineId = v.FineId,
                    Fine = v.Fine,
                    DaysAllowedId = v.DaysAllowedId,
                    DaysAllowed = v.DaysAllowed,
                    Format = v.Format.Name,
                    Volume = v.Volume,
                    Edition = v.Edition,
                    VariantCopies = v.VariantCopies.ToList(),
                    Sources = v.VariantCopies.Select(vl => vl.Source).ToList()
                });
        }
        public static IQueryable<BookPreviewViewModel> MapToBookPreviewViewModel(this IQueryable<Variant> variant)
        {
            var query = variant.Select(v => new BookPreviewViewModel
            {
                Title = v.Book.Title,
                SubTitle = v.Book.SubTitle,
                Description = v.Book.Description,
                Genre = v.Book.Genre,
                Authors = v.Book.BookAuthors.Select(al => al.Author).MapToAuthorViewModel().ToList(),
                Category = v.Book.Category,
                Cover = v.Book.Cover,
                Publisher = v.Book.Publisher,
                ISBN = v.ISBN,
                Year = v.Year,
                Fine = v.Fine,                
                DaysAllowed = v.DaysAllowed,
                Locations = v.VariantCopies.Select(vl => vl.Location).ToList(),
                Sources = v.VariantCopies.Select(vl => vl.Source).ToList(),                
            });

            return query;
        }
        public static IQueryable<CheckedBookViewModel> MapToCheckedBookViewModel(this IQueryable<CheckOut> checkOuts)
        {
            var checkOutQuery = checkOuts
            //    .Include(co => co.Patron)
            //    .Include(co => co.Variant)
            //    .ThenInclude(bv => bv.Book).ThenInclude(b => b.AuthorsLink).ThenInclude(al => al.Author)
            //    .Include(co => co.Variant).ThenInclude(bv => bv.Format)
            //    .Include(co => co.Variant).ThenInclude(bv => bv.Book).ThenInclude(bv => bv.Genre)
               .Select(
                   co => new CheckedBookViewModel
                   {
                       Id = co.Id,
                       VariantId = co.VariantCopy.Variant.Id,
                       VariantCopyId = co.VariantCopyId,
                       Title = co.VariantCopy.Variant.Book.Title,
                       Genre = co.VariantCopy.Variant.Book.Genre,
                       ISBN = co.VariantCopy.Variant.ISBN,
                       Format = co.VariantCopy.Variant.Format.Name,
                       Authors = co.VariantCopy.Variant.Book.BookAuthors.Select(al => al.Author).ToList(),
                       Patron = co.Patron
                   }
               );
            return checkOutQuery;
        }
        public static BookEditorViewModel MapToBookEditorViewModel(this Variant model)
        {
            var result = new BookEditorViewModel
            {
                Id = model.Book.Id,
                Title = model.Book.Title,
                SubTitle = model.Book.SubTitle,
                Description = model.Book.Description,
                GenreId = model.Book.GenreId,
                Series = model.Book.Series,
                NoInSeries = model.Book.NoInSeries,
                CategoryId = model.Book.CategoryId,                
                PublisherId = model.Book.PublisherId,

                VariantId = model.Id,
                Pages = model.Pages,
                FormatId = model.FormatId,
                ISBN = model.ISBN,                
                CollectionModeId = model.CollectionModeId,                
                GrantId = model.GrantId,
                DaysAllowedId = model.DaysAllowedId,
                FineId = model.FineId,                
                YearId = model.YearId                
            };
            return result;
        }

        private static BookViewModel BookViewModel(Variant v)
        {
            return new BookViewModel
            {
                Id = v.Book.Id,
                VariantId = v.Id,
                Title = v.Book.Title,
                SubTitle = v.Book.SubTitle,
                ISBN = v.ISBN,
                Description = v.Book.Description,
                GenreId = v.Book.GenreId,
                Genre = v.Book.Genre,
                Authors = v.Book.BookAuthors.Select(al => al.Author).ToList(),
                CategoryId = v.Book.CategoryId,
                Category = v.Book.Category,
                YearId = v.YearId,
                Year = v.Year,
                GrantId = v.GrantId,
                Grant = v.Grant,
                CollectionModeId = v.CollectionModeId,
                CollectionMode = v.CollectionMode,
                FineId = v.FineId,
                Fine = v.Fine,
                DaysAllowedId = v.DaysAllowedId,
                DaysAllowed = v.DaysAllowed,
                VariantCopies = v.VariantCopies.ToList(),
                Format = v.Format.Name,
                Volume = v.Volume,
                Edition = v.Edition
            };
        }

        public static IQueryable<BookViewModel> OrderBooksBy(this IQueryable<BookViewModel> books, OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return books.OrderByDescending(x => x.Id);
                // case OrderByOptions.ByVotes:              
                //     return books.OrderByDescending(x => x.ReviewsCount > 0 ? x.ReviewsAverageVotes : 0);
                // case OrderByOptions.ByPublicationDate:    
                //     return books.OrderByDescending(x => x.PublishedOn);     
                // case OrderByOptions.ByPriceLowestFirst:   
                //     return books.OrderBy(x => x.Price);
                // case OrderByOptions.ByPriceHigestFirst:   
                //     return books.OrderByDescending(x => x.ActualPrice);              
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
        public static IQueryable<BookViewModel> FilterBooksBy(this IQueryable<BookViewModel> books, BooksFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
            {
                return books;
            }
            switch (filterBy)
            {
                case BooksFilterBy.NoFilter:
                    return books;
                case BooksFilterBy.ISBN:
                    return books.Where(book => book.ISBN.Equals(filterValue));
                case BooksFilterBy.Price:
                    return books.Where(book => book.Price.Name.Equals(filterValue));
                case BooksFilterBy.YearPublished:
                    return books.Where(book => book.PublishedOn.Year == int.Parse(filterValue) && book.PublishedOn <= System.DateTime.UtcNow);
                case BooksFilterBy.Author:
                    var names = (string[])filterValue.Split(new[] { ", " }, StringSplitOptions.None);
                    foreach (var name in names)
                    {
                        books = books.Where(book => book.Authors.Where(a => a.FirstName.Contains(name) && a.LastName.Contains(name)).Any());
                    }
                    return books;
                case BooksFilterBy.Genre:
                    return books.Where(book => book.GenreId.ToString().Equals(filterValue));
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }

        public static IQueryable<BookCirculationViewModel> MapToBookCirculationViewModel(this IQueryable<CheckOut> checkouts) {            
            var mapping = checkouts.Select(co => new BookCirculationViewModel {
                CheckOutId = co.Id,              
                RequestedDays = co.RequestedDays.Name,
                ApprovedDays = co.ApprovedDays.Name,
                BookTitle = co.VariantCopy.Variant.Book.Title,
                BookISBN = co.VariantCopy.Variant.ISBN,
                BookFormat = co.VariantCopy.Variant.Format.Name,
                PatronName = $"{co.Patron.LastName}, {co.Patron.FirstName}",
                PatronUserName = co.Patron.UserName,
                RequestDate = co.CheckOutStates.Where(cos => cos.Status.Name.ToLower().Equals("borrow initiated")).Select(cos => cos.InsertedAt).Single(),
                RequestApprovedDate = co.CheckOutStates.Where(cos => cos.Status.Name.ToLower().Equals("approved")).Select(cos => cos.InsertedAt).SingleOrDefault(),
                ApprovedDaysId = co.ApprovedDaysId,
                CheckOutStates = co.CheckOutStates
            });
            return mapping;
        }
    }
}