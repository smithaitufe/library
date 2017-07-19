using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Http;

using Library.Web.Code;
using Library.Web.Extensions;
using Library.Core.Models;
using Library.Web.Models.CommonPageSettingsViewModels;
using Library.Web.Models.BookViewModels;
using Library.Web.Models.StateViewModels;
using Library.Web.Models.LocationViewModels;
using Library.Web.Models.ShelfViewModels;

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
            services.AddCustomizedDataStore(Configuration);
            services.AddCustomizedIdentity();
            services.AddCustomizedIdentityAuthentication();
            services.AddCustomizedIdentityAuthorization();
                       
            var mapperConfig = new AutoMapper.MapperConfiguration(cfg => {
                cfg.CreateMap<LocationEditorViewModel, Location>();
                cfg.CreateMap<Location, LocationEditorViewModel>();
                cfg.CreateMap<StateEditorViewModel, State>();
                cfg.CreateMap<State, StateEditorViewModel>();                   
                cfg.CreateMap<VariantCopy, BookLocationEditorViewModel>();             
                cfg.CreateMap<BookLocationEditorViewModel, VariantCopy>();
                cfg.CreateMap<VariantCopy, BookLocationEditorViewModel>();             
                cfg.CreateMap<BookLocationEditorViewModel, VariantCopy>();
                cfg.CreateMap<Variant, BookTypeEditorViewModel>();
                cfg.CreateMap<BookTypeEditorViewModel, Variant>();
                cfg.CreateMap<Variant, BookBasicEditorViewModel>();
                cfg.CreateMap<BookBasicEditorViewModel, Variant>();
                cfg.CreateMap<BookBasicEditorViewModel, Book>();
                cfg.CreateMap<ShelfEditorViewModel, Shelf>();
                cfg.CreateMap<Shelf, ShelfEditorViewModel>();
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper); 
            
            services.AddMvc(config => {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });            
           
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
                app.UseSeed();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");                
            }
            
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCustomizedMvc();
            
        }
    }
}
