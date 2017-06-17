using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;
using Library.Web.Models.ClubViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.PostViewModels
{
    public class PostMessageViewModel {
        [DefaultValue(0)]
        public int? Id { get; set; }
        public int AuthorId { get; set; }
        [Required]
        [Display(Name="Club")]
        public int ClubId { get; set; }        
        public ICollection<ClubViewModel> Clubs { get; set; } = new HashSet<ClubViewModel>();
        [Display(Name="Category")]
        public int CategoryId { get; set; }
        public ICollection<Term> Categories { get; set; } = new HashSet<Term>();        
        public string Text { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Title { get; set; }        
        
    }
}