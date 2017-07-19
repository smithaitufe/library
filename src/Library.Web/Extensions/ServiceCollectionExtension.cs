using System;
using Library.Core.Models;
using Library.Repo;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace Library.Web.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection AddCustomizedIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<LibraryDbContext>();


            return services;   
        }
        public static IServiceCollection AddCustomizedIdentityAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(auth => {
                auth.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                auth.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                auth.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });   
            services.AddCookieAuthentication(o => {
                o.AccessDeniedPath = new PathString("/Account/AccessDenied");
                o.CookieName = "Cookies";
                o.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                o.SlidingExpiration = true;
                o.LoginPath = new PathString("/Home");
                o.LogoutPath = new PathString("/Home");                
            }); 
            return services;
    
        }

        public static IServiceCollection AddCustomizedIdentityAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options => {
                options.AddPolicy("ElevatedRights", policy => policy.RequireRole("SuperAdministrator", "Administrator"));
                options.AddPolicy("SuperAdministratorOnly", policy => policy.RequireRole("SuperAdministrator"));
                options.AddPolicy("AdministratorOnly", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("MemberOnly", policy => policy.RequireRole("Member"));
                options.AddPolicy("WebmasterOnly", policy => policy.RequireRole("Webmaster"));
                options.AddPolicy("LibrarianOnly", policy => policy.RequireRole("Librarian"));
            }); 
            return services;  
    
        }


        public static IServiceCollection AddCustomizedDataStore(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddEntityFrameworkNpgsql().AddDbContext<LibraryDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), o => o.MigrationsAssembly("Library.Web")));
            
            services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), o=>o.MigrationsAssembly("Library.Web")));
            return services;
        }
    }
}