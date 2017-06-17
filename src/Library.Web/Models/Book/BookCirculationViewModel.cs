using System;
using System.Collections.Generic;
using Library.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Models.BookViewModels
{
    public class BookCirculationViewModel {
        public int CheckOutId { get; set; }
        public int VariantId { get; set; }
        public int LocationId { get; set; }
        public string RequestedDays { get; set; }
        public string ApprovedDays { get; set; } = null;
        public int? ApprovedDaysId { get; set; }
        public int PreviousStatusId { get; set; }
        public int? StatusId { get; set; }
        public string BookTitle { get; set; }
        public string BookISBN { get; set; }
        public string BookFormat { get; set; }
        public string PatronName { get; set; }
        public string PatronUserName { get; set; }
        public DateTime RequestDate { get; set; }
        public Nullable<DateTime> RequestApprovedDate { get; set; }       
        public ICollection<CheckOutState> CheckOutStates { get; set; }
        public ICollection<CheckOutStatus> Statuses { get; set; } = new HashSet<CheckOutStatus>();
        public ICollection<SelectListItem> Days { get; set; } = new HashSet<SelectListItem>();
    }
}