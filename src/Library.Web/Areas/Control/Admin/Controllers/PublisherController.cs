using System.Linq;
using System.Threading.Tasks;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Extensions;
using Library.Web.Models.PublisherViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Areas.Admin.Controllers
{
    [Area(SiteAreas.Admin)]
    [Authorize(Policy="AdministratorOnly")]
    public class PublisherController: Controller
    {
        LibraryDbContext _context;
        PublisherService publisherService;
        public PublisherController(LibraryDbContext context) {
            _context = context;
            publisherService = new PublisherService(_context);
        }
        public async Task<IActionResult> Index() {
            
            var publishers = await publisherService.GetAllPublishers()            
            .AsNoTracking()            
            .OrderBy(p => p.Name)
            .ToListAsync();
            
            var listing = new PublisherListingViewModel() {Publishers = publishers};

            return View(listing);
        }

        [HttpGet]
        public async Task<IActionResult> Create() {
            var model = new PublisherEditorViewModel();
            await PopulateDropdown(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PublisherEditorViewModel model) {
            
            if(!ModelState.IsValid)
            {  
                await PopulateDropdown(model);
                return View(model);
            }
            var publisher = model.MapToPublisher();
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PublisherController.Index));
           
        }
        [HttpGet]
        public IActionResult Edit(int id) {
            var publisher = _context.Publishers.AsNoTracking().SingleOrDefault(p => p.Id == id);
            if(publisher == null) return NotFound();
            return View(publisher.MapToPublisherEditorViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, PublisherEditorViewModel model) {
             var publisher = _context.Publishers.SingleOrDefault(p => p.Id == id);
             if(publisher == null) return NotFound();

             if(await TryUpdateModelAsync<Publisher>(publisher, string.Empty, p=>p.Name, p=>p.Address, p=>p.PhoneNumber))
             {
                await _context.SaveChangesAsync();
             }
            //  publisher = model.MapToPublisher();
            // _context.Entry(publisher).State = EntityState.Modified;
            // _context.SaveChanges();
            return RedirectToAction(nameof(PublisherController.Index));
            
        }

        private async Task PopulateDropdown(PublisherEditorViewModel model)
        {
            model.Address.Countries = await _context.Countries.OrderBy(c=>c.Name).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToListAsync();
        }
    }
}