using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Models.BookViewModels
{
    public class BookBasicEditorViewModel
    {

        public int Id { get; set; }        
        [Required(ErrorMessage = "Title is required")]
        [StringLength(300, MinimumLength = 2)]
        public string Title { get; set; }
        [Display(Name = "Sub Title")]
        public string SubTitle { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Specify the ISBN of this book")]
        public string ISBN { get; set; }
        [Display(Name = "Genre")]
        [Required(ErrorMessage = "Select the genre")]
        public int GenreId { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Select a category for this book")]
        public int CategoryId { get; set; }
        [Display(Name = "Publisher")]
        [Required(ErrorMessage = "The publisher is required")]
        public int PublisherId { get; set; }
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
        public Image Cover { get; set; } = new Image();

        public int VariantId { get; set; }
        [Required(ErrorMessage = "Please specify the numbe of pages of this book")]
        public int Pages { get; set; }        
        // [Display(Name = "Book Format")]
        // public int FormatId { get; set; }
        // [Display(Name = "Sale Grant")]
        // [Required(ErrorMessage = "Choose sale grant for this book")]
        // public int GrantId { get; set; }
        // [Display(Name = "Year")]
        // [Required(ErrorMessage = "Select the year of publication")]
        // public int YearId { get; set; }        
        // [Display(Name = "Collection")]
        // [Required(ErrorMessage = "Select the collection modes for this book")]
        // public int CollectionModeId { get; set; }
        // [Display(Name = "Fine After Due Date")]
        // public int FineId { get; set; }
        // [Display(Name = "Days Allowed")]
        // [Required(ErrorMessage = "Indicate the number of days allowed")]
        // public int DaysAllowedId { get; set; }        
        public Variant Variant { get; set; }
        public ICollection<SelectListItem> Publishers { get; set; }
        public ICollection<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
        public ICollection<SelectListItem> Categories { get; set; }
        

    }
}