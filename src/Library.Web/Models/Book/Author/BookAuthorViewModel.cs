using Library.Core.Models;

namespace Library.Models.BookViewModels
{
    public class BookAuthorViewModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public Book Book { get; set; }
        public Author Author { get; set; }
    }
}