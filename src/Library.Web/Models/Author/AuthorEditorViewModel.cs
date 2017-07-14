using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models.AuthorViewModels
{
    public class AuthorEditorViewModel
    {

        public int? Id { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(15)]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}