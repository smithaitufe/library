using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Models.ClubViewModels
{
    public class ClubViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Term> Genres { get; set; }     
        public string GenresNameJoined { get; set; }   
        
    }
}