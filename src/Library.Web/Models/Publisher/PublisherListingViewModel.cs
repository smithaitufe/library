using System.Collections.Generic;

namespace Library.Web.Models.PublisherViewModels {
    public class PublisherListingViewModel 
    {
        public IList<PublisherViewModel> Publishers  { get; set; } = new List<PublisherViewModel>();
    }
}