using System;
using System.Collections.Generic;
using System.IO;
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
            await SaveTermsAsync(terms);

            var termSet = await _context.TermSets.Where(ts => ts.Name.Equals("book-reservation-reason")).FirstOrDefaultAsync();
            terms = new Term[]
            {
                    new Term{ Name="Membership", TermSetId = termSet.Id},
                    new Term{ Name="Active Subscription", TermSetId = termSet.Id},
                    new Term{ Name="No Pending Borrowed Books", TermSetId = termSet.Id},
                    new Term{ Name="Returns Regular", TermSetId = termSet.Id},
                    new Term{ Name="Returns", TermSetId = termSet.Id},
            };
            await SaveTermsAsync(terms);

            var result = await GetTerms("book-reservation-reason");

            var grant = await _context.TermSets.Where(ts => ts.Name.Equals("book-sale-grant")).FirstOrDefaultAsync();
            terms = new Term[]{
                    new Term{ Name= "Free", TermSetId = grant.Id},
                    new Term{ Name= "Borrow Only", TermSetId = grant.Id },
                    new Term { Name = "For Sale", TermSetId = grant.Id }
                };
            await SaveTermsAsync(terms);

            var announcementCategory = await _context.TermSets.Where(ts => ts.Name.Equals("announcement-category")).FirstOrDefaultAsync();
            terms = new[]{
                    new Term{ Name= "Member", TermSetId = announcementCategory.Id},
                    new Term{ Name= "Staff", TermSetId = announcementCategory.Id },
                    new Term { Name = "General", TermSetId = announcementCategory.Id },
                    new Term { Name = "Public", TermSetId = announcementCategory.Id },
                };
            await SaveTermsAsync(terms);
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
            await SaveTermsAsync(terms);

            var availabilty = await _context.TermSets.Where(ts => ts.Name.Equals("book-availability")).FirstOrDefaultAsync();
            terms = new Term[] {
                    new Term { Name ="On Shelf", TermSetId = availabilty.Id },
                    new Term { Name ="Off Shelf", TermSetId = availabilty.Id }
                };
            await SaveTermsAsync(terms);

            var collectionMode = await _context.TermSets.Where(ts => ts.Name.Equals("book-collection-mode")).FirstOrDefaultAsync();
            terms = new Term[] {
                    new Term { Name ="All Users", TermSetId = collectionMode.Id },
                    new Term { Name ="Paid Users", TermSetId = collectionMode.Id }
                };
            await SaveTermsAsync(terms);

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
            await SaveTermsAsync(terms);
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
            await SaveTermsAsync(terms);
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
            await SaveTermsAsync(terms);
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
            await SaveTermsAsync(terms);

            var condition = await _context.TermSets.Where(ts => ts.Name.Equals("book-condition")).FirstOrDefaultAsync();
            terms = new Term[] {
                    new Term { Name ="New", TermSetId = condition.Id },
                    new Term { Name ="Used", TermSetId = condition.Id },
                    new Term { Name ="New - Refixed", TermSetId = condition.Id },
                    new Term { Name ="Used - Refixed", TermSetId = condition.Id }
                };
            await SaveTermsAsync(terms);

            var fine = await _context.TermSets.Where(ts => ts.Name.Equals("book-fine")).FirstOrDefaultAsync();
            terms = new Term[] {
                    new Term { Name ="10", TermSetId = fine.Id },
                    new Term { Name ="15", TermSetId = fine.Id },
                    new Term { Name ="20", TermSetId = fine.Id },
                    new Term { Name ="30", TermSetId = fine.Id }
                };
            await SaveTermsAsync(terms);

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

            await SaveTermsAsync(terms);

            var users = _context.Users.ToList();

            if (!_context.CheckOutStatuses.Any())
            {
                var parentCheckOutStatuses = new CheckOutStatus[]
                {
                    new CheckOutStatus{ Name="Borrow Initiated"},
                    new CheckOutStatus{ Name="Return Initiated"}
                };
                parentCheckOutStatuses.ToList().ForEach(cos => _context.CheckOutStatuses.Add(cos));
                _context.SaveChanges();


                var borrowParent = _context.CheckOutStatuses.SingleOrDefault(cos => cos.Name.ToLower().Equals(parentCheckOutStatuses[0].Name.ToLower()));
                var returnParent = _context.CheckOutStatuses.SingleOrDefault(cos => cos.Name.ToLower().Equals(parentCheckOutStatuses[1].Name.ToLower()));

                var checkOutStatuses = new CheckOutStatus[] {
                    new CheckOutStatus{ Name="Borrow Approved", Parent = borrowParent},
                    new CheckOutStatus{ Name="Borrow Rejected", Parent = borrowParent},
                    new CheckOutStatus{ Name="Contact Librarian", Parent = borrowParent},
                    new CheckOutStatus{ Name="Return Confirmed", Parent = returnParent},
                    new CheckOutStatus{ Name="Return Rejected", Parent = returnParent},
                    new CheckOutStatus{ Name="Contact Librarian", Parent = returnParent},
                };
                checkOutStatuses.ToList().ForEach(cos => _context.CheckOutStatuses.Add(cos));
                _context.SaveChanges();
                
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
                    new Country { Name = "Afghanistan"},
                    new Country { Name = "Albania"},
                    new Country { Name = "Algeria"},
                    new Country { Name = "American Samoa"},
                    new Country { Name = "Andorra"},
                    new Country { Name = "Angola"},
                    new Country { Name = "Anguilla"},
                    new Country { Name = "Antigua & Barbuda"},
                    new Country { Name = "Argentina"},
                    new Country { Name = "Armenia"},
                    new Country { Name = "Aruba"},
                    new Country { Name = "Australia"},
                    new Country { Name = "Austria"},
                    new Country { Name = "Azerbaijan"},
                    new Country { Name = "Bahamas, The"},
                    new Country { Name = "Bahrain"},
                    new Country { Name = "Bangladesh"},
                    new Country { Name = "Barbados"},
                    new Country { Name = "Belarus"},
                    new Country { Name = "Belgium"},
                    new Country { Name = "Belize"},
                    new Country { Name = "Benin"},
                    new Country { Name = "Bermuda"},
                    new Country { Name = "Bhutan"},
                    new Country { Name = "Bolivia"},
                    new Country { Name = "Bosnia & Herzegovina"},
                    new Country { Name = "Botswana"},
                    new Country { Name = "Brazil"},
                    new Country { Name = "British Virgin Is."},
                    new Country { Name = "Brunei"},
                    new Country { Name = "Bulgaria"},
                    new Country { Name = "Burkina Faso"},
                    new Country { Name = "Burma"},
                    new Country { Name = "Burundi"},
                    new Country { Name = "Cambodia"},
                    new Country { Name = "Cameroon"},
                    new Country { Name = "Canada"},
                    new Country { Name = "Cape Verde"},
                    new Country { Name = "Cayman Islands"},
                    new Country { Name = "Central African Rep."},
                    new Country { Name = "Chad"},
                    new Country { Name = "Chile"},
                    new Country { Name = "China"},
                    new Country { Name = "Colombia"},
                    new Country { Name = "Comoros"},
                    new Country { Name = "Congo, Dem. Rep."},
                    new Country { Name = "Congo, Repub. of the"},
                    new Country { Name = "Cook Islands"},
                    new Country { Name = "Costa Rica"},
                    new Country { Name = "Cote d'Ivoire"},
                    new Country { Name = "Croatia"},
                    new Country { Name = "Cuba"},
                    new Country { Name = "Cyprus"},
                    new Country { Name = "Czech Republic"},
                    new Country { Name = "Denmark"},
                    new Country { Name = "Djibouti"},
                    new Country { Name = "Dominica"},
                    new Country { Name = "Dominican Republic"},
                    new Country { Name = "East Timor"},
                    new Country { Name = "Ecuador"},
                    new Country { Name = "Egypt"},
                    new Country { Name = "El Salvador"},
                    new Country { Name = "Equatorial Guinea"},
                    new Country { Name = "Eritrea"},
                    new Country { Name = "Estonia"},
                    new Country { Name = "Ethiopia"},
                    new Country { Name = "Faroe Islands"},
                    new Country { Name = "Fiji"},
                    new Country { Name = "Finland"},
                    new Country { Name = "France"},
                    new Country { Name = "French Guiana"},
                    new Country { Name = "French Polynesia"},
                    new Country { Name = "Gabon"},
                    new Country { Name = "Gambia, The"},
                    new Country { Name = "Gaza Strip"},
                    new Country { Name = "Georgia"},
                    new Country { Name = "Germany"},
                    new Country { Name = "Ghana"},
                    new Country { Name = "Gibraltar"},
                    new Country { Name = "Greece"},
                    new Country { Name = "Greenland"},
                    new Country { Name = "Grenada"},
                    new Country { Name = "Guadeloupe"},
                    new Country { Name = "Guam"},
                    new Country { Name = "Guatemala"},
                    new Country { Name = "Guernsey"},
                    new Country { Name = "Guinea"},
                    new Country { Name = "Guinea-Bissau"},
                    new Country { Name = "Guyana"},
                    new Country { Name = "Haiti"},
                    new Country { Name = "Honduras"},
                    new Country { Name = "Hong Kong"},
                    new Country { Name = "Hungary"},
                    new Country { Name = "Iceland"},
                    new Country { Name = "India"},
                    new Country { Name = "Indonesia"},
                    new Country { Name = "Iran"},
                    new Country { Name = "Iraq"},
                    new Country { Name = "Ireland"},
                    new Country { Name = "Isle of Man"},
                    new Country { Name = "Israel"},
                    new Country { Name = "Italy"},
                    new Country { Name = "Jamaica"},
                    new Country { Name = "Japan"},
                    new Country { Name = "Jersey"},
                    new Country { Name = "Jordan"},
                    new Country { Name = "Kazakhstan"},
                    new Country { Name = "Kenya"},
                    new Country { Name = "Kiribati"},
                    new Country { Name = "Korea, North"},
                    new Country { Name = "Korea, South"},
                    new Country { Name = "Kuwait"},
                    new Country { Name = "Kyrgyzstan"},
                    new Country { Name = "Laos"},
                    new Country { Name = "Latvia"},
                    new Country { Name = "Lebanon"},
                    new Country { Name = "Lesotho"},
                    new Country { Name = "Liberia"},
                    new Country { Name = "Libya"},
                    new Country { Name = "Liechtenstein"},
                    new Country { Name = "Lithuania"},
                    new Country { Name = "Luxembourg"},
                    new Country { Name = "Macau"},
                    new Country { Name = "Macedonia"},
                    new Country { Name = "Madagascar"},
                    new Country { Name = "Malawi"},
                    new Country { Name = "Malaysia"},
                    new Country { Name = "Maldives"},
                    new Country { Name = "Mali"},
                    new Country { Name = "Malta"},
                    new Country { Name = "Marshall Islands"},
                    new Country { Name = "Martinique"},
                    new Country { Name = "Mauritania"},
                    new Country { Name = "Mauritius"},
                    new Country { Name = "Mayotte"},
                    new Country { Name = "Mexico"},
                    new Country { Name = "Micronesia, Fed. St."},
                    new Country { Name = "Moldova"},
                    new Country { Name = "Monaco"},
                    new Country { Name = "Mongolia"},
                    new Country { Name = "Montserrat"},
                    new Country { Name = "Morocco"},
                    new Country { Name = "Mozambique"},
                    new Country { Name = "Namibia"},
                    new Country { Name = "Nauru"},
                    new Country { Name = "Nepal"},
                    new Country { Name = "Netherlands"},
                    new Country { Name = "Netherlands Antilles"},
                    new Country { Name = "New Caledonia"},
                    new Country { Name = "New Zealand"},
                    new Country { Name = "Nicaragua"},
                    new Country { Name = "Niger"},
                    new Country { Name = "Nigeria"},
                    new Country { Name = "N. Mariana Islands"},
                    new Country { Name = "Norway"},
                    new Country { Name = "Oman"},
                    new Country { Name = "Pakistan"},
                    new Country { Name = "Palau"},
                    new Country { Name = "Panama"},
                    new Country { Name = "Papua New Guinea"},
                    new Country { Name = "Paraguay"},
                    new Country { Name = "Peru"},
                    new Country { Name = "Philippines"},
                    new Country { Name = "Poland"},
                    new Country { Name = "Portugal"},
                    new Country { Name = "Puerto Rico"},
                    new Country { Name = "Qatar"},
                    new Country { Name = "Reunion"},
                    new Country { Name = "Romania"},
                    new Country { Name = "Russia"},
                    new Country { Name = "Rwanda"},
                    new Country { Name = "Saint Helena"},
                    new Country { Name = "Saint Kitts & Nevis"},
                    new Country { Name = "Saint Lucia"},
                    new Country { Name = "St Pierre & Miquelon"},
                    new Country { Name = "Saint Vincent and the Grenadines"},
                    new Country { Name = "Samoa"},
                    new Country { Name = "San Marino"},
                    new Country { Name = "Sao Tome & Principe"},
                    new Country { Name = "Saudi Arabia"},
                    new Country { Name = "Senegal"},
                    new Country { Name = "Serbia"},
                    new Country { Name = "Seychelles"},
                    new Country { Name = "Sierra Leone"},
                    new Country { Name = "Singapore"},
                    new Country { Name = "Slovakia"},
                    new Country { Name = "Slovenia"},
                    new Country { Name = "Solomon Islands"},
                    new Country { Name = "Somalia"},
                    new Country { Name = "South Africa"},
                    new Country { Name = "Spain"},
                    new Country { Name = "Sri Lanka"},
                    new Country { Name = "Sudan"},
                    new Country { Name = "Suriname"},
                    new Country { Name = "Swaziland"},
                    new Country { Name = "Sweden"},
                    new Country { Name = "Switzerland"},
                    new Country { Name = "Syria"},
                    new Country { Name = "Taiwan"},
                    new Country { Name = "Tajikistan"},
                    new Country { Name = "Tanzania"},
                    new Country { Name = "Thailand"},
                    new Country { Name = "Togo"},
                    new Country { Name = "Tonga"},
                    new Country { Name = "Trinidad & Tobago"},
                    new Country { Name = "Tunisia"},
                    new Country { Name = "Turkey"},
                    new Country { Name = "Turkmenistan"},
                    new Country { Name = "Turks & Caicos Is"},
                    new Country { Name = "Tuvalu"},
                    new Country { Name = "Uganda"},
                    new Country { Name = "Ukraine"},
                    new Country { Name = "United Arab Emirates"},
                    new Country { Name = "United Kingdom"},
                    new Country { Name = "United States"},
                    new Country { Name = "Uruguay"},
                    new Country { Name = "Uzbekistan"},
                    new Country { Name = "Vanuatu"},
                    new Country { Name = "Venezuela"},
                    new Country { Name = "Vietnam"},
                    new Country { Name = "Virgin Islands"},
                    new Country { Name = "Wallis and Futuna"},
                    new Country { Name = "West Bank"},
                    new Country { Name = "Western Sahara"},
                    new Country { Name = "Yemen"},
                    new Country { Name = "Zambia"},
                    new Country { Name = "Zimbabwe"}

                }).ForEach(country => _context.Countries.Add(country));
                await _context.SaveChangesAsync();
            }
            if (!_context.States.Any())
            {
                (new List<State>
                    {
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Abia" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Adamawa" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Anambra" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Bauchi" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Benue" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Cross Rivers" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Delta" },
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Ebonyi" },
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Edo" },
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Ekiti" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Enugu" },
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Gombe" },
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Imo" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Jigawa" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Kogi" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Ogun" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Oyo" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Rivers" } ,
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "Plateau" },
                        new State { CountryId = _context.Countries.SingleOrDefault(c => c.Name.Equals("Nigeria")).Id,  Name = "FCT" }
                    }
                ).ForEach(s =>
                {
                    _context.States.Add(s);
                    _context.SaveChanges();
                });               
                
            }
            if(!_context.Publishers.Any()){
                var publishers = new Publisher[]{
                    new Publisher(){ Name="Pearson Corporations", PhoneNumber = "+2348138238095"},
                    new Publisher(){ Name="McGraw Hills Publishers", PhoneNumber = "+1987456738"},
                    new Publisher(){ Name = "Gefen Publishing House"},
                    new Publisher(){ Name = "Greenleaf Publishing Ltd"}
                };
                publishers.ToList().ForEach( publisher => _context.Publishers.Add(publisher));                
                await _context.SaveChangesAsync();
            }
            if(!_context.Authors.Any())
            {

                var authors = new Author[]
                {
                    new Author { FirstName = "Patrick", LastName = "Harrison", Email = "patrickha@gmail.com"},
                    new Author { FirstName = "Wilson", LastName = "Ferdinard", Email = "wilsfed@yahoo.com"},
                };
                authors.ToList().ForEach( author => _context.Authors.Add(author));
                await _context.SaveChangesAsync();
                
            }
            await PerformBooksSeedAsync();
            

            
        }

        private static async Task PerformBooksSeedAsync()
        {
            var asabaLocationId = (await _context.Locations.Where(l=>l.Name.ToLower().Equals("asaba")).FirstOrDefaultAsync()).Id;
            var shelfId = 0;
            if(!_context.Shelves.Any())
            {
                (new List<Shelf> {
                    new Shelf { Name = "AX", LocationId = asabaLocationId },
                    new Shelf { Name = "BH", LocationId = asabaLocationId },
                    new Shelf { Name = "SC", LocationId = asabaLocationId },
                    new Shelf { Name = "HV", LocationId = asabaLocationId },
                }).ForEach(shelf => _context.Shelves.Add(shelf));
                await _context.SaveChangesAsync();    

                shelfId = (await _context.Shelves.FirstAsync()).Id;
            }

            if(!_context.Books.Any())
            {            
                var books = new Book[] {                      
                        new Book { 
                            Title = "Beginning ASP.NET 4.5 in C#", 
                            GenreId = GetTerm("Computer", "genre").Id, 
                            PublisherId = _context.Publishers.First().Id, 
                            CategoryId = GetTerm("Flip Book", "book-category").Id,
                            // Cover = GetBookCover("Beginning-ASP.NET-4.5-in-C#","/home/smith/Pictures/downloads/31263r.jpg"),
                            BookAuthors = new List<BookAuthor> {
                                new BookAuthor { Author = _context.Authors.FirstOrDefault() },
                                new BookAuthor { Author = _context.Authors.OrderByDescending(a=>a.Id).Take(1).FirstOrDefault() }
                            },
                            Variants = new List<Variant>{
                                new Variant {
                                    ISBN = "90149008993",
                                    Edition = "1st",
                                    LanguageId = GetTerm("English","book-language").Id,                             
                                    FormatId = GetTerm("Hard Cover","book-format").Id,
                                    Pages = 200,                            
                                    YearId = GetTerm("2009", "book-year").Id,                             
                                    GrantId = GetTerm("Free", "book-sale-grant").Id,
                                    CollectionModeId = GetTerm("All Users", "book-collection-mode").Id, 
                                    FineId = GetTerm("10", "book-fine").Id, 
                                    DaysAllowedId = GetTerm("10", "book-days-allowed").Id,
                                    VariantCopies = new List<VariantCopy>{
                                        new VariantCopy { 
                                            LocationId = asabaLocationId,
                                            SourceId = GetTerm("State Purchased", "book-source").Id,
                                            SerialNo = "UEAS3094D",
                                            AvailabilityId = GetTerm("On Shelf", "book-availability").Id,
                                            ShelfId = shelfId,
                                        },
                                        new VariantCopy { 
                                            LocationId = asabaLocationId,
                                            SourceId = GetTerm("State Purchased", "book-source").Id,
                                            SerialNo = "UEAS3024E",
                                            AvailabilityId = GetTerm("On Shelf", "book-availability").Id,
                                            ShelfId = shelfId,
                                        }
                                    },
                                    VariantPrices = new List<VariantPrice>{
                                        new VariantPrice { ConditionId = GetTerm("New", "book-condition").Id, PriceId = GetTerm("Membership","book-price").Id },
                                        new VariantPrice { ConditionId = GetTerm("New", "book-condition").Id, PriceId = GetTerm("Free","book-price").Id},
                                    }
                                },
                                new Variant {                            
                                    ISBN = "90149008921",
                                    Edition = "1st",
                                    LanguageId = GetTerm("English","book-language").Id,                             
                                    FormatId = GetTerm("EBook","book-format").Id,
                                    Pages = 180,                            
                                    YearId = GetTerm("2009", "book-year").Id,                             
                                    GrantId = GetTerm("Free", "book-sale-grant").Id,
                                    CollectionModeId = GetTerm("All Users", "book-collection-mode").Id, 
                                    FineId = GetTerm("10", "book-fine").Id, 
                                    DaysAllowedId = GetTerm("20", "book-days-allowed").Id,
                                    VariantCopies = new List<VariantCopy>{
                                        new VariantCopy { 
                                            LocationId = asabaLocationId,
                                            SourceId = GetTerm("State Purchased", "book-source").Id,
                                            SerialNo = "UEAS30911",
                                            AvailabilityId = GetTerm("Off Shelf", "book-availability").Id,
                                        },
                                        new VariantCopy { 
                                            LocationId = asabaLocationId,
                                            SourceId = GetTerm("State Purchased", "book-source").Id,
                                            SerialNo = "UEAS30372",
                                            AvailabilityId = GetTerm("Off Shelf", "book-availability").Id,
                                        }
                                    },
                                    VariantPrices = new List<VariantPrice>{
                                        new VariantPrice { ConditionId = GetTerm("New", "book-condition").Id, PriceId = GetTerm("Free","book-price").Id},
                                    }
                                }
                        
                            }
                        },
                        new Book { 
                            Title = "Beginning ASP.NET Core", 
                            GenreId = GetTerm("Computer", "genre").Id, 
                            PublisherId = _context.Publishers.OrderByDescending(p=>p.Id).Take(1).First().Id,
                            CategoryId = GetTerm("Flip Book", "book-category").Id,
                            // Cover = GetBookCover("Beginning-ASP.NET-Core","/home/smith/Pictures/downloads/Alan_Duff_-_Once_Were_Warriors.jpeg"),
                            BookAuthors = new List<BookAuthor> {
                                new BookAuthor { Author = _context.Authors.OrderByDescending(a=>a.Id).Take(1).FirstOrDefault() }
                            },
                            Variants = new List<Variant>() {
                                    new Variant {                            
                                        ISBN = "20128943834",
                                        Edition = "1st",
                                        LanguageId = GetTerm("English","book-language").Id,                             
                                        FormatId = GetTerm("Hard Cover","book-format").Id,
                                        Pages = 250,                            
                                        YearId = GetTerm("2012", "book-year").Id,                             
                                        GrantId = GetTerm("Free", "book-sale-grant").Id,
                                        CollectionModeId = GetTerm("All Users", "book-collection-mode").Id, 
                                        FineId = GetTerm("10", "book-fine").Id, 
                                        DaysAllowedId = GetTerm("20", "book-days-allowed").Id,
                                        VariantCopies = new List<VariantCopy>{
                                            new VariantCopy { 
                                                LocationId = asabaLocationId,
                                                SourceId = GetTerm("State Purchased", "book-source").Id,
                                                SerialNo = "2YHNC239295",
                                                AvailabilityId = GetTerm("On Shelf", "book-availability").Id,
                                                ShelfId = shelfId,
                                            },
                                            new VariantCopy { 
                                                LocationId = asabaLocationId,
                                                SourceId = GetTerm("State Purchased", "book-source").Id,
                                                SerialNo = "2WHNC239286",
                                                AvailabilityId = GetTerm("On Shelf", "book-availability").Id,
                                                ShelfId = shelfId,
                                            }
                                        },
                                        VariantPrices = new List<VariantPrice>{                                
                                            new VariantPrice { ConditionId = GetTerm("New", "book-condition").Id, PriceId = GetTerm("Membership","book-price").Id }
                                        }
                                    }
                            }                        
                        }
                        
                };             

                books.ToList().ForEach(b => _context.Books.Add(b)) ;
                await _context.SaveChangesAsync();
            }
            if(!_context.CheckOuts.Any())
            {       
                var patron = await _context.Users.FirstOrDefaultAsync(u=>u.UserName.Equals("08037862905"));
                var patron2 = await _context.Users.FirstOrDefaultAsync(u=>u.UserName.Equals("08053094604"));
                var confirmedBy = await _context.Users.FirstOrDefaultAsync(u=>u.UserName.Equals("08064028176"));

                var variant  = _context.Variants.Include(v=>v.VariantCopies).Where(v=>v.ISBN == "90149008921").SingleOrDefault();
                var variant2  = _context.Variants.Include(v=>v.VariantCopies).Where(v=>v.ISBN == "90149008993").SingleOrDefault();

                var checkOuts = new CheckOut[] {
                    new CheckOut(patron.Id, _context.CheckOutStatuses.SingleOrDefault(cs=> cs.Name.Equals("Borrow Initiated")).Id) { 
                        PatronId = patron.Id, 
                        VariantId = variant.Id,            
                        VariantCopyId = variant.VariantCopies.ToList()[1].Id,
                        RequestedDaysId = GetTerm("2", "book-days-allowed").Id,
                        ApprovedDaysId = GetTerm("1", "book-days-allowed").Id
                    },
                    new CheckOut(patron2.Id, _context.CheckOutStatuses.SingleOrDefault(cs=> cs.Name.Equals("Borrow Initiated")).Id) { 
                        PatronId = patron2.Id, 
                        VariantId = variant2.Id, 
                        VariantCopyId = variant.VariantCopies.ToList()[0].Id,                   
                        RequestedDaysId = GetTerm("3", "book-days-allowed").Id,
                        ApprovedDaysId = GetTerm("1", "book-days-allowed").Id,                        
                    }                    
                };
                _context.CheckOuts.AddRange(checkOuts);
                await _context.SaveChangesAsync(); 
                
                var checkOut1 = await _context.CheckOuts.FirstAsync();
                var checkOut2 = await _context.CheckOuts.OrderByDescending(c=>c.Id).FirstAsync();

                var states = new List<CheckOutState> {
                    new CheckOutState(){ CheckOut = checkOut1, ModifiedByUserId = confirmedBy.Id, StatusId = _context.CheckOutStatuses.SingleOrDefault(cs=> cs.Name.Equals("Borrow Approved")).Id },
                    new CheckOutState(){ CheckOut = checkOut2, ModifiedByUserId = confirmedBy.Id, StatusId = _context.CheckOutStatuses.SingleOrDefault(cs=> cs.Name.Equals("Borrow Approved")).Id },
                    new CheckOutState(){ CheckOut = checkOut2, ModifiedByUserId = patron2.Id, StatusId = _context.CheckOutStatuses.SingleOrDefault(cs=> cs.Name.Equals("Return Initiated")).Id }
                };
                _context.CheckOutStates.AddRange(states);
                _context.SaveChanges();  
            }              
        }
        private static async Task SaveTermsAsync(Term[] terms)
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
        // private static async Task<Term> GetTerm(string name, string termSet)
        // {
        //     return await _context.Terms.Include(t => t.TermSet).Where(t => t.Name.Equals(name) && t.TermSet.Name.Equals(termSet)).FirstOrDefaultAsync();
        // }

        private static Term GetTerm(string name, string termSet)
        {
            return _context.Terms.Include(t => t.TermSet).Where(t => t.Name.Equals(name) && t.TermSet.Name.Equals(termSet)).FirstOrDefault();
        }


        private static Variant GetVariant(string isbn, string format)
        {
            return _context.Variants.Include(v => v.Book).Include(v => v.Format).SingleOrDefault(v => v.ISBN == isbn && v.Format.Name == format);
        }
        private static Image GetBookCover(string name, string sourcePath = "/home/smith/Pictures/downloads/31263r.jpg")
        {
            var fileInfo = new FileInfo(sourcePath);
            var extension = fileInfo.Extension;
            
            var destPath = $"/home/smith/Desktop/Library/src/Library.Web/wwwroot/uploads/images/books/{name}{extension}";            

            File.Copy(sourcePath, destPath, true);
            var image = new Image { Path = destPath, Extension = Path.GetExtension(destPath) };
            return image;
        }

    }
}