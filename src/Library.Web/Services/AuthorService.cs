using System.Collections.Generic;
using System.Linq;
// using Library.Data;
using Library.Web.Extensions;
using Library.Web.Models.AuthorViewModels;
using Library.Web.Models.TermSetViewModels;
using Library.Repo;

namespace Library.Web.Services
{
    public class AuthorService {
        LibraryDbContext context;
        public AuthorService(LibraryDbContext context){
            this.context = context;
        }
        public IList<AuthorViewModel> GetAllAuthors() {
            var query = context.Authors
            .OrderBy(a => a.LastName)
            .MapToAuthorViewModel()
            .ToList();

            return query;
        }
        public AuthorViewModel GetAuthorById(int id) {
            var query = context.Authors
            .Where(author => author.Id == id)
            .MapToAuthorViewModel()
            .SingleOrDefault();

            return query;
        }
    }
}