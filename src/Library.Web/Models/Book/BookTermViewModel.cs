namespace Library.Web.Models.BookViewModels
{
    public class BookTermViewModel
    { 
        public int Id { get; set; }
        public int VariantId { get; set; }
        public int TermId { get; set; }
        public string TermName { get; set; }
        public bool Selected { get; set; }
    }
}