using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Data;
using Library.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Web.Extensions
{
    public static class LibraryDbContextExtension
    {
        static LibraryDbContext _context;
        static IHostingEnvironment _environment;

        public static async void RunSeedData(this LibraryDbContext context, IServiceScope serviceScope, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
            var migrationCompleted = context.AllMigrationsApplied();
            if (migrationCompleted)
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();     
                
                if (!_context.TermSets.Any())
                {
                    var termSets = new TermSet[]{
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
        new TermSet { Name = "book-reservation-reason"},
    };
                    termSets.ToList().ForEach(ts => _context.TermSets.Add(ts));
                    _context.SaveChanges();

                    var users = _context.Users.ToList();

                    var bookFormat = _context.TermSets.Where(ts => ts.Name.Equals("book-format")).First();
                    var terms = new Term[]{
                        new Term{ Name="EBook", TermSetId = bookFormat.Id},
                        new Term{ Name="Hard Cover", TermSetId = bookFormat.Id}
                    };
                    await SaveTerms(terms);

                    var termSet = _context.TermSets.Where(ts => ts.Name.Equals("book-reservation-reason")).First();
                    terms = new Term[]{
                        new Term{ Name="Membership", TermSetId = termSet.Id},
                        new Term{ Name="Active Subscription", TermSetId = termSet.Id},
                        new Term{ Name="No Pending Borrowed Books", TermSetId = termSet.Id},
                        new Term{ Name="Returns Regular", TermSetId = termSet.Id},
                        new Term{ Name="Returns", TermSetId = termSet.Id},
                    };
                    await SaveTerms(terms);

                

                    var parentCheckOutStatuses = new CheckOutStatus[]{
                        new CheckOutStatus{ Name="Borrow Initiated", Out =  true},                        
                        new CheckOutStatus{ Name="Return Initiated"}                       
                    };     
                    parentCheckOutStatuses.ToList().ForEach(cos => _context.CheckOutStatuses.Add(cos));
                    _context.SaveChanges();
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
                    
                    var grant = _context.TermSets.Where(ts => ts.Name.Equals("book-sale-grant")).First();

                    terms = new Term[]{
        new Term{ Name= "Free", TermSetId = grant.Id},
        new Term{ Name= "Borrow Only", TermSetId = grant.Id },
        new Term { Name = "For Sale", TermSetId = grant.Id }
    };
                    await SaveTerms(terms);

                    var announcementCategory = _context.TermSets.Where(ts => ts.Name.Equals("announcement-category")).First();
                    terms = new []{
                        new Term{ Name= "Member", TermSetId = announcementCategory.Id},
                        new Term{ Name= "Staff", TermSetId = announcementCategory.Id },
                        new Term { Name = "General", TermSetId = announcementCategory.Id },
                        new Term { Name = "Public", TermSetId = announcementCategory.Id },
                    };
                    await SaveTerms(terms);



                    var bookSource = _context.TermSets.Where(ts => ts.Name.Equals("book-source")).First();
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

                    
                    (new Location[] {
                        new Location { Name ="Agbor", Code = "AGB" },
                        new Location { Name ="Asaba", Code = "ASB" },
                        new Location { Name ="Oghara", Code = "OGH" },
                        new Location { Name ="Ughelli", Code = "UGH" },
                        new Location { Name ="Warri", Code = "WAR" }
                    }).ToList().ForEach(l => _context.Locations.Add(l));

                    var availabilty = _context.TermSets.Where(ts => ts.Name.Equals("book-availability")).First();
                    terms = new Term[] {
    new Term { Name ="On Shelf", TermSetId = availabilty.Id },
    new Term { Name ="Off Shelf", TermSetId = availabilty.Id }
};
                    await SaveTerms(terms);

                    var collectionMode = _context.TermSets.Where(ts => ts.Name.Equals("book-collection-mode")).First();
                    terms = new Term[] {
    new Term { Name ="All Users", TermSetId = collectionMode.Id },
    new Term { Name ="Paid Users", TermSetId = collectionMode.Id }
};
                    await SaveTerms(terms);

                    var category = _context.TermSets.Where(ts => ts.Name.Equals("book-category")).First();
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
                    var bookPrice = _context.TermSets.Where(ts => ts.Name.Equals("book-price")).First();
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

                    var postCategory = _context.TermSets.Where(ts => ts.Name.Equals("post-category")).First();
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

                    var language = _context.TermSets.Where(ts => ts.Name.Equals("book-language")).First();
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

                    var condition = _context.TermSets.Where(ts => ts.Name.Equals("book-condition")).First();
                    terms = new Term[] {
                        new Term { Name ="New", TermSetId = condition.Id },
                        new Term { Name ="Used", TermSetId = condition.Id },
                        new Term { Name ="New - Refixed", TermSetId = condition.Id },
                        new Term { Name ="Used - Refixed", TermSetId = condition.Id }
                    };
                    await SaveTerms(terms);


                    var fine = _context.TermSets.Where(ts => ts.Name.Equals("book-fine")).First();
                    terms = new Term[] {
                        new Term { Name ="10", TermSetId = fine.Id },
                        new Term { Name ="15", TermSetId = fine.Id },
                        new Term { Name ="20", TermSetId = fine.Id },
                        new Term { Name ="30", TermSetId = fine.Id }
                    };
                    await SaveTerms(terms);

                    var tsDaysAllowed = _context.TermSets.Where(ts => ts.Name.Equals("book-days-allowed")).First();
                    var daysAllowed = new List<Term>();

                    for (var i = 1; i <= 30; i++)
                    {
                        daysAllowed.Add(new Term { Name = i.ToString(), TermSetId = tsDaysAllowed.Id });
                    }
                    await SaveTerms(daysAllowed.ToArray());


                    var year = _context.TermSets.Where(ts => ts.Name.Equals("book-year")).First();
                    var years = new List<Term>();

                    for (var i = 2001; i <= DateTime.UtcNow.Year; i++)
                    {
                        years.Add(new Term { Name = i.ToString(), TermSetId = year.Id });
                    }
                    await SaveTerms(years.ToArray());

                    var genre = _context.TermSets.Where(ts => ts.Name.Equals("genre")).First();
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
                };

                var authors = new Author[] {
                    new Author{ FirstName = "Smith", LastName = "Paul", Email = "paulsmith@hotmail.com", PhoneNumber = "+11893778348"},
                    new Author{ FirstName = "McGreg", LastName = "Danielson", Email = "mcgregdan@esk.com", PhoneNumber = "+91893008391"},
                    new Author { FirstName = "John", LastName ="McGray", Email ="johnmc@gmai.com", PhoneNumber = "+11280987456"},
                    new Author { FirstName = "Lennart", LastName ="Jorelid", Email ="lenjorelid@outlook.com", PhoneNumber = "+98280987400"},
                    new Author { FirstName = "Matthew", LastName ="MacDonald", Email ="mamacd@gmai.com", PhoneNumber = "+233280987456"},
                    new Author { FirstName = "Adam", LastName ="Freeman", Email ="adam.freeman@booksprint.com", PhoneNumber = "+41280980156"}

                };
                if (!_context.Authors.Any())
                {
                    authors.ToList().ForEach(author => _context.Authors.Add(author));
                    _context.SaveChanges();
                }

                var publishers = new Publisher[] {
                    new Publisher{ Name = "Bloomsbury Publishing Plc"},
                    new Publisher{ Name = "A. S. Barnes"},
                    new Publisher{ Name = "Abilene Christian University Press"},
                    new Publisher{ Name = "Ablex Publishing"},
                    new Publisher{ Name = "Academic Press"},
                    new Publisher{ Name = "Addison–Wesley "},
                    new Publisher{ Name = "Pearson Education"},
                    new Publisher{ Name = "Apress"},
                    new Publisher{ Name = "Baker Book House"},
                    new Publisher{ Name = "Beacon Publishing"},
                    new Publisher{ Name = "Bogle-L'Ouverture Publications"},
                    new Publisher{ Name = "Canongate Books"},
                    new Publisher{ Name = "Cambridge University Press"},
                    new Publisher{ Name = "Random House"},
                    new Publisher{ Name = "Delacorte Press"},
                    new Publisher{ Name = "Faber and Faber"},
                    new Publisher{ Name = "Four Walls Eight Windows"},
                    new Publisher{ Name = "Gefen Publishing House"},
                    new Publisher{ Name = "Greenleaf Publishing Ltd"},
                    };
                if (!_context.Publishers.Any())
                {
                    publishers.ToList().ForEach(publisher => _context.Publishers.Add(publisher));
                    _context.SaveChanges();
                }

                var books = new Book[] {
                    new Book {
                        Title = "Beginning ASP.NET 4.5 in C#", ISBN = "90149008993",
                        GenreId = GetTerm("Computer", "genre").Id, PublisherId = _context.Publishers.First().Id,
                        CategoryId = GetTerm("Flip Book", "book-category").Id,
                        Cover = GetBookCover("beginning-aspnet")
                    },
                    new Book {
                        Title = "Beginning ASP.NET Core", ISBN = "11220986734",
                        GenreId = GetTerm("Computer", "genre").Id, PublisherId = _context.Publishers.First().Id,
                        CategoryId = GetTerm("Flip Book", "book-category").Id,
                        Cover = GetBookCover("beginning-aspnetcore")
                    }

                };

                var variants = new Variant[]{
                        new Variant {
                            Book = books[0], Format = _context.Terms.Include(term => term.TermSet).Where(term => term.Name.Equals("Hard Cover") && term.TermSet.Name.Equals("book-format")).First(), Pages = 200,
                            YearId = GetTerm("2009", "book-year").Id,            
                            GrantId = GetTerm("Free", "book-sale-grant").Id, CollectionModeId = GetTerm("All Users", "book-collection-mode").Id,
                            FineId = GetTerm("10", "book-fine").Id, DaysAllowedId = GetTerm("10", "book-days-allowed").Id
                        },
                        new Variant {
                            Book = books[0], Format = _context.Terms.Include(term => term.TermSet).Where(term => term.Name.Equals("EBook") && term.TermSet.Name.Equals("book-format")).First(), Pages = 185,            
                            YearId = GetTerm("2015", "book-year").Id,            
                            GrantId = GetTerm("Free", "book-sale-grant").Id,
                            CollectionModeId = GetTerm("All Users", "book-collection-mode").Id,
                            FineId = GetTerm("10", "book-fine").Id,
                            DaysAllowedId = GetTerm("10", "book-days-allowed").Id,
                        },
                        new Variant {
                            Book = books[1], Format = _context.Terms.Include(term => term.TermSet).Where(term => term.Name.Equals("Hard Cover") && term.TermSet.Name.Equals("book-format")).First(), Pages = 1500,            
                            YearId = GetTerm("2016", "book-year").Id,            
                            GrantId = GetTerm("Free", "book-sale-grant").Id,
                            CollectionModeId = GetTerm("All Users", "book-collection-mode").Id,
                            FineId = GetTerm("10", "book-fine").Id,
                            DaysAllowedId = GetTerm("10", "book-days-allowed").Id,
                        }
                };
                variants.ToList().ForEach(variant => _context.Variants.Add(variant));
                _context.SaveChanges();
                var locations = new VariantLocation[] {
                    new VariantLocation { SourceId = GetTerm("Internet Archive", "book-source").Id, VariantId = GetVariant("90149008993", "Hard Cover").Id, LocationId = _context.Locations.SingleOrDefault(l => l.Name.Equals("Asaba")).Id, AvailabilityId = GetTerm("On Shelf", "book-availability").Id },
                    new VariantLocation { SourceId = GetTerm("Internet Archive", "book-source").Id, VariantId = GetVariant("90149008993", "Hard Cover").Id, LocationId = _context.Locations.SingleOrDefault(l => l.Name.Equals("Agbor")).Id, AvailabilityId = GetTerm("On Shelf", "book-availability").Id},
                    new VariantLocation { SourceId = GetTerm("Internet Archive", "book-source").Id, VariantId = GetVariant("90149008993", "Hard Cover").Id, LocationId = _context.Locations.SingleOrDefault(l => l.Name.Equals("Warri")).Id, AvailabilityId = GetTerm("On Shelf", "book-availability").Id },
                    new VariantLocation { SourceId = GetTerm("State Purchased", "book-source").Id, VariantId = GetVariant("90149008993", "EBook").Id, LocationId = _context.Locations.SingleOrDefault(l => l.Name.Equals("Agbor")).Id, AvailabilityId = GetTerm("On Shelf", "book-availability").Id },
                    new VariantLocation { SourceId = GetTerm("Individual Donated", "book-source").Id, VariantId = GetVariant("11220986734", "Hard Cover").Id, LocationId = _context.Locations.SingleOrDefault(l => l.Name.Equals("Ughelli")).Id, AvailabilityId = GetTerm("On Shelf", "book-availability").Id},
                    new VariantLocation { SourceId = GetTerm("State Purchased", "book-source").Id, VariantId = GetVariant("11220986734", "Hard Cover").Id, LocationId = _context.Locations.SingleOrDefault(l => l.Name.Equals("Warri")).Id, AvailabilityId = GetTerm("On Shelf", "book-availability").Id }
                };
                locations.ToList().ForEach((location) => {
                    var variant = _context.Variants.Include(c => c.Book).ThenInclude(b => b.Category).SingleOrDefault(v => v.Id == location.VariantId);
                    var bookService = new Services.BookService(_context);
                    location.SerialNo =  bookService.GetSerialNo(location.LocationId, variant.Book.CategoryId).Result;
                    _context.VariantLocations.Add(location);
                    _context.SaveChanges();
                });
                
                var prices = new VariantPrice[]{
                    new VariantPrice { ConditionId = GetTerm("New", "book-condition").Id, PriceId = GetTerm("Membership","book-price").Id, VariantId = GetVariant("90149008993", "Hard Cover").Id },
                    new VariantPrice { ConditionId = GetTerm("New", "book-condition").Id, PriceId = GetTerm("Free","book-price").Id, VariantId = GetVariant("90149008993", "EBook").Id},
                    new VariantPrice { ConditionId = GetTerm("New", "book-condition").Id, PriceId = GetTerm("Membership","book-price").Id, VariantId = GetVariant("11220986734", "Hard Cover").Id }
                };
                prices.ToList().ForEach(price => _context.VariantPrices.Add(price));
                
                var languages = new VariantLanguage[]{
                    new VariantLanguage { LanguageId =  GetTerm("English", "book-language").Id, VariantId = GetVariant("90149008993", "Hard Cover").Id},
                    new VariantLanguage { LanguageId =  GetTerm("French", "book-language").Id, VariantId = GetVariant("90149008993", "Hard Cover").Id},
                    new VariantLanguage { LanguageId =  GetTerm("English", "book-language").Id, VariantId = GetVariant("11220986734", "Hard Cover").Id}
                };
                languages.ToList().ForEach(language => _context.VariantLanguages.Add(language));
                _context.SaveChanges();

                var bookAuthors = new List<BookAuthor> {
                    new BookAuthor { Author = _context.Authors.FirstOrDefault(), Book = _context.Books.First() },
                    new BookAuthor { Author = _context.Authors.FirstOrDefault(a=>a.Email.Equals("mcgregdan@esk.com")), Book = _context.Books.First(b => b.ISBN.Equals("90149008993")) },
                    new BookAuthor { Author = _context.Authors.FirstOrDefault(a=>a.Email.Equals("mamacd@gmai.com")), Book = _context.Books.First(b => b.ISBN.Equals("11220986734")) },
                    new BookAuthor { Author = _context.Authors.FirstOrDefault(a=>a.Email.Equals("paulsmith@hotmail.com")), Book = _context.Books.First(b => b.ISBN.Equals("11220986734")) }
                };
                bookAuthors.ToList().ForEach(ba => _context.BookAuthors.Add(ba));
                _context.SaveChanges();

                var editions = new Edition[] {
                    new Edition { VariantId =  GetVariant("90149008993", "Hard Cover").Id, Name = "1st Ed."},
                    new Edition { VariantId = GetVariant("11220986734", "Hard Cover").Id, Name = "1st Ed."},
                    new Edition { VariantId = GetVariant("11220986734", "Hard Cover").Id, Name = "2nd Ed."}
                };
                editions.ToList().ForEach(edition => _context.Editions.Add(edition));

                var volumes = new Volume[] {
                    new Volume { VariantId =  GetVariant("90149008993", "Hard Cover").Id, Name = "Vol. II"},
                    new Volume { VariantId = GetVariant("11220986734", "Hard Cover").Id, Name = "Vol. I"}
                };
                volumes.ToList().ForEach(volume => _context.Volumes.Add(volume));
                _context.SaveChanges();
                
                var patron = await userManager.FindByNameAsync("08138238095");
                var patron2 = await userManager.FindByNameAsync("08053094604");
                var confirmedBy = await userManager.FindByNameAsync("08064028176");
                var bookLocation = GetTerm("Asaba", "book-location");
                
                var checkOuts = new CheckOut[] {
                    new CheckOut(patron.Id, context.CheckOutStatuses.SingleOrDefault(cs=> cs.Name.Equals("Borrow Initiated")).Id) { 
                        PatronId = patron.Id, VariantId = _context.Variants.First().Id, LocationId = bookLocation.Id,
                        RequestedDaysId = GetTerm("10", "book-days-allowed").Id
                    },
                    new CheckOut(patron.Id, context.CheckOutStatuses.SingleOrDefault(cs=> cs.Name.Equals("Borrow Initiated")).Id) { 
                        PatronId = patron.Id, VariantId = _context.Variants.FirstOrDefault(bv => bv.Id == 3).Id, LocationId = bookLocation.Id, 
                        RequestedDaysId = GetTerm("7", "book-days-allowed").Id
                    },
                    new CheckOut(patron2.Id, context.CheckOutStatuses.SingleOrDefault(cs=> cs.Name.Equals("Borrow Initiated")).Id) { 
                        PatronId = patron.Id, VariantId = _context.Variants.FirstOrDefault(bv => bv.Id == 3).Id, LocationId = bookLocation.Id, 
                        RequestedDaysId = GetTerm("7", "book-days-allowed").Id                        
                    }
                    
                };
                _context.CheckOuts.AddRange(checkOuts);
                _context.SaveChanges();               
                
                var states = new List<CheckOutState> {
                    new CheckOutState(){ CheckOut = checkOuts[1], ModifiedByUserId = confirmedBy.Id, StatusId = context.CheckOutStatuses.SingleOrDefault(cs=> cs.Name.Equals("Approved")).Id },
                    new CheckOutState(){ CheckOut = checkOuts[2], ModifiedByUserId = confirmedBy.Id, StatusId = context.CheckOutStatuses.SingleOrDefault(cs=> cs.Name.Equals("Approved")).Id },
                    new CheckOutState(){ CheckOut = checkOuts[2], ModifiedByUserId = patron2.Id, StatusId = context.CheckOutStatuses.SingleOrDefault(cs=> cs.Name.Equals("Return Initiated")).Id }
                };
                _context.CheckOutStates.AddRange(states);
                _context.SaveChanges();

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
                    foreach (var genre in genres)
                    {
                        _context.ClubGenres.Add(new ClubGenre { ClubId = _context.Clubs.FirstOrDefault(c => c.Name.Equals(club)).Id, GenreId = _context.Terms.Include(t => t.TermSet).FirstOrDefault(t => t.Name.Equals(genre) && t.TermSet.Name.Equals("genre")).Id });
                    }
                }
                _context.SaveChanges();

                var members = new Member[]{
                    new Member { UserId = patron.Id, ClubId = _context.Clubs.SingleOrDefault(c => c.Name.Equals("Sisi")).Id },
                    new Member { UserId = patron.Id, ClubId = _context.Clubs.SingleOrDefault(c => c.Name.Equals("Lupta")).Id }
                };
                _context.Members.AddRange(members);
                _context.SaveChanges();
                SeedPosts();
                SeedAnnouncements();
                (new List<Country>{
                    new Country { Name ="Ghana", TelephoneCode = "+233", Abbreviation = "GH"},
                    new Country { Name ="Nigeria", TelephoneCode = "+234", Abbreviation = "NIG"},

                }).ForEach(country => _context.Countries.Add(country));
                await _context.SaveChangesAsync();

                (new List<State> {
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
                    new State { CountryId = _context.Countries.SingleOrDefault(c => c.Abbreviation.Equals("NIG")).Id,  Name = "FCT" } ,
                    

                    
                }).ForEach(s => { _context.States.Add(s); _context.SaveChanges(); });
            }

        }
        private static async Task SaveTerms(Term[] terms)
        {
            terms.ToList().ForEach(term => _context.Terms.Add(term));
            await _context.SaveChangesAsync();
        }
        private static Term GetTerm(string name, string termSet)
        {
            return _context.Terms.Include(t => t.TermSet).Where(t => t.Name.Equals(name) && t.TermSet.Name.Equals(termSet)).First();
        }
        private static Variant GetVariant(string isbn, string format)
        {
            return _context.Variants.Include(v => v.Book).Include(v => v.Format).SingleOrDefault(v => v.Book.ISBN == isbn && v.Format.Name == format);
        }
        private static Image GetBookCover(string name, string sourcePath = "/home/smith/Downloads/book-013.jpg")
        {
            var destPath = $"/uploads/images/books/{name}.jpg";
            var absolutePath = _environment.WebRootPath + destPath;
            File.Copy(sourcePath, absolutePath, true);
            var image = new Image { Path = destPath, AbsolutePath = absolutePath, Extension = Path.GetExtension(destPath) };
            return image;
        }
        private static void SeedAnnouncements() 
        {
            var announcements = new Announcement[] {
                new Announcement { Title = "Borrowing of books has just been made easier", Introduction = "Since inception, borrowing of books has been a challenge. But Delta State Library has chosen to automate the process", Text = "Since inception, borrowing of books has been a challenge. But Delta State Library has chosen to automate the process. <p>You can walk into any our libraries and borrow book without the assistance of any library staff. We are at pal with the world advanced libraries. We have adopted the use of RFID automation to ensure that our books are signed accordingly</p>", ReleaseAt = DateTime.Now.AddDays(2), ExpiresAt = DateTime.Now.AddDays(4), CategoryId = GetTerm("Member", "announcement-category").Id},
                new Announcement { Title = "How long should one possess a borrowed book?", Introduction = "You may have been wondering about the question raised in the <b>title</b>. At the end of the management meeting, which was held on 22 April, 2016 we are", Text = "<p>You may have been wondering about the question raised in the <b>title</b>. At the end of the management meeting, which was held on 22 April, 2016 we are</p>", ReleaseAt = DateTime.Now.AddDays(2), ExpiresAt = DateTime.Now.AddDays(4), CategoryId = GetTerm("Member", "announcement-category").Id}
            };
            announcements.ToList().ForEach(announcement => _context.Announcements.Add(announcement));
            _context.SaveChanges();
        }
        private static void SeedPosts() {
            var posts = new Post[] {
                new Post {
                    Title = "How to carry out stack overflow (A case study of the modern programming)", 
                    Text = @"<h2>A Call out to Developers</h2><p>Any bugfix can be send to our main github code repository for review straight away. If you are not sure or want to take on a bigger task or change feel free to open up a thread on our forum where you write down your proposal to get some initial feedback.</p><p>Wiki pages to get you started:*http://kodi.wiki/view/Development<br/>Forum:*Developer sucbsection<br/>Code on Github:*https://github.com/xbmc/xbmc</p>Regards As Always", AuthorId = _context.Users.ToList()[0].Id,  ClubId = _context.Clubs.ToList()[3].Id, CategoryId = GetTerm("Researching", "post-category").Id
                },
                new Post {
                    Title = "How did Oliver get to London? ( Extract from Oliver Twist Title)", 
                    Text = @"<h2>A Call out to Developers</h2><p>Any bugfix can be send to our main github code repository for review straight away. If you are not sure or want to take on a bigger task or change feel free to open up a thread on our forum where you write down your proposal to get some initial feedback.</p><p>Wiki pages to get you started:*http://kodi.wiki/view/Development<br/>Forum:*Developer sucbsection<br/>Code on Github:*https://github.com/xbmc/xbmc</p>Regards As Always", AuthorId = _context.Users.ToList()[0].Id,  ClubId = _context.Clubs.ToList()[3].Id, CategoryId = GetTerm("Review", "post-category").Id,
                    Comments = new Comment[] {
                        new Comment { CommenterId = _context.Users.ToList()[2].Id, Text = comments[0], Status = CommentStatus.Approved},
                        new Comment { CommenterId = _context.Users.ToList()[0].Id, Text = comments[1], Status = CommentStatus.Approved},
                        new Comment { CommenterId = _context.Users.ToList()[3].Id, Text = comments[2], Status = CommentStatus.InReview},
                        new Comment { CommenterId = _context.Users.ToList()[0].Id, Text = comments[3], Status = CommentStatus.Rejected},
                    }
                }
            };
            posts.ToList().ForEach(post => _context.Posts.Add(post));
            _context.SaveChanges();
        }

        static string[] comments = new string[]{
            @"<p>Thank you Smith for this educative post, really it helped me to get started on Stack overflow</p><p>Have you seen this new programming book ?</p><p>21 Guides to Advanced Web and Developement, check it out <br/>and let us know the most inspiring reading clubs now, have a nice day house.</p>",
            @"<p>Thank you Patrick, you are so kind</p><p>AAm yet to see the book, I will check it out from the catalogue or maybe over the shelf if I have to. Take care of yourself.</p>",
            @"<p>Thank you Smith. I really enjoyed the post</p><p>I have been thinking about this topic. Your post simply corrected some misconceptions I had about it</p>",
            @"<p>Thank you Isioma, you are so kind</p><p>AAm yet to see the book, I will check it out from the catalogue or maybe over the shelf if I have to. Take care of yourself.</p>"
        };
    }
}