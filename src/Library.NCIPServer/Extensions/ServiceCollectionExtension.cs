using Library.Repo;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Library.NCIPServer.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void UseCustomizedDataSource(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<LibraryDbContext>(options => { options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")); });
        }
        public static void UseCustomizedMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.InputFormatters.Add(new XmlSerializerInputFormatter());
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            }).AddXmlDataContractSerializerFormatters();
        }
    }
}