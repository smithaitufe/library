using System.Collections.Generic;

namespace Library.Models.BookViewModels
{
    public class ManageBookTermViewModel
    { 
        public BookTermFormViewModel Form { get; set; }
        public ICollection<BookTermViewModel> Terms { get; set; }
    }
}