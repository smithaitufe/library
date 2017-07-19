using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;

namespace Library.Web.Models.BookViewModels
{
    public class BookTypeLocationSearchViewModel
    {
        [Display(Name="Location")]
        public int? LocationId { get; set; }
        public IList<Location> Locations { get; set; } = new List<Location>();
    }
}