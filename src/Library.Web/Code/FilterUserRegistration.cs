using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Code
{
    public class FilterUserRegistration {
        [Display(Name="Start Date")]
        public Nullable<DateTime> StartDate { get; set; } = null;
        [Display(Name="End Date")]
        public Nullable<DateTime> EndDate { get; set; } = null;
    }
}