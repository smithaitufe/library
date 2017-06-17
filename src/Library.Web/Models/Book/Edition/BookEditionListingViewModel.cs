using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Web.Models.BookViewModels
{
    public class BookEditionListingViewModel
    {
        public Variant Variant { get; set; }
        public IList<Edition> Editions { get; set; } = new List<Edition>();
    }
}