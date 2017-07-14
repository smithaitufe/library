using System.Collections.Generic;

namespace Library.Web.Models.AuthorViewModels
{
    public class AuthorListingViewModel {
        public ICollection<AuthorViewModel> Authors { get; set; }
    }
}