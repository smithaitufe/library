using System.Collections.Generic;

namespace Library.Models.AccountViewModels {
    public class PatronProfileViewModel {
        public string Name { get; set; }
        public string LibraryNo { get; set; }
        public string PhoneNumber { get; set; }
        public IList<string> Roles { get; set; }
        public int NumberOfPosts { get; set; }
        public int NumberOfComments { get; set; }
        public Address Address { get; set; }
    }
}