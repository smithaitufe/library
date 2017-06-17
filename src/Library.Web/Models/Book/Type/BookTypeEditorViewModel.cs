using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Models.BookViewModels
{
    public class BookTypeEditorViewModel
    {
        public int? Id { get; set; }
        public int BookId { get; set; }
        [Required(ErrorMessage = "Please specify the numbe of pages of this book")]
        public int Pages { get; set; }
        [Display(Name = "Book Format")]
        public int FormatId { get; set; }        
        [Display(Name = "Sale Grant")]
        [Required(ErrorMessage = "Choose sale grant for this book")]
        public int GrantId { get; set; }
        [Display(Name = "Year")]
        [Required(ErrorMessage = "Select the year of publication")]
        public int YearId { get; set; }
        [Display(Name = "Max. Days Allowed")]
        [Required(ErrorMessage = "Indicate the number of days")]
        public int DaysAllowedId { get; set; }
        [Display(Name = "Collection")]
        [Required(ErrorMessage = "Select the collection modes for this book")]
        public int CollectionModeId { get; set; }
        [Display(Name = "Fine After Due Date")]
        [Required]
        public int FineId { get; set; }



        public ICollection<SelectListItem> BookFormats { get; set; }
        public ICollection<SelectListItem> Days { get; set; }
        public ICollection<SelectListItem> CollectionModes { get; set; }
        public ICollection<SelectListItem> Grants { get; set; }
        public ICollection<SelectListItem> Fines { get; set; }        
        public ICollection<SelectListItem> Years { get; set; }
    }
}