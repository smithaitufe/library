namespace Library.Web.Models.BookViewModels
{
    public class BookTermFormViewModel
    { 
        public int Id { get; set; }
        public int VariantId { get; set; }
        public int TermSetId { get; set; }
        public int TermId { get; set; }
        public string TermName { get; set; }
    }
}