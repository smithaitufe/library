using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Web.Models.PublisherViewModels {
    public class PublisherListingViewModel 
    {
        public PublisherListingViewModel()
        {
            //Publishers = new List<PublisherViewModel>();
        }
        //public IList<PublisherViewModel> Publishers  { get; set; }

        public IList<Publisher> Publishers { get; set; }
    }
}