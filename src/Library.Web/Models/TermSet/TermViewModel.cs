using Library.Core.Models;

namespace Library.Web.Models.TermSetViewModels
{
    public class TermViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TermSetId { get; set; }
        public TermSet TermSet { get; set;}
    }
}