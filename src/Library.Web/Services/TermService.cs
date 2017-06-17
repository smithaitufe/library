using System.Collections.Generic;
using System.Linq;
using Library.Core.Models;
// using Library.Data;
using Library.Web.Extensions;
using Library.Web.Models.TermSetViewModels;
using Library.Repo;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Services
{
    public class TermService {
        LibraryDbContext context;
        public TermService(LibraryDbContext context){
            this.context = context;
        }
        public IQueryable<Term> GetAllTerms() {
            return Terms();
        }
        public IQueryable<Term> GetTermById(int id) {
            return Terms().Where(t => t.Id ==id);
        }
        public IQueryable<Term> GetTermsBySet(string name) {
            return Terms().Where(t => t.TermSet.Name.Equals(name));
        }
        public IQueryable<Term> GetTermsBySet(int id) {
            return Terms().Where(t => t.TermSetId == id);
        }
        public void DeleteTerm(int id) {
            var term = context.Terms.SingleOrDefault(t => t.Id == id);
            context.Terms.Remove(term);
            context.SaveChanges();
        }

        private IQueryable<Term> Terms () {
            return context.Terms.Include(t => t.TermSet);
        }
    }
}