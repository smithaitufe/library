using Library.Repo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;



namespace Library.Web
{
    // public class LibraryDbContextFactory: IDesignTimeDbContextFactory<LibraryDbContext> 
    // {
    //     public LibraryDbContext CreateDbContext(string[] args){
    //         var builder = new DbContextOptionsBuilder<LibraryDbContext>();
    //         return new LibraryDbContext(builder);
    //     }

    // }

    // class LibraryDbContextFactory : IDbContextFactory<LibraryDbContext>
    // {
    //     public LibraryDbContext Create(string[] args)
    //         => Program.BuildWebHost(args)
    //         .Services.CreateScope()
    //         // .Services.GetRequiredService<LibraryDbContext>();
    // }
        
}
