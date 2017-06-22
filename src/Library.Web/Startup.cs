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
using Library.Web.Extensions;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Library.Web.Models.CommonPageSettingsViewModels;
using Library.Web.Code;
using Library.Web.Models.BookViewModels;
using Library.Web.Models.StateViewModels;
using Library.Web.Models.LocationViewModels;
using Microsoft.AspNetCore.Http;

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
            // services.AddEntityFrameworkSqlite().AddDbContext<LibraryDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"), o => o.MigrationsAssembly("Library.Web")));
            // services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), o => o.MigrationsAssembly("Library.Web")));
            services.AddEntityFrameworkNpgsql().AddDbContext<LibraryDbContext>(options => { 
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), o => o.MigrationsAssembly("Library.Web"));
            });
            services.AddIdentity<User, Role>().AddEntityFrameworkStores<LibraryDbContext>().AddDefaultTokenProviders();
            services.AddAuthentication(auth => {
                auth.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                auth.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                auth.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });   
            services.AddCookieAuthentication(o => {
                o.AccessDeniedPath = new PathString("/Account/AccessDenied");
                o.CookieName = "Cookies";
                o.ExpireTimeSpan =TimeSpan.FromMinutes(20);
                o.SlidingExpiration = true;
                o.LoginPath = new PathString("/Home");
                o.LogoutPath = new PathString("/Home");                
            }); 
            services.AddAuthorization(options => {
                options.AddPolicy("ElevatedRights", policy => policy.RequireRole("SuperAdministrator", "Administrator"));
                options.AddPolicy("SuperAdministratorOnly", policy => policy.RequireRole("SuperAdministrator"));
                options.AddPolicy("AdministratorOnly", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("MemberOnly", policy => policy.RequireRole("Member"));
                options.AddPolicy("WebmasterOnly", policy => policy.RequireRole("Webmaster"));
                options.AddPolicy("LibrarianOnly", policy => policy.RequireRole("Librarian"));
            });   
            var mapperConfig = new AutoMapper.MapperConfiguration(cfg => {
                cfg.CreateMap<LocationEditorViewModel, Location>();
                cfg.CreateMap<Location, LocationEditorViewModel>();
                cfg.CreateMap<StateEditorViewModel, State>();
                cfg.CreateMap<State, StateEditorViewModel>();
                cfg.CreateMap<Edition, BookEditionEditorViewModel>();
                cfg.CreateMap<BookEditionEditorViewModel, Edition>();
                cfg.CreateMap<Volume, BookVolumeEditorViewModel>();
                cfg.CreateMap<BookVolumeEditorViewModel, Volume>();      
                cfg.CreateMap<VariantLocation, BookLocationEditorViewModel>();             
                cfg.CreateMap<BookLocationEditorViewModel, VariantLocation>();
                cfg.CreateMap<VariantLocation, BookLocationEditorViewModel>();             
                cfg.CreateMap<BookLocationEditorViewModel, VariantLocation>();
                cfg.CreateMap<Variant, BookTypeEditorViewModel>();
                cfg.CreateMap<BookTypeEditorViewModel, Variant>();
                cfg.CreateMap<Variant, BookBasicEditorViewModel>();
                cfg.CreateMap<BookBasicEditorViewModel, Variant>();
                cfg.CreateMap<BookBasicEditorViewModel, Book>();
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper); 
            services.AddMvc(config => {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });            
            // services.ConfigureApplicationCookie(options =>{
            //     options.AccessDeniedPath = "/Account/AccessDenied";
            //     options.CookieName = "Cookies";
            //     options.LogoutPath = "/Home";
            // });
            services.Configure<RazorViewEngineOptions>(options => {
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Areas/Control/{2}/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/Control/{2}/Views/Shared/{0}.cshtml");                
                options.AreaViewLocationFormats.Add("/Areas/Control/Views/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/Control/Views/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/Members/Views/{1}/{0}.cshtml"); 
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");               

            });
            services.Configure<ImageUploadSettings>(Configuration.GetSection("ImageUploadSettings"));            
            services.Configure<SitePageSettingsViewModel>(options => Configuration.GetSection("Library").Bind(options));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SetUpDatabaseAndSeedData(app); 
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");                
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            

            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        private void SetUpDatabaseAndSeedData(IApplicationBuilder app) {
            var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();                
            var databaseFacade = serviceScope.ServiceProvider.GetService<LibraryDbContext>().Database;                
            if(databaseFacade.GetAppliedMigrations().Count() == 0){
                databaseFacade.Migrate();
                var task = Task.Run(() => serviceScope.EnsureRolesCreated());
                var continuation = task.ContinueWith((previous) => {
                    if(previous.IsCompleted){
                        var userServiceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                        userServiceScope.EnsureUsersCreated();
                    }
                });                
            }
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
