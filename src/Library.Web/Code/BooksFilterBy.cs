using System.ComponentModel.DataAnnotations;

namespace Library.Code
{
    public enum BooksFilterBy
    {
        [Display(Name="All")]
        NoFilter = 0,
        [Display(Name="Author")]
        Author,
        [Display(Name="Year Published")]
        YearPublished,
        [Display(Name="ISBN")]
        ISBN,
        [Display(Name="Language")]
        Language,
        [Display(Name="Book Format")]
        BookFormat,
        [Display(Name="Publisher")]
        Publisher,        
        [Display(Name="Price")]
        Price,
        [Display(Name="Genre")]
        Genre
    }
}