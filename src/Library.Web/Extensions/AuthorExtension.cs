using System.Linq;
using System.Collections.Generic;
using Library.Web.Models;
using Library.Web.Models.AuthorViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Core.Models;

namespace Library.Web.Extensions
{
    public static class AuthorExtension
    {
        public static IQueryable<AuthorViewModel> MapToAuthorViewModel(this IQueryable<Author> authors) {
            return authors
            .Include(a => a.BooksLink)
            .ThenInclude(b => b.Book)
            .Select(author => new AuthorViewModel { 
                    Id = author.Id,
                    FirstName = author.FirstName, 
                    LastName = author.LastName,                     
                    Email = author.Email,
                    PhoneNumber = author.PhoneNumber,
                    Books = author.BooksLink.Select(bl => bl.Book).ToList()
                }
            );
        }
        public static IQueryable<AuthorViewModel> MapToAuthorViewModel(this IEnumerable<Author> authors) {
            return authors.AsQueryable()
            .Include(a => a.BooksLink)
            .ThenInclude(b => b.Book)
            .Select(author => new AuthorViewModel { 
                    Id = author.Id,
                    FirstName = author.FirstName, 
                    LastName = author.LastName,                     
                    Email = author.Email,
                    PhoneNumber = author.PhoneNumber
                    // Books = author.BooksLink.Select(bl => bl.Book).ToList()
                }
            );
        }
        public static Author MapToAuthor(this CreateEditAuthorViewModel model) {
            var author = new Author();
            author.LastName = model.LastName;
            author.FirstName = model.FirstName;
            author.PhoneNumber = model.PhoneNumber;
            author.Email = model.Email;
            if(model.Id != null){
                author.Id = (int)model.Id;
            }
            return author;
        }
        public static CreateEditAuthorViewModel MapToCreateEditAuthorViewModel(this IQueryable<Author> author) {
            return author.Select (a => new CreateEditAuthorViewModel {
                Id = a.Id,
                LastName = a.LastName,
                FirstName = a.FirstName,
                PhoneNumber = a.PhoneNumber,
                Email = a.Email,
            }).FirstOrDefault();
        }
        public static IList<SelectListItem> MapToSelectList(this IList<AuthorViewModel> authors) {            
            return authors.OrderBy(a => a.Id).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = $"{a.LastName} {a.FirstName}" }).ToList();
        }
    }
}