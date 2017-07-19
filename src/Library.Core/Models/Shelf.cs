using System.ComponentModel.DataAnnotations;

namespace Library.Core.Models
{
    public class Shelf: BaseEntity
    {
        [Required]
        public int LocationId { get; set; }
        public Location Location { get; set; }
        [Required(ErrorMessage ="{0} must be specified")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }
}