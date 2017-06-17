using System.ComponentModel.DataAnnotations;

namespace Library.Models.BookViewModels
{
    public class BookEditionViewModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Name { get; set; }
        
    }

}