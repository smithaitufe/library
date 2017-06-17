using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models.BookViewModels
{
    public class BookVolumeViewModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Name { get; set; }
        
    }
    public class CreateBookVolumeViewModel {
        // [Required(ErrorMessage="Specify the book for this volume")]
        public int BookId { get; set; }
        [Display(Name="Volume")]
        [Required(ErrorMessage="Volume is required")]
        public string Name { get; set; }
    }
}