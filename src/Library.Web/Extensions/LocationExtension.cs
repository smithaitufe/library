using System.Collections.Generic;
using System.Linq;
using Library.Core.Models;
using Library.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Extensions
{
    public static class LocationExtension {
        public static IQueryable<SelectListItem> MapToSelectList(this IQueryable<Location> locations) {
            return locations.Select(t=> new SelectListItem { Value = t.Id.ToString(), Text = t.Name});
        }
        public static IEnumerable<SelectListItem> MapToSelectList(this IEnumerable<Location> locations) {
            return locations.Select(t=> new SelectListItem { Value = t.Id.ToString(), Text = t.Name});
        }
    }
}