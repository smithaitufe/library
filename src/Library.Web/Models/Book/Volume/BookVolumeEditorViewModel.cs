using System.ComponentModel.DataAnnotations;
using Library.Core.Models;
using Library.Web.Models.BootstrapModels;

namespace Library.Web.Models.BookViewModels
{
    public class BookVolumeEditorViewModel
    {
        public int? Id { get; set; }
        [Display(Name="Book Volume")]
        [Required(ErrorMessage="{0} is required")]
        public string Name { get; set; }
        public int VariantId { get; set; }
        public Variant Variant { get; set; }

        public EditorAttributes EditorAttributes { get; set; }
    }
}