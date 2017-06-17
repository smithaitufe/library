using System.Linq;
using Library.Core.Models;
using Library.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Extensions
{
    public static class CheckOutExtension {
        
        public static IQueryable<SelectListItem> MapToSelectListItem(this IQueryable<CheckOutStatus> checkOutStatuses) {
            return checkOutStatuses.Select(t=> new SelectListItem { Value = t.Id.ToString(), Text = t.Name});
        }
    }
}