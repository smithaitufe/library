using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.ClubViewModels
{
    public class JoinClubViewModel
    {
        [Display(Name="Club")]
        public int ClubId { get; set; }
        public int UserId { get; set; }
        public int[] SelectedClubs { get; set; }
        
        [Display(Name="Clubs")]
        public IList<ClubViewModel> Clubs { get; set; }    
        public JoinClubViewModel(){
            
        }          
        public JoinClubViewModel(int userId) {
            UserId = userId;
        }
    }
}