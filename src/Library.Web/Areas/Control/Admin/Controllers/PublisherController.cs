using System.Linq;
using System.Threading.Tasks;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Extensions;
using Library.Web.Models.PublisherViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index() {
            var publisherListing = new PublisherListingViewModel();
            publisherListing.Publishers = publisherService.GetAllPublishers().OrderBy(p => p.Id).OrderBy(p => p.Name).MapToPublisherViewModel().ToList();
            return View(publisherListing);
        }

        [HttpGet]
        public IActionResult Create() {
            var model = new CreateEditPublisherViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEditPublisherViewModel model) {
            
            if(!ModelState.IsValid)  return View(model);
            _context.Publishers.Add(model.MapToPublisher());
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PublisherController.Index));
           
        }
        [HttpGet]
        public IActionResult Edit(int id) {
            var publisher = _context.Publishers.Find(id);
            if(publisher == null) return NotFound();
            return View(publisher.MapToCreateEditPublisherViewModel());
        }
        [HttpPost]
        public IActionResult Edit(int id, CreateEditPublisherViewModel model) {
             var publisher = _context.Publishers.AsNoTracking().SingleOrDefault(p => p.Id == id);
             if(publisher == null) return NotFound();
             publisher = model.MapToPublisher();
            _context.Entry(publisher).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction(nameof(PublisherController.Index));
            
        }
    }
}