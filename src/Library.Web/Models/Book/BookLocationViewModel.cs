using System.Collections.Generic;
using System.ComponentModel;
using Library.Core.Models;
using Library.Models.TermSetViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Models.BookViewModels
{
    public class BookLocationViewModel
    { 
        [DefaultValue(0)]
        public int Id { get; set; }
        public int VariantId { get; set; }
        public Variant Variant { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }

        public int SourceId { get; set; }
        public TermViewModel Source { get; set; }
        public ICollection<SelectListItem> Sources { get; set; }

        public int AvailabilityId { get; set; }
        public TermViewModel Availability { get; set; }
        public ICollection<SelectListItem> Availables { get; set; }

        //Inventory
        public int QuantityIn { get; set; }
        public int QuantityOut { get; set; }
        public int Quantity { get; set; }
    }
}