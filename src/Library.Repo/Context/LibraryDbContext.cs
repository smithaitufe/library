using System;
using Library.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Library.Repo
{
    public class LibraryDbContext: IdentityDbContext<User, Role, int>
    {
                public DbSet<Announcement> Announcements { get; set; }
        public DbSet<TermSet> TermSets { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        // public DbSet<BookImage> BookImages { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<VariantPrice> VariantPrices { get; set; }
        public DbSet<VariantLanguage> VariantLanguages { get; set; }
        public DbSet<VariantLocation> VariantLocations { get; set; }
        public DbSet<Volume> Volumes { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<CheckOut> CheckOuts { get; set; }  
        public DbSet<CheckOutStatus> CheckOutStatuses { get; set; }  
        public DbSet<CheckOutState> CheckOutStates { get; set; }
        public DbSet<Inventory> Inventories { get; set; }    
        public DbSet<Recall> Recalls { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubGenre> ClubGenres { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<PriceOffer> PriceOffers { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Country> Countries { get; set; }        


        public LibraryDbContext(): base(){}
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options): base(options){}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            // builder.Entity<Book>().HasMany(b => b.Genre).WithOne(b => b.Book).WillCascadeOnDelete(false);
            
            // foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            // {
            //     relationship.DeleteBehavior = DeleteBehavior.Restrict;
            // }

            builder.Entity<Term>()
            .HasMany(t => t.GenreBooks)
            .WithOne(t => t.Genre)
            .HasForeignKey(b => b.GenreId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Term>()
            .HasMany(t => t.CategoryBooks)
            .WithOne(t => t.Category)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Term>()
            .HasMany(t => t.DaysAllowedVariants)
            .WithOne(t => t.DaysAllowed)
            .HasForeignKey(b => b.DaysAllowedId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Term>()
            .HasMany(t => t.GrantVariants)
            .WithOne(t => t.Grant)
            .HasForeignKey(b => b.GrantId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Term>()
            .HasMany(t => t.FineVariants)
            .WithOne(t => t.Fine)
            .HasForeignKey(b => b.FineId)
            .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Term>()
            .HasMany(t => t.FormatVariants)
            .WithOne(t => t.Format)
            .HasForeignKey(b => b.FormatId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Term>()
            .HasMany(t => t.YearVariants)
            .WithOne(t => t.Year)
            .HasForeignKey(b => b.YearId)
            .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Term>()
            .HasMany(t => t.CheckOutRequestedDays)
            .WithOne(t => t.RequestedDays)
            .HasForeignKey(t => t.RequestedDaysId)
            .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Term>()
            .HasMany(t => t.CheckOutApprovedDays)
            .WithOne(t => t.ApprovedDays)
            .HasForeignKey(t => t.ApprovedDaysId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Post>()
            .HasMany(t => t.Comments)
            .WithOne(t => t.Post)
            .HasForeignKey(t => t.PostId)
            .OnDelete(DeleteBehavior.Restrict);               

            builder.Entity<Variant>()
            .HasMany(t => t.CheckOuts)
            .WithOne(t => t.Variant)
            .HasForeignKey(t => t.VariantId)
            .OnDelete(DeleteBehavior.Restrict);  
        
            builder.Entity<Variant>()
            .HasMany(t => t.Inventories)
            .WithOne(t => t.Variant)
            .HasForeignKey(t => t.VariantId)
            .OnDelete(DeleteBehavior.Restrict);              

            builder.Entity<User>()
            .HasMany(u => u.ReservationBookings)
            .WithOne(u=> u.ReservedBy)
            .HasForeignKey( u => u.ReservedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
            .HasMany(u => u.Reservations)
            .WithOne(u=> u.Patron)
            .HasForeignKey( u => u.PatronId)           
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Location>()
            .HasMany(u => u.VariantLocations)
            .WithOne(u=> u.Location)
            .HasForeignKey( u => u.LocationId)           
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Term>()
            .HasMany(u => u.AvailabilityVariantLocations)
            .WithOne(u=> u.Availability)
            .HasForeignKey( u => u.AvailabilityId)           
            .OnDelete(DeleteBehavior.Restrict);  

            builder.Entity<Term>()
            .HasMany(u => u.SourceVariantLocations)
            .WithOne(u=> u.Source)
            .HasForeignKey( u => u.SourceId)           
            .OnDelete(DeleteBehavior.Restrict);       

            builder.Entity<Term>()
            .HasMany(t => t.VariantPrices)
            .WithOne(t=> t.Price)
            .HasForeignKey(t => t.PriceId)           
            .OnDelete(DeleteBehavior.Restrict);  

            builder.Entity<Variant>()
            .HasMany(t => t.VariantLocations)
            .WithOne(t=> t.Variant)
            .HasForeignKey(t => t.VariantId)           
            .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Variant>()
            .HasMany(t => t.PricesLink)
            .WithOne(t=> t.Variant)
            .HasForeignKey(t => t.VariantId)           
            .OnDelete(DeleteBehavior.Restrict); 
            
            builder.Entity<User>()
            .HasMany(t => t.CheckOutStates)
            .WithOne(t=> t.ModifiedBy)
            .HasForeignKey(t => t.ModifiedByUserId)           
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>().HasMany(u=>u.RecalledBooks)
            .WithOne(u=>u.RecalledBy)
            .HasForeignKey(u=>u.RecalledByUserId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CheckOut>().HasMany(c=>c.RecalledBooks)
            .WithOne(c=>c.CheckOut).HasForeignKey(c=>c.CheckOutId)
            .OnDelete(DeleteBehavior.Restrict);        



            builder.Entity<UserAddress>().HasKey(x => new { x.UserId, x.AddressId });
            builder.Entity<BookAuthor>().HasIndex(x => new { x.BookId, x.AuthorId }).IsUnique(true);            
            builder.Entity<Variant>().HasMany(bv => bv.CheckOuts).WithOne(co => co.Variant);
            builder.Entity<User>(i => {
                i.ToTable("Users");
                i.HasKey(x => x.Id);
            });
            builder.Entity<Role>(i =>
            {
                i.ToTable("Roles");
                i.HasKey(x => x.Id);
            });
            builder.Entity<IdentityUserRole<int>>(i =>
            {
                i.ToTable("UserRoles");
                i.HasKey(x => new { x.RoleId, x.UserId });
            });
            builder.Entity<IdentityUserLogin<int>>(i =>
            {
                i.ToTable("UserLogins");
                i.HasIndex(x => new { x.ProviderKey, x.LoginProvider });
            });
            builder.Entity<IdentityRoleClaim<int>>(i =>
            {
                i.ToTable("RoleClaims");
                i.HasKey(x => x.Id);
            });
            builder.Entity<IdentityUserClaim<int>>(i =>
            {
                i.ToTable("UserClaims");
                i.HasKey(x => x.Id);
            });
            builder.Entity<IdentityUserToken<int>>(i => {
                i.ToTable("UserTokens");
                i.HasKey(x => x.UserId);
            });
        }
        
        
    }
}