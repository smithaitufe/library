using System.Collections.Generic;

namespace Library.Models.StateViewModels
{
    public class StateListingViewModel
    {
        public IList<State> States { get; set; } = new List<State>();
    }
}