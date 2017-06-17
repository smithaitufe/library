using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.TermSetViewModels
{
    public class TermListingViewModel {
        [Display(Name="Categories")]
        public int TermSetId { get; set; }
        public ICollection<SelectListItem> TermSets { get; set; }
        public List<TermViewModel> Terms { get; set; } = new List<TermViewModel>();
    }
}