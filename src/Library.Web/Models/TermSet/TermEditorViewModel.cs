using System.ComponentModel.DataAnnotations;
using Library.Core.Models;

namespace Library.Web.Models.TermSetViewModels
{
    public class TermEditorViewModel {
        public TermSet TermSet { get; set; }
        public int TermSetId { get; set; }
        public int TermId { get; set; }
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }
    }
}