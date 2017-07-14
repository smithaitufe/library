using System.ComponentModel.DataAnnotations;
using Library.Web.Models.AddressViewModels;

namespace Library.Web.Models.PublisherViewModels {
    public class PublisherEditorViewModel {    
        public int? Id { get; set; }     
        [Required]
        [StringLength(200)]        
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public AddressEditorViewModel Address { get; set; }

        public PublisherEditorViewModel()
        {
            Address = new AddressEditorViewModel();
        }
        
    }
}