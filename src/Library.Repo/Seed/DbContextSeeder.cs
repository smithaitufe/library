using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Repo
{
    public static class DbContextSeeder
    {
        static LibraryDbContext _context;

        public static async Task PerformSeeding(LibraryDbContext context)
        {
            _context = context;
            if (!_context.TermSets.Any())
            {
                var termSets = new TermSet[]
                {
                    new TermSet { Name = "book-format"},
                    new TermSet { Name = "book-location"},
                    new TermSet { Name = "book-availability"},
                    new TermSet { Name = "genre"},
                    new TermSet { Name = "book-price"},
                    new TermSet { Name = "book-days-allowed"},
                    new TermSet { Name = "book-collection-mode"},
                    new TermSet { Name = "book-sale-grant"},
                    new TermSet { Name = "book-category"},
                    new TermSet { Name = "book-language"},
                    new TermSet { Name = "book-source"},
                    new TermSet { Name = "book-condition"},
                    new TermSet { Name = "book-fine"},
                    new TermSet { Name = "book-year"},
                    new TermSet { Name = "post-category"},
                    new TermSet { Name = "announcement-category"},
                    new TermSet { Name = "checkout-status"},
                    new TermSet { Name = "book-reservation-reason"}
                };
                termSets.ToList().ForEach(ts => _context.TermSets.Add(ts));
                _context.SaveChanges();
            }

            var bookFormat = await _context.TermSets.Where(ts => ts.Name.Equals("book-format")).FirstOrDefaultAsync();
            var terms = new Term[]{
                    new Term{ Name="EBook", TermSetId = bookFormat.Id},
                    new Term{ Name="Hard Cover", TermSetId = bookFormat.Id}
                };
            await SaveTerms(terms);

            var termSet = await _context.TermSets.Where(ts => ts.Name.Equals("book-reservation-reason")).FirstOrDefaultAsync();
            terms = new Term[]
            {
                    new Term{ Name="Membership", TermSetId = termSet.Id},
                    new Term{ Name="Active Subscription", TermSetId = termSet.Id},
                    new Term{ Name="No Pending Borrowed Books", TermSetId = termSet.Id},
                    new Term{ Name="Returns Regular", TermSetId = termSet.Id},
                    new Term{ Name="Returns", TermSetId = termSet.Id},
            };
            await SaveTerms(terms);

            var result = await GetTerms("book-reservation-reason");

            var grant = await _context.TermSets.Where(ts => ts.Name.Equals("book-sale-grant")).FirstOrDefaultAsync();
            terms = new Term[]{
                    new Term{ Name= "Free", TermSetId = grant.Id},
                    new Term{ Name= "Borrow Only", TermSetId = grant.Id },
                    new Term { Name = "For Sale", TermSetId = grant.Id }
                };
            await SaveTerms(terms);

            var announcementCategory = await _context.TermSets.Where(ts => ts.Name.Equals("announcement-category")).FirstOrDefaultAsync();
            terms = new[]{
                    new Term{ Name= "Member", TermSetId = announcementCategory.Id},
                    new Term{ Name= "Staff", TermSetId = announcementCategory.Id },
                    new Term { Name = "General", TermSetId = announcementCategory.Id },
                    new Term { Name = "Public", TermSetId = announcementCategory.Id },
                };
            await SaveTerms(terms);
            var bookSource = await _context.TermSets.Where(ts => ts.Name.Equals("book-source")).FirstOrDefaultAsync();
            terms = new Term[]{
                    new Term{ Name="State Donated", TermSetId = bookSource.Id},
                    new Term{ Name="Federal Donated", TermSetId = bookSource.Id},
                    new Term{ Name="NGO Donated", TermSetId = bookSource.Id},
                    new Term{ Name="Individual Donated", TermSetId = bookSource.Id},
                    new Term{ Name="State Purchased", TermSetId = bookSource.Id},
                    new Term{ Name="Project Donation", TermSetId = bookSource.Id},
                    new Term{ Name="Group Donation", TermSetId = bookSource.Id},
                    new Term{ Name="Group Purchase", TermSetId = bookSource.Id},
                    new Term{ Name="Alumni Donation", TermSetId = bookSource.Id},
                    new Term{ Name="Alumni Purchase", TermSetId = bookSource.Id},
                    new Term{ Name="Project Gunteberg", TermSetId = bookSource.Id},
                    new Term{ Name="Europeana", TermSetId = bookSource.Id},
                    new Term{ Name="Digital Public Library of America", TermSetId = bookSource.Id},
                    new Term{ Name="Internet Archive", TermSetId = bookSource.Id},
                    new Term{ Name="Open Library", TermSetId = bookSource.Id},
                    new Term{ Name="Feedbooks", TermSetId = bookSource.Id},
                    new Term{ Name="Manybooks", TermSetId = bookSource.Id},
                    new Term{ Name="Daily Lit", TermSetId = bookSource.Id},
                    new Term{ Name="Open Culture", TermSetId = bookSource.Id}
                };
            await SaveTerms(terms);

            var availabilty = await _context.TermSets.Where(ts => ts.Name.Equals("book-availability")).FirstOrDefaultAsync();
            terms = new Term[] {
                    new Term { Name ="On Shelf", TermSetId = availabilty.Id },
                    new Term { Name ="Off Shelf", TermSetId = availabilty.Id }
                };
            await SaveTerms(terms);

            var collectionMode = await _context.TermSets.Where(ts => ts.Name.Equals("book-collection-mode")).FirstOrDefaultAsync();
            terms = new Term[] {
                    new Term { Name ="All Users", TermSetId = collectionMode.Id },
                    new Term { Name ="Paid Users", TermSetId = collectionMode.Id }
                };
            await SaveTerms(terms);

            var category = await _context.TermSets.Where(ts => ts.Name.Equals("book-category")).FirstOrDefaultAsync();
            terms = new Term[] {
                    new Term { Name ="Antiquarian science books", TermSetId = category.Id },
                    new Term { Name ="Antisemitica", TermSetId = category.Id },
                    new Term { Name ="Artists' books", TermSetId = category.Id },
                    new Term { Name ="Autograph book", TermSetId = category.Id },
                    new Term { Name ="Big Little Book series", TermSetId = category.Id },
                    new Term { Name ="Blook", TermSetId = category.Id },
                    new Term { Name ="Board book", TermSetId = category.Id },
                    new Term { Name ="Book size", TermSetId = category.Id },
                    new Term { Name ="Book-and-record set", TermSetId = category.Id },
                    new Term { Name ="Books for the Blind", TermSetId = category.Id },
                    new Term { Name ="Books of secrets", TermSetId = category.Id },
                    new Term { Name ="Calendar (archives)", TermSetId = category.Id },
                    new Term { Name ="Catalogue raisonné", TermSetId = category.Id },
                    new Term { Name ="Chansonnier", TermSetId = category.Id },
                    new Term { Name ="Chapter book", TermSetId = category.Id },
                    new Term { Name ="Choirbook", TermSetId = category.Id },
                    new Term { Name ="Codex", TermSetId = category.Id },
                    new Term { Name ="Coffee table book", TermSetId = category.Id },
                    new Term { Name ="Coloring book", TermSetId = category.Id },
                    new Term { Name ="Comedia suelta", TermSetId = category.Id },
                    new Term { Name ="Commonplace book", TermSetId = category.Id },
                    new Term { Name ="Condolence book", TermSetId = category.Id },
                    new Term { Name ="Confession album", TermSetId = category.Id },
                    new Term { Name ="Controversial literature", TermSetId = category.Id },
                    new Term { Name ="Dream diary", TermSetId = category.Id },
                    new Term { Name ="E-text", TermSetId = category.Id },
                    new Term { Name ="Edited volume", TermSetId = category.Id },
                    new Term { Name ="Emblem books‎", TermSetId = category.Id },
                    new Term { Name ="Exhibition catalogue", TermSetId = category.Id },
                    new Term { Name ="Factory service manual", TermSetId = category.Id },
                    new Term { Name ="Festival book", TermSetId = category.Id },
                    new Term { Name ="Festschrift", TermSetId = category.Id },
                    new Term { Name ="Flip Book", TermSetId = category.Id },
                    new Term { Name ="Graphic novels‎", TermSetId = category.Id },
                    new Term { Name ="Grimoires‎", TermSetId = category.Id },
                    new Term { Name ="Handscroll", TermSetId = category.Id },
                    new Term { Name ="Herbal", TermSetId = category.Id },
                    new Term { Name ="Illuminated manuscript", TermSetId = category.Id },
                    new Term { Name ="Incunable", TermSetId = category.Id },
                    new Term { Name ="Interactive children's book", TermSetId = category.Id },
                    new Term { Name ="Kashi-hon", TermSetId = category.Id },
                    new Term { Name ="Limited edition books", TermSetId = category.Id },
                    new Term { Name ="Literary annuals", TermSetId = category.Id },
                    new Term { Name ="Literary trilogies", TermSetId = category.Id },
                    new Term { Name ="Liturgical book", TermSetId = category.Id },
                    new Term { Name ="Mapback", TermSetId = category.Id },
                    new Term { Name ="Midlist", TermSetId = category.Id },
                    new Term { Name ="Miniature book", TermSetId = category.Id },
                    new Term { Name ="Miscellany", TermSetId = category.Id },
                    new Term { Name ="Monograph", TermSetId = category.Id },
                    new Term { Name ="Monographic series", TermSetId = category.Id },
                    new Term { Name ="Movable book", TermSetId = category.Id },
                    new Term { Name ="Movable Book Society", TermSetId = category.Id },
                    new Term { Name ="Networked book", TermSetId = category.Id },
                    new Term { Name ="Novelization", TermSetId = category.Id },
                    new Term { Name ="Orihon", TermSetId = category.Id },
                    new Term { Name ="Out-of-print book", TermSetId = category.Id },
                    new Term { Name ="Personalized book", TermSetId = category.Id },
                    new Term { Name ="Photo-book", TermSetId = category.Id },
                    new Term { Name ="Picture book", TermSetId = category.Id },
                    new Term { Name ="Poetry books", TermSetId = category.Id },
                    new Term { Name ="Pop-up book", TermSetId = category.Id },
                    new Term { Name ="Postmodern picture book", TermSetId = category.Id },
                    new Term { Name ="Prayer books", TermSetId = category.Id },
                    new Term { Name ="Punch out book", TermSetId = category.Id },
                    new Term { Name ="Radio audiobook", TermSetId = category.Id },
                    new Term { Name ="Remaindered book", TermSetId = category.Id },
                    new Term { Name ="Samut khoi", TermSetId = category.Id },
                    new Term { Name ="Scroll", TermSetId = category.Id },
                    new Term { Name ="Short story collections‎", TermSetId = category.Id },
                    new Term { Name ="Social Register", TermSetId = category.Id },
                    new Term { Name ="Stamp catalog", TermSetId = category.Id },
                    new Term { Name ="Sticker album", TermSetId = category.Id },
                    new Term { Name ="Stripped book", TermSetId = category.Id },
                    new Term { Name ="Table-book", TermSetId = category.Id },
                    new Term { Name ="Tankōbon", TermSetId = category.Id },
                    new Term { Name ="Textbook", TermSetId = category.Id },
                    new Term { Name ="Tête-bêche books", TermSetId = category.Id },
                    new Term { Name ="Three-volume novel", TermSetId = category.Id },
                    new Term { Name ="Tie-in", TermSetId = category.Id },
                    new Term { Name ="Tokyo Kodomo Club", TermSetId = category.Id },
                    new Term { Name ="Tract (literature)", TermSetId = category.Id },
                    new Term { Name ="Trade paperback", TermSetId = category.Id },
                    new Term { Name ="Volume (bibliography)", TermSetId = category.Id },
                    new Term { Name ="Wallbook", TermSetId = category.Id },
                    new Term { Name ="Webcomics in print", TermSetId = category.Id },
                    new Term{ Name = "Wimmelbilderbuch", TermSetId = category.Id},
                    new Term { Name ="Yearbook", TermSetId = category.Id }
                };
            await SaveTerms(terms);
            var bookPrice = await _context.TermSets.Where(ts => ts.Name.Equals("book-price")).FirstOrDefaultAsync();
            terms = new Term[] {
                    new Term { Name ="Free", TermSetId = bookPrice.Id },
                    new Term { Name ="Membership", TermSetId = bookPrice.Id },
                    new Term { Name ="100", TermSetId = bookPrice.Id },
                    new Term { Name ="200", TermSetId = bookPrice.Id },
                    new Term { Name ="300", TermSetId = bookPrice.Id },
                    new Term { Name ="400", TermSetId = bookPrice.Id },
                    new Term { Name ="500", TermSetId = bookPrice.Id },
                };
            await SaveTerms(terms);
            var postCategory = await _context.TermSets.Where(ts => ts.Name.Equals("post-category")).FirstOrDefaultAsync();
            terms = new Term[] {
                    new Term { Name ="Review", TermSetId = postCategory.Id },
                    new Term { Name ="Reading", TermSetId = postCategory.Id },
                    new Term { Name ="Request", TermSetId = postCategory.Id },
                    new Term { Name ="Returning", TermSetId = postCategory.Id },
                    new Term { Name ="Borrowing", TermSetId = postCategory.Id },
                    new Term { Name ="Suggestion", TermSetId = postCategory.Id },
                    new Term { Name ="Researching", TermSetId = postCategory.Id },
                    new Term { Name ="Recommendation", TermSetId = postCategory.Id }
                };
            await SaveTerms(terms);
            var language = await _context.TermSets.Where(ts => ts.Name.Equals("book-language")).FirstOrDefaultAsync();
            terms = new Term[] {
                    new Term { Name ="Arabic", TermSetId = language.Id },
                    new Term { Name ="Chinese", TermSetId = language.Id },
                    new Term { Name ="English", TermSetId = language.Id },
                    new Term { Name ="French", TermSetId = language.Id },
                    new Term { Name ="German", TermSetId = language.Id },
                    new Term { Name ="Hausa", TermSetId = language.Id },
                    new Term { Name ="Hebrew", TermSetId = language.Id },
                    new Term { Name ="Hindi", TermSetId = language.Id },
                    new Term { Name ="Igbo", TermSetId = language.Id },
                    new Term { Name ="Ika", TermSetId = language.Id },
                    new Term { Name ="Isoko", TermSetId = language.Id },
                    new Term { Name ="Kanuri", TermSetId = language.Id },
                    new Term { Name ="Latin", TermSetId = language.Id },
                    new Term { Name ="Polish", TermSetId = language.Id },
                    new Term { Name ="Portuguese", TermSetId = language.Id },
                    new Term { Name ="Russian", TermSetId = language.Id },
                    new Term { Name ="Spanish", TermSetId = language.Id },
                    new Term { Name ="Urhobo", TermSetId = language.Id },
                    new Term { Name ="Yoruba", TermSetId = language.Id },
                };
            await SaveTerms(terms);

            var condition = await _context.TermSets.Where(ts => ts.Name.Equals("book-condition")).FirstOrDefaultAsync();
            terms = new Term[] {
                    new Term { Name ="New", TermSetId = condition.Id },
                    new Term { Name ="Used", TermSetId = condition.Id },
                    new Term { Name ="New - Refixed", TermSetId = condition.Id },
                    new Term { Name ="Used - Refixed", TermSetId = condition.Id }
                };
            await SaveTerms(terms);

            var fine = await _context.TermSets.Where(ts => ts.Name.Equals("book-fine")).FirstOrDefaultAsync();
            terms = new Term[] {
                    new Term { Name ="10", TermSetId = fine.Id },
                    new Term { Name ="15", TermSetId = fine.Id },
                    new Term { Name ="20", TermSetId = fine.Id },
                    new Term { Name ="30", TermSetId = fine.Id }
                };
            await SaveTerms(terms);

            var tsDaysAllowed = await _context.TermSets.Where(ts => ts.Name.Equals("book-days-allowed")).FirstOrDefaultAsync();


            for (var i = 1; i <= 30; i++)
            {

                _context.Terms.Add(new Term { Name = i.ToString(), TermSetId = tsDaysAllowed.Id });
                _context.SaveChanges();
            }


            var year = await _context.TermSets.Where(ts => ts.Name.Equals("book-year")).FirstOrDefaultAsync();

            for (var i = 2001; i <= DateTime.UtcNow.Year; i++)
            {
                _context.Terms.Add(new Term { Name = i.ToString(), TermSetId = year.Id });
                _context.SaveChanges();
            }

            var genre = await _context.TermSets.Where(ts => ts.Name.Equals("genre")).FirstOrDefaultAsync();
            terms = new Term[] {
                        new Term{ Name="Science Fiction", TermSetId = genre.Id},
                        new Term{ Name="Satire", TermSetId = genre.Id},
                        new Term{ Name="Drama", TermSetId = genre.Id},
                        new Term{ Name="Action", TermSetId = genre.Id},
                        new Term{ Name = "Adventure", TermSetId = genre.Id},
                        new Term{ Name="Romance", TermSetId = genre.Id},
                        new Term{ Name="Mystery", TermSetId = genre.Id},
                        new Term{ Name="Horror", TermSetId = genre.Id},
                        new Term{ Name="Self Help", TermSetId = genre.Id},
                        new Term{ Name="Health", TermSetId = genre.Id},
                        new Term{ Name="Guide", TermSetId = genre.Id},
                        new Term{ Name="Travel", TermSetId = genre.Id},
                        new Term{ Name="Children", TermSetId = genre.Id},
                        new Term{ Name="Religion, Spirituality and New Age", TermSetId = genre.Id},
                        new Term{ Name="Science", TermSetId = genre.Id},
                        new Term{ Name="History", TermSetId = genre.Id},
                        new Term{ Name="Math", TermSetId = genre.Id},
                        new Term{ Name="Anthology", TermSetId = genre.Id},
                        new Term{ Name="Poetry", TermSetId = genre.Id},
                        new Term{ Name="Encyclopedias", TermSetId = genre.Id},
                        new Term{ Name="Dictionaries", TermSetId = genre.Id},
                        new Term{ Name="Comics", TermSetId = genre.Id},
                        new Term{ Name="Arts", TermSetId = genre.Id},
                        new Term{ Name="Cookbook", TermSetId = genre.Id},
                        new Term{ Name="Diaries", TermSetId = genre.Id},
                        new Term{ Name="Journal", TermSetId = genre.Id},
                        new Term{ Name="Prayer books", TermSetId = genre.Id},
                        new Term{ Name="Series", TermSetId = genre.Id},
                        new Term{ Name="Trilogy", TermSetId = genre.Id},
                        new Term{ Name="Biographies", TermSetId = genre.Id},
                        new Term{ Name="Autobiographies", TermSetId = genre.Id},
                        new Term{ Name="Fantasy", TermSetId = genre.Id},
                        new Term { Name = "Computer", TermSetId = genre.Id}
                    };

            await SaveTerms(terms);

            var users = _context.Users.ToList();

            if (!_context.CheckOutStatuses.Any())
            {
                var parentCheckOutStatuses = new CheckOutStatus[]
                {
                    new CheckOutStatus{ Name="Borrow Initiated", Out =  true},
                    new CheckOutStatus{ Name="Return Initiated"}
                };
                parentCheckOutStatuses.ToList().ForEach(cos => _context.CheckOutStatuses.Add(cos));
                _context.SaveChanges();

                if (!_context.CheckOutStatuses.Any())
                {
                    var borrowParent = _context.CheckOutStatuses.SingleOrDefault(cos => cos.Name.ToLower().Equals(parentCheckOutStatuses[0].Name.ToLower()));
                    var returnParent = _context.CheckOutStatuses.SingleOrDefault(cos => cos.Name.ToLower().Equals(parentCheckOutStatuses[1].Name.ToLower()));

                    var checkOutStatuses = new CheckOutStatus[] {
                        new CheckOutStatus{ Name="Approved", Out =  true, Parent = borrowParent},
                        new CheckOutStatus{ Name="Rejected", Parent = borrowParent},
                        new CheckOutStatus{ Name="Contact Librarian", Parent = borrowParent},
                        new CheckOutStatus{ Name="Return Confirmed", Parent = returnParent},
                        new CheckOutStatus{ Name="Return Rejected", Parent = returnParent},
                        new CheckOutStatus{ Name="Contact Librarian", Parent = returnParent},
                    };
                    checkOutStatuses.ToList().ForEach(cos => _context.CheckOutStatuses.Add(cos));
                    _context.SaveChanges();
                }
            }



            if (!_context.Locations.Any())
            {
                (new Location[] {
                    new Location { Name ="Agbor", Code = "AGB" },
                    new Location { Name ="Asaba", Code = "ASB" },
                    new Location { Name ="Oghara", Code = "OGH" },
                    new Location { Name ="Ughelli", Code = "UGH" },
                    new Location { Name ="Warri", Code = "WAR" }
                }).ToList().ForEach(l => _context.Locations.Add(l));
                await _context.SaveChangesAsync();
            }

            if (!_context.Clubs.Any())
            {
                var clubs = new Club[] {
                    new Club { Name = "Sisi" },
                    new Club { Name = "Venus" },
                    new Club { Name = "Maxi" },
                    new Club { Name = "Medit" },
                    new Club { Name = "Lupta" },
                    new Club { Name = "Duo" },
                    new Club { Name = "Octa" },
                    new Club { Name = "Trio" },
                    new Club { Name = "Troy" },
                    new Club { Name = "Spartan" },
                };
                clubs.ToList().ForEach(club => _context.Clubs.Add(club));
                _context.SaveChanges();
            }

            if (!_context.ClubGenres.Any())
            {
                var dicts = new Dictionary<string, string>();
                dicts.Add("Sisi", "Satire|Drama|Action|Adventure");
                dicts.Add("Venus", "Romance|Mystery|Horror");
                dicts.Add("Maxi", "Self Help|Health|Guide");
                dicts.Add("Medit", "Travel|Children|Religion, Spirituality and New Age");
                dicts.Add("Lupta", "Science|Science Fiction|History|Math");
                dicts.Add("Duo", "Anthology|Poetry|Encyclopedias");
                dicts.Add("Octa", "Dictionaries|Comics|Arts");
                foreach (KeyValuePair<string, string> dict in dicts)
                {
                    var club = dict.Key;
                    var genres = dict.Value.ToString().Split('|');
                    foreach (var g in genres)
                    {
                        _context.ClubGenres.Add(new ClubGenre { ClubId = _context.Clubs.FirstOrDefault(c => c.Name.Equals(club)).Id, GenreId = _context.Terms.Include(t => t.TermSet).FirstOrDefault(t => t.Name.Equals(g) && t.TermSet.Name.Equals("genre")).Id });
                    }
                }
                _context.SaveChanges();
            }
            if (!_context.Countries.Any())
            {
                (new List<Country>{
                    new Country { Name ="Ghana", TelephoneCode = "+233", Abbreviation = "GH"},
                    new Country { Name ="Nigeria", TelephoneCode = "+234", Abbreviation = "NIG"},

                }).ForEach(country => _context.Countries.Add(country));
                await _context.SaveChangesAsync();
            }
            if (!_context.States.Any())
            {
                (new List<State>
                    {
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Abia" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Adamawa" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Anambra" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Bauchi" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Benue" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Cross Rivers" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Delta" },
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Ebonyi" },
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Edo" },
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Ekiti" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Enugu" },
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Gombe" },
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Imo" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Jigawa" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Kogi" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Ogun" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Oyo" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Rivers" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "Plateau" },
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "FCT" }
                    }
                ).ForEach(s =>
                {
                    _context.States.Add(s);
                    _context.SaveChanges();
                });
            }

            
        }
        private static async Task SaveTerms(Term[] terms)
        {
            terms.ToList().ForEach((term) =>
            {
                var result = _context.Terms.Where(t => t.TermSetId == term.TermSetId && t.Name.ToLower().Equals(term.Name.ToLower())).FirstOrDefault();
                if (result == null) _context.Terms.Add(term);
            });
            await _context.SaveChangesAsync();
            
        }
        private static async Task<IEnumerable<Term>> GetTerms(string termSet)
        {
            return await _context.Terms.Include(t => t.TermSet).Where(t => t.TermSet.Name.Equals(termSet)).ToListAsync();
        }
        private static async Task<Term> GetTerm(string name, string termSet)
        {
            return await _context.Terms.Include(t => t.TermSet).Where(t => t.Name.Equals(name) && t.TermSet.Name.Equals(termSet)).FirstOrDefaultAsync();
        }

    }
}