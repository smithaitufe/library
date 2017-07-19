using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;

namespace Library.Web.Models.ShelfViewModels
{
    public class ShelfListingViewModel
    {
        public int LocationId { get; set; }
        public ICollection<Location> Locations { get; set; }
        public ICollection<Shelf> Shelves { get; set; }
    }
}