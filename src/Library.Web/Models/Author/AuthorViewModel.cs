using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;

namespace Library.Models.AuthorViewModels
{
    public class AuthorViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        public string Initials { get { return $"{LastName}, {FirstName[0]}"; } }
        public string FullName { get { return $"{LastName}, {FirstName}"; } }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(15)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}