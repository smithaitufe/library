using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.BookViewModels
{
    public class BookBasicEditorViewModel
    {

        public int? Id { get; set; }        
        [Required(ErrorMessage = "Title is required")]
        [StringLength(300, MinimumLength = 2)]
        public string Title { get; set; }
        [Display(Name = "Sub Title")]
        public string SubTitle { get; set; }
        public string Description { get; set; }
        [Display(Name = "Genre")]
        [Required(ErrorMessage = "Select the genre")]
        public int GenreId { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Select a category for this book")]
        public int CategoryId { get; set; }
        [Display(Name = "Publisher")]
        [Required(ErrorMessage = "The publisher is required")]
        public int PublisherId { get; set; }
        
        
        
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
                    return null;
                }
            }
        }        
        
        public Image Cover { get; set; } = new Image();
        public string PreviousCover { get; set; }
        public ICollection<SelectListItem> Publishers { get; set; }
        public ICollection<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
        public ICollection<SelectListItem> Categories { get; set; }
        

    }
}