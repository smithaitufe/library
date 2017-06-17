using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Models.BookViewModels
{
    public class ReserveBookFormViewModel {
        [Display(Name="Library No")]
        [Required(ErrorMessage="{0} cannot be empty")]
        public string LibraryNo { get; set; }
        [Display(Name="Serial No")]
        [Required(ErrorMessage="{0} cannot be empty")]
        public string SerialNo { get; set; }
        [Display(Name="Location")]
        [Required(ErrorMessage="{0} was not selected")]
        public int LocationId { get; set; }
        [Display(Name="Number of Days")]
        [Required(ErrorMessage="{0} cannot be empty")]
        public int NumberOfDaysId { get; set; }
        [Display(Name="Reason")]
        [Required(ErrorMessage="{0} was not selected")]
        public int ReasonId { get; set; }
        

        public ICollection<SelectListItem> Reasons { get; set; }
        public ICollection<SelectListItem> Locations { get; set; }
        public ICollection<SelectListItem> Days { get; set; }
    }
}