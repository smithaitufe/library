using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Web.Models.LocationViewModels
{
    public class LocationListingViewModel
    {
        public IList<Location> Locations { get; set; } = new List<Location>();
    }
}