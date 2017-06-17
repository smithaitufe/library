using System.Collections.Generic;

namespace Library.Models.AuthorViewModels
{
    public class AuthorListingViewModel {
        public ICollection<AuthorViewModel> Authors { get; set; }
    }
}