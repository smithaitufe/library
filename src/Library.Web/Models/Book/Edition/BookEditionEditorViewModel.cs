using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;
using Library.Models.BootstrapModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Models.BookViewModels
{
    public class BookEditionEditorViewModel
    {
        public int? Id { get; set; }

        public Variant Variant { get; set; }

        [Required]
        public int VariantId { get; set; }
        
        [Display(Name="Book Edition")]
        [Required(ErrorMessage ="{0} is required")]
        public string Name { get; set; }

        public EditorAttributes EditorAttributes { get; set; }
    }
}