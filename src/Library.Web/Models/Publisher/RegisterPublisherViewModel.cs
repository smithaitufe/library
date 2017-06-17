using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models.PublisherViewModels {
    public class RegisterPublisherViewModel {    
        public int? Id { get; set; }     
        [Required]
        [StringLength(200)]        
        public string Name { get; set; }
        
    }
}