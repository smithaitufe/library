using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Web.Models.StateViewModels
{
    public class StateListingViewModel
    {
        public IList<State> States { get; set; } = new List<State>();
    }
}