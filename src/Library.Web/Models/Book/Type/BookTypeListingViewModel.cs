using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Web.Models.BookViewModels
{
    public class BookTypeListingViewModel
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public BookTypeLocationSearchViewModel BookTypeLocationSearch { get; set; }
        public ICollection<Variant> Variants { get; set; }
    }
}