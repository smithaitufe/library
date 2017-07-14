using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Repo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Library.NCIPServer
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
            services.AddEntityFrameworkNpgsql()
            .AddDbContext<LibraryDbContext>(options => { options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")); });
            services.AddMvc(options =>
            {
                // To add XmlSerializer based Input and Output formatters.
                options.InputFormatters.Add(new XmlSerializerInputFormatter());
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            }).AddXmlDataContractSerializerFormatters();

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvc();
        }
    }

    
}
