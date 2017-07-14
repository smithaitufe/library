using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Library.Repo;
using Library.Web.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Design;

namespace Library.Web
{
    public class LibraryDbContextFactory: IDesignTimeDbContextFactory<LibraryDbContext> 
    {
        // public LibraryDbContext CreateDbContext(string[] args){
        //     var builder = new DbContextOptions<LibraryDbContext>();
        //     return new LibraryDbContext(builder);
        // }
        // public LibraryDbContext Create(string[] args)
        //     => Program.BuildWebHost(args).Services
        //     .GetRequiredService<LibraryDbContext>();

        public LibraryDbContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var contentRootPath = System.IO.Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
                            .SetBasePath(contentRootPath)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{environmentName}.json", true);

            builder.AddEnvironmentVariables();
            var _configuration = builder.Build();

            //setup DI
            var containerBuilder = new ContainerBuilder();

            IServiceCollection services = new ServiceCollection();            
            services.AddCustomizedDataStore(_configuration);

            containerBuilder.Populate(services);
            var _serviceProvider = containerBuilder.Build().Resolve<IServiceProvider>();

            return _serviceProvider.GetRequiredService<LibraryDbContext>();

        }
            
    }
}