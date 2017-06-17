using System.ComponentModel.DataAnnotations;

namespace Library.Models.TermSetViewModels
{
    public class TermFormViewModel {
        public int TermSetId { get; set; }
        public int TermId { get; set; }
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }
    }
}