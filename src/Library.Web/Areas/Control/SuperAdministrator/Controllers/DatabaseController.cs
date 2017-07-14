using System;
using System.Threading.Tasks;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Web.Areas.Control.SuperAdministrator {
    [Area(SiteAreas.SuperAdministrator)]
    [Authorize(Policy="SuperAdministratorOnly")]
    public class DatabaseController: Controller {
        private readonly LibraryDbContext _context;
        private readonly IServiceProvider _provider;               
        public DatabaseController(LibraryDbContext context, IServiceProvider provider){
            _context = context;     
            _provider = provider;                  
        }
        public IActionResult Index() {
            return View();
        }
        public async Task<IActionResult> Seed() {  
            using(var context = _provider.GetService<LibraryDbContext>()){
                await DbContextSeeder.PerformSeeding(context);
            }           
            return View();
        }

    }
}