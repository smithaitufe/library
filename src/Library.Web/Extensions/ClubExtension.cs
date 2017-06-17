using System.Linq;
using Library.Core.Models;
using Library.Web.Models;
using Library.Web.Models.ClubViewModels;

namespace Library.Web.Extensions
{
    public static class ClubExtension {
        
        public static IQueryable<ClubViewModel> MapToClubViewModel(this IQueryable<Club> query) {
            var clubQuery = query.Select(
                club => new ClubViewModel {
                    Id = club.Id,
                    Name = club.Name,
                    Genres = club.GenresLink.Select(gl => gl.Genre).ToList(),
                    GenresNameJoined = string.Join(", ", club.GenresLink.Select(gl => gl.Genre.Name).ToList())
                }
            );

            return clubQuery;
        }
    }
}