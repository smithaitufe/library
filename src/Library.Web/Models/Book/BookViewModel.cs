using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;
using Library.Web.Models.AuthorViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.BookViewModels
{
    public class BookViewModel
    {       
        public int Id { get; set; }        
        public string Title { get; set; }
        [Display(Name="Sub Title")]
        public string SubTitle { get; set; }
        public string Description { get; set; }        
        public string ISBN { get; set; }
        public int GenreId { get; set; }
        [Display(Name="Category")]
        public int CategoryId { get; set; }
        public Image Cover { get; set; }
        public bool Checked { get; set; }
        public int VariantId { get; set; }
        public string Format { get;set; }
        [Display(Name="Published On")]
        public DateTime PublishedOn { get; set; }        
        public Term Genre { get;set;}
        public bool Returned { get; set; }
        [Display(Name="Condition")]
        public int ConditionId { get; set; }
        public int LanguageId { get; set; }
        [Display(Name="Price")]
        public int PriceId { get; set; }
        [Display(Name="Book Source")]
        public int SourceId { get; set; }
        [Display(Name="Volume")]
        public string Volume { get; set; }
        public string VolumesJoined { get; set; }
        [Display(Name="Edition")]
        public string Edition { get; set; }
        public string EditionsJoined { get; set; }
        [Display(Name="Availability")]
        public int AvailabilityId { get; set; }
        [Display(Name="Location")]
        public int LocationId { get; set; }
        [Display(Name="Sale Grant")]
        public int GrantId { get; set; }
        [Display(Name="Book Cover")]
        public int CoverId { get; set; }
        [Display(Name="Days Allowed")]
        public int DaysAllowedId { get; set; }
        [Display(Name="Collection Mode")]
        public int CollectionModeId { get; set; }
        [Display(Name="Fine")]
        public int FineId { get; set; }
        [Display(Name="Is Series")]
        public bool IsSeries { get; set; } = false;
        [Display(Name="No of Volume in Series")]
        public int NoInSeries { get; set; } = 0;
        public int Pages { get; set; }
        public int YearId { get; set; }
        public Term Year { get; set; }        
        public Term Category { get; set; }
        public Term DaysAllowed { get; set; }
        public Term Price { get; set; }
        public Term Location { get; set; }
        public Term Language { get; set; }
        public Term Source { get; set; }
        public Term CollectionMode { get; set; }
        public Term Availability { get; set; }
        public Term Fine { get; set; }
        public Term Grant { get; set; }
        public Term Condition { get; set; }
        public Image Image { get; set; }
        
        public ICollection<VariantPrice> Prices { get; set; }        
        public ICollection<VariantCopy> VariantCopies { get; set; }
        public ICollection<Term> Sources { get; set; }
        public ICollection<Publisher> Publishers { get; set; }
        public ICollection<Author> Authors { get; set; }        
        public Publisher Publisher { get; set; }

    }
}