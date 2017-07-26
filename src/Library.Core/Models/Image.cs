using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public class Image: BaseEntity {
        public string Path { get; set; }        
        public string ContentType { get; set; }
        public string Extension { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string Base64 { get; set; }
    }

}