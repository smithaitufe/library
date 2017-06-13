namespace Library.Core.Models
{
    public class BookImage: BaseEntity {
        public int BookId { get; set; }
        public int ImageId { get; set; }
        public bool Default { get; set; } = false;

        public Book Book { get; set; }
        public Image Image { get; set; }
    }

}