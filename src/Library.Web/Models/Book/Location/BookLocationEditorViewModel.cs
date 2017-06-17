using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;
using Library.Models.BootstrapModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Models.BookViewModels
{
    public class BookLocationEditorViewModel { 
        public int? Id { get; set; }       
        
        public int VariantId { get; set; }
        public Variant Variant { get; set; }
        [Display(Name="Location")]
        [Required(ErrorMessage="{0} is required")]
        public int? LocationId { get; set; }
        public ICollection<SelectListItem> Locations { get; set; } = new HashSet<SelectListItem>();
        [Display(Name="Book Source")]
        [Required(ErrorMessage="{0} is required")]
        public int? SourceId { get; set; }
        public ICollection<SelectListItem> Sources { get; set; } = new HashSet<SelectListItem>();
        [Display(Name="Availability")]
        [Required(ErrorMessage="{0} is required")]
        public int? AvailabilityId { get; set; }
        public ICollection<SelectListItem> Availables { get; set; } = new HashSet<SelectListItem>();



        public EditorAttributes EditorAttributes { get; set; }

        
    }
}