using System.ComponentModel.DataAnnotations;

namespace Library.Models.PublisherViewModels
{
    public class CreateEditPublisherViewModel
    {
        public int? Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

    }
}