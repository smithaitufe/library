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
                ISBN = model.ISBN,
                Description = model.Description,
                IsSeries = model.IsSeries,
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
                Pages = model.Pages
            };
            return variant;
        }
        public static Volume MapToVolume(this BookEditorViewModel model) => new Volume { VariantId = (int)model.VariantId, Name = model.Volume };
        public static Edition MapToEdition(this BookEditorViewModel model) => new Edition { VariantId = (int)model.VariantId, Name = model.Edition };
        public static VariantPrice MapToVariantPrice(this BookEditorViewModel model) => new VariantPrice { VariantId = (int)model.VariantId, PriceId = model.PriceId, ConditionId = model.ConditionId };
        public static VariantLanguage MapToVariantLanguage(this BookEditorViewModel model) => new VariantLanguage { VariantId = (int)model.VariantId, LanguageId = model.LanguageId };
        public static VariantLocation MapToVariantLocation(this BookEditorViewModel model) => new VariantLocation { VariantId = (int)model.VariantId, LocationId = model.LocationId, AvailabilityId = model.AvailabilityId, SourceId = model.SourceId };
        public static IQueryable<BookLocationViewModel> MapToVariantLocationViewModel(this IQueryable<VariantLocation> variantLocation) {
            var query = variantLocation.Select(vl => new BookLocationViewModel {
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
                    ISBN = book.ISBN,
                    Description = book.Description,
                    GenreId = book.GenreId,
                    Genre = book.Genre,
                    CategoryId = book.CategoryId,
                    Authors = book.AuthorsLink.Select(al => al.Author).ToList()
                }
            );
        }
        public static IQueryable<BookViewModel> MapToBookViewModel(this IQueryable<Variant> variants)
        {
            return variants.Select(v => new BookViewModel
                {
                    Id = v.Book.Id,
                    VariantId = v.Id,
                    Title = v.Book.Title,
                    SubTitle = v.Book.SubTitle,
                    ISBN = v.Book.ISBN,
                    Description = v.Book.Description,
                    GenreId = v.Book.GenreId,
                    Genre = v.Book.Genre,
                    Publisher = v.Book.Publisher,
                    Authors = v.Book.AuthorsLink.Select(al => al.Author).ToList(),
                    CategoryId = v.Book.CategoryId,
                    Category = v.Book.Category,
                    Cover = v.Book.Cover,
                    YearId = v.YearId,
                    Year = v.Year,
                    GrantId = v.GrantId,
                    Grant = v.Grant,
                    CollectionModeId = v.CollectionModeId,
                    CollectionMode = v.CollectionMode,
                    Prices = v.PricesLink,
                    FineId = v.FineId,
                    Fine = v.Fine,
                    DaysAllowedId = v.DaysAllowedId,
                    DaysAllowed = v.DaysAllowed,
                    Format = v.Format.Name,
                    Volumes = v.Volumes,
                    Editions = v.Editions,
                    Locations = v.VariantLocations.ToList(),
                    Sources = v.VariantLocations.Select(vl => vl.Source).ToList()
                });
        }
        public static IQueryable<BookPreviewViewModel> MapToBookPreviewViewModel(this IQueryable<Variant> variant)
        {
            var query = variant.Select(v => new BookPreviewViewModel
            {
                Title = v.Book.Title,
                SubTitle = v.Book.SubTitle,
                ISBN = v.Book.ISBN,
                Description = v.Book.Description,
                Genre = v.Book.Genre,
                Authors = v.Book.AuthorsLink.Select(al => al.Author).MapToAuthorViewModel().ToList(),
                Category = v.Book.Category,
                Cover = v.Book.Cover,
                Publisher = v.Book.Publisher,
                Year = v.Year,
                Fine = v.Fine,                
                DaysAllowed = v.DaysAllowed,
                Locations = v.VariantLocations.Select(vl => vl.Location).ToList(),
                Sources = v.VariantLocations.Select(vl => vl.Source).ToList(),                
            });

            return query;
        }
        public static IQueryable<CheckedBookViewModel> MapToCheckedBookViewModel(this IQueryable<CheckOut> checkOuts)
        {
            var checkOutQuery = checkOuts
               .Include(co => co.Patron)
               .Include(co => co.Variant)
               .ThenInclude(bv => bv.Book).ThenInclude(b => b.AuthorsLink).ThenInclude(al => al.Author)
               .Include(co => co.Variant).ThenInclude(bv => bv.Format)
               .Include(co => co.Variant).ThenInclude(bv => bv.Book).ThenInclude(bv => bv.Genre)
               .Select(
                   co => new CheckedBookViewModel
                   {
                       Id = co.Id,
                       BookVariantId = co.Variant.Id,
                       Title = co.Variant.Book.Title,
                       Genre = co.Variant.Book.Genre,
                       ISBN = co.Variant.Book.ISBN,
                       Format = co.Variant.Format.Name,
                       Authors = co.Variant.Book.AuthorsLink.Select(al => al.Author).ToList(),
                       Patron = co.Patron,
                       Returned = co.Returned,
                       ReturnedDate = co.ReturnedDate
                   }
               );
            return checkOutQuery;
        }
        public static BookEditorViewModel MapToBookEditorViewModel(this Variant model)
        {
            var result = new BookEditorViewModel
            {
                VariantId = model.Id,
                Pages = model.Pages,
                FormatId = model.FormatId,
                // SerialNo = model.SerialNo,
                Id = model.Book.Id,
                Title = model.Book.Title,
                SubTitle = model.Book.SubTitle,
                ISBN = model.Book.ISBN,
                Description = model.Book.Description,
                // LocationId = model.LocationId,
                CollectionModeId = model.CollectionModeId,
                IsSeries = model.Book.IsSeries,
                NoInSeries = model.Book.NoInSeries,
                // PriceId = model.PriceId,
                // ConditionId = model.ConditionId,
                // LanguageId = model.Book.LanguageId,
                // SourceId = model.SourceId,
                CategoryId = model.Book.CategoryId,
                // AvailabilityId = model.AvailabilityId,
                GrantId = model.GrantId,
                DaysAllowedId = model.DaysAllowedId,
                FineId = model.FineId,
                GenreId = model.Book.GenreId,
                YearId = model.YearId,
                PublisherId = model.Book.PublisherId
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
                ISBN = v.Book.ISBN,
                Description = v.Book.Description,
                GenreId = v.Book.GenreId,
                Genre = v.Book.Genre,
                Authors = v.Book.AuthorsLink.Select(al => al.Author).ToList(),
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
                Locations = v.VariantLocations.ToList(),
                Format = v.Format.Name,
                Volumes = v.Volumes,
                Editions = v.Editions
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
                VariantId = co.VariantId,
                LocationId = co.LocationId,
                RequestedDays = co.RequestedDays.Name,
                ApprovedDays = co.ApprovedDays.Name,
                BookTitle = co.Variant.Book.Title,
                BookISBN = co.Variant.Book.ISBN,
                BookFormat = co.Variant.Format.Name,
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