using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Models.BookViewModels
{
    public class BookVolumeListingViewModel
    {
        public Variant Variant { get; set; }
        public IList<Volume> Volumes { get; set; } = new List<Volume>();
    }
}