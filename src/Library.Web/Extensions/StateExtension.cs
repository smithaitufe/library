using System.Linq;
using Library.Core.Models;
using Library.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Extensions {
    public static class StateExtension {

        public static IQueryable<SelectListItem> MapToSelectList(this IQueryable<State> states) {
            return states.Select(s => new SelectListItem {
                Value = s.Id.ToString(),
                Text = s.Name                
            });
        }
    }
}