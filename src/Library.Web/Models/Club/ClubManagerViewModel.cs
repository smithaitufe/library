using System.Collections.Generic;

namespace Library.Models.ClubViewModels
{
    public class ClubManagerViewModel
    {
        public int UserId { get; set; }
        public IEnumerable<ClubViewModel> Clubs { get; set; }
    }
}