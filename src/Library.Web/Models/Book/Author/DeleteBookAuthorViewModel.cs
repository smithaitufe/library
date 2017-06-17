using Library.Core.Models;

namespace Library.Models.BookViewModels
{
    public class DeleteBookAuthorViewModel
    {
        public int Id { get; set; }
        public Author Author { get; set; }
        public Book Book { get; set; }
    }
}