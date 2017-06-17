using System.Collections.Generic;
using System.Linq;
using Library.Core.Models;
using Library.Web.Models;
using Library.Web.Models.ClubViewModels;
using Library.Web.Models.TermSetViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Extensions
{
    public static class TermExtension {
        public static IQueryable<TermViewModel> MapToTermViewModel(this IQueryable<Term> query) {
            return query
            .Include(t => t.TermSet)
            .Select(
                term => new TermViewModel {
                    Id = term.Id,
                    Name = term.Name,
                    TermSetId = term.TermSetId,
                    TermSet = term.TermSet
                }
            );            
        }
        public static TermViewModel MapToTermViewModel(this Term term) {
            return new TermViewModel {
                Id = term.Id,
                Name = term.Name,
                TermSetId = term.TermSetId,
                TermSet = term.TermSet
            };
        }
        public static IList<SelectListItem> MapToSelectList(this IQueryable<Term> terms, int order = - 1) {
            terms = order != -1 ? ( order == 0 ? terms.OrderBy(t => t.Id) : (order == 1 ? terms.OrderByDescending(t => t.Id) : terms)) : terms;            
            return terms.Select(t=> new SelectListItem { Value = t.Id.ToString(), Text = t.Name}).ToList();
        }
         public static IList<SelectListItem> MapToSelectList(this IEnumerable<Term> terms) {            
            return terms.Select(t=> new SelectListItem { Value = t.Id.ToString(), Text = t.Name}).ToList();
        }
        
    }
}