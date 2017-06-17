using System;
namespace Library.Models.BookViewModels
{
    public class BookCirculationSearchViewModel {
        
        public string Status { get; set; }
        public Nullable<DateTime> StartDate { get; set; } = null;
        public Nullable<DateTime> EndDate { get; set; } = null;
    }
}