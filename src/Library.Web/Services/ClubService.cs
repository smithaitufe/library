using System.Collections.Generic;
using System.Linq;
using Library.Core.Models;
using Library.Data;
using Library.Extensions;
using Library.Models;
using Library.Models.ClubViewModels;
using Library.Repo;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Services
{
    public class ClubService {
        LibraryDbContext context;
        public ClubService(LibraryDbContext context){
            this.context = context;
        }

        public IQueryable<Club> GetAllClubs() {
            return context.Clubs 
            .Include(c => c.GenresLink)
            .ThenInclude(c => c.Genre);             
        }
        public IQueryable<Club> GetMemberClub(int userId, int clubId) {
            var query = context.Members
            .Include(m => m.Club)
            .ThenInclude(c => c.GenresLink)
            .ThenInclude(cl => cl.Genre)
            .Where(m => m.UserId == userId && m.ClubId == clubId)
            .Select(m => m.Club);     
            return query;       
        }


        public IQueryable<Club> GetMemberClubs(int userId) {
            return context.Members
            .Include(m => m.Club)
            .ThenInclude(c => c.GenresLink)
            .ThenInclude(cl => cl.Genre)
            .Where(m => m.UserId == userId)
            .Select(m => m.Club);            
        }


    }
}