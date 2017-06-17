using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;
using Library.Web.Models.BookViewModels;

namespace Library.Web.Models.PublisherViewModels {
    public class PublisherViewModel { 
        public int Id { get; set; }
        [Required]
        [StringLength(200)]        
        public string Name { get; set; }
        public IList<Book> Books { get; set; }
    }
}