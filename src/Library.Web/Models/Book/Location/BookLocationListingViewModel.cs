using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Models.BookViewModels
{
    public class BookLocationListingViewModel
    {
        public Variant Variant { get; set; }
        public IList<VariantLocation> Locations { get; set; } = new List<VariantLocation>();
        
    }
}