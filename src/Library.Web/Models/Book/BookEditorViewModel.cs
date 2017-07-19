using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.BookViewModels
{
    public class BookEditorViewModel
    {
        [DefaultValue(0)]
        public int Id { get; set; }
        public int VariantId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(300, MinimumLength = 2)]
        public string Title { get; set; }
        [Display(Name = "Sub Title")]
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string SerialNo { get; set; }
        [Display(Name = "Book Format")]
        public int FormatId { get; set; }
        [Display(Name = "Language")]
        [Required(ErrorMessage = "Select a language for this book")]
        public int LanguageId { get; set; }
        [Display(Name = "Publisher")]
        [Required(ErrorMessage = "The publisher is required")]
        public int PublisherId { get; set; }
        [Display(Name = "Is series")]
        public bool Series { get; set; } = false;
        [Display(Name = "No of books in series")]
        public int? NoInSeries { get; set; } = 0;
        [Display(Name = "Year")]
        [Required(ErrorMessage = "Select the year of publication")]
        public int YearId { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Select a category for this book")]
        public int CategoryId { get; set; }
        [Display(Name = "Condition")]
        [Required(ErrorMessage = "Book condition must be specified")]
        public int ConditionId { get; set; }
        [Display(Name = "Price")]
        [Required(ErrorMessage = "Specify book price")]
        public int PriceId { get; set; }
        [Display(Name = "Book Source")]
        [Required(ErrorMessage = "Book source is required")]
        public int SourceId { get; set; }
        [Display(Name = "Availability")]
        [Required(ErrorMessage = "Select availability")]
        public int AvailabilityId { get; set; }
        [Display(Name = "Location")]
        [Required(ErrorMessage = "Select a location for this book")]
        public int LocationId { get; set; }
        [Display(Name = "Book Quantity")]
        [Required(ErrorMessage = "{0} cannot be empty")]
        [RegularExpression("^[1-9][0-9]+$", ErrorMessage = "Invalid characters were found in {0}")]
        public int? Quantity { get; set; }
        [Display(Name = "Sale Grant")]
        [Required(ErrorMessage = "Choose sale grant for this book")]
        public int GrantId { get; set; }
        [Display(Name = "Max. Days Allowed")]
        [Required(ErrorMessage = "Indicate the number of days")]
        public int DaysAllowedId { get; set; }
        [Display(Name = "Collection")]
        [Required(ErrorMessage = "Select the collection modes for this book")]
        public int CollectionModeId { get; set; }
        [Display(Name = "Fine After Due Date")]
        public int FineId { get; set; }
        [Required(ErrorMessage = "Specify the ISBN of this book")]
        public string ISBN { get; set; }
        [Display(Name = "Genre")]
        [Required(ErrorMessage = "Select the genre")]
        public int GenreId { get; set; }
        [Required(ErrorMessage = "An author must be selected")]
        [Display(Name = "Author(s)")]
        public string[] SelectedAuthorIds { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<SelectListItem> Authors { get; set; }
        public ICollection<SelectListItem> Publishers { get; set; }
        public ICollection<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
        public ICollection<SelectListItem> BookFormats { get; set; }
        public ICollection<SelectListItem> Days { get; set; }
        public ICollection<SelectListItem> CollectionModes { get; set; }
        public ICollection<SelectListItem> Conditions { get; set; }
        public ICollection<SelectListItem> BookSources { get; set; }
        public ICollection<SelectListItem> Locations { get; set; }
        public ICollection<SelectListItem> Availabilities { get; set; }
        public ICollection<SelectListItem> Grants { get; set; }
        public ICollection<SelectListItem> Fines { get; set; }
        public ICollection<SelectListItem> Languages { get; set; }
        public ICollection<SelectListItem> Prices { get; set; }
        public ICollection<SelectListItem> Categories { get; set; }
        public ICollection<SelectListItem> Years { get; set; }
        public ICollection<SelectListItem> Volumes { get; set; }
        public ICollection<SelectListItem> Editions { get; set; }
        [Required(ErrorMessage = "Please Upload an Image File")]
        [DataType(DataType.Upload)]
        [Display(Name = "Upload Image")]
        public IFormFile Image { get; set; }

        [FileExtensions(Extensions = "jpg,jpeg,png,gif,bmp,svg")]
        public string ImageExtension
        {
            get
            {
                if (Image != null)
                {
                    return Image.FileName;
                }
                else
                {
                    return "";
                }
            }
        }
        [Required(ErrorMessage = "Please specify the numbe of pages of this book")]
        public int Pages { get; set; }
        // [Required(ErrorMessage = "Book Edition is required")]
        [StringLength(20, ErrorMessage = "Maximum characters allowed for edition is ${0}")]
        public string Edition { get; set; }
        // [Required(ErrorMessage = "Book Volume is required")]
        [StringLength(20, ErrorMessage = "Maximum characters allowed for volume is ${0}")]
        public string Volume { get; set; }
        [Display(Name="Shelf No")]        
        public int? ShelfId { get; set; }

    }
}