using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Models.BookViewModels
{
    public class BookTypeListingViewModel
    {
        public Book Book { get; set; }
        public ICollection<Variant> Variants { get; set; }
    }
}