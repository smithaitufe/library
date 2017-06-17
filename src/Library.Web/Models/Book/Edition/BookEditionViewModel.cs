using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models.BookViewModels
{
    public class BookEditionViewModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Name { get; set; }
        
    }

}