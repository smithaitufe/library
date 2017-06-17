using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Repo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Library.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Library.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddEntityFrameworkSqlite().AddDbContext<LibraryDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"), o => o.MigrationsAssembly("Library.Web")));
            // services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), o => o.MigrationsAssembly("Library.Web")));
            // services
            // .AddEntityFrameworkNpgsql()
            // .AddDbContext<LibraryDbContext>(options => { 
            //     options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), o => o.MigrationsAssembly("Library.Web"));
            // });
            services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<LibraryDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(auth => {
                auth.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                auth.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                auth.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    // public class LibraryDbContextFactory: IDesignTimeDbContextFactory<LibraryDbContext> 
    // {
    //     public LibraryDbContext CreateDbContext(string[] args){
    //         var builder = new DbContextOptionsBuilder<LibraryDbContext>();
    //         return new LibraryDbContext(builder);
    //     }
            
    // }

    class LibraryDbContextFactory : IDbContextFactory<LibraryDbContext>
    {
        public LibraryDbContext Create(string[] args)
            => Program.BuildWebHost(args).Services
            .GetRequiredService<LibraryDbContext>();
    }
        
}
