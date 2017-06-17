using System.Collections.Generic;

namespace Library.Models.PublisherViewModels {
    public class PublisherListingViewModel 
    {
        public IList<PublisherViewModel> Publishers  { get; set; } = new List<PublisherViewModel>();
    }
}