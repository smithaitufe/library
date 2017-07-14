using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Code
{
    public class SearchBookOptions {
        public string Location { get; set; }
        public string Phrase { get; set; }
        public ICollection<SelectListItem> Locations { get; set; }
    }
}