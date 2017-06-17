using System.Collections.Generic;
using Library.Core.Models;
using Library.Web.Models.AuthorViewModels;

namespace Library.Web.Models.BookViewModels {
    public class BookPreviewViewModel {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public Term Year { get; set; }
        public Term Genre { get; set; }
        public Term Category { get; set; }
        public Image Cover { get; set; }
        public ICollection<AuthorViewModel> Authors { get; set; }
        public string AuthorsJoined { get; set; }
        public Publisher Publisher { get; set; }
        public Term Availability { get; set; }
        public Term Price { get; set; }
        public Term Fine { get; set; }
        public Term DaysAllowed { get; set; }
        public ICollection<Term> Languages { get; set; }
        public ICollection<Location> Locations { get; set; }
        public ICollection<Term> Sources { get; set; }
    }
}