using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Library.Core.Models;

namespace Library.Models.BookViewModels
{
    public class BookAuthorEditorViewModel
    {

        public Book Book { get; set; }
        
        [Display(Name="Author(s)")]
        [Required(ErrorMessage ="One or more authors must be selected")]
        public string[] SelectedAuthorIds { get; set; }
        public ICollection<SelectListItem> Authors { get; set; } = new HashSet<SelectListItem>();

    }
}