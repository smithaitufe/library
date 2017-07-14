using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Web.Models.BookViewModels
{
    public class BookLocationListingViewModel
    {
        public Variant Variant { get; set; }
        public IList<VariantCopy> Copies { get; set; } = new List<VariantCopy>();
        
    }
}