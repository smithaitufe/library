namespace Library.Core.Models
{
    public class Image: BaseEntity {
        public string Path { get; set; }
        public string AbsolutePath { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
    }

}