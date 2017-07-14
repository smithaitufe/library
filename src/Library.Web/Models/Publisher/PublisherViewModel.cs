using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;
using Library.Web.Models.BookViewModels;

namespace Library.Web.Models.PublisherViewModels {
    public class PublisherViewModel { 

        public PublisherViewModel()
        {
            Address = new Address();
        }
        [Display(Name="Publisher ID")]
        public int Id { get; set; }
        [Display(Name="Publisher")]
        [Required]
        [StringLength(200)]        
        public string Name { get; set; }
        [Display(Name="Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name="Address")]
        public Address Address { get; set; }
        public IList<Book> Books { get; set; }
    }
}