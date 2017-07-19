using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;

namespace Library.Web.Models.BookViewModels
{
    public class CheckoutViewModel
    {
        public int Id { get; set; }
        public int VariantId { get; set; }
        public int? VariantCopyId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }        
        public string ISBN { get; set; }
        public string Format { get;set; }
        public bool Checked { get; set; } = false;   
        public bool Active { get; set; }     
        [Display(Name="Price")]
        public decimal Price { get; set; } = 0.0M;
        [Display(Name="Published On")]
        public DateTime PublishedOn { get; set; }
        [Display(Name="Check-In Date")]
        public DateTime CheckInDate { get; set; }
        public Term Genre { get;set;}        
        public User Patron { get; set; }
        [Display(Name="Check-Out Date")]
        public Nullable<DateTime> CheckOutDate { get; set; }
        public Nullable<DateTime> ReturnedDate { get; set; }
        
        public ICollection<Publisher> Publishers { get; set; }
        public ICollection<Author> Authors { get; set; }
    }
    
}