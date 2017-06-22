using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Models.BootstrapModels;
using Library.Web.Models.DashboardViewModels;
using Library.Web.Models.LocationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Areas.Control.Admin.Controllers
{
    [Area(SiteAreas.Admin)]
    [Authorize(Policy="AdministratorOnly")]
    public class LocationController: Controller {
        LibraryDbContext _context;
        private readonly IMapper _mapper;
        public LocationController(LibraryDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(){
            var locationListing = new LocationListingViewModel();
            locationListing.Locations = await _context.Locations.ToListAsync();
            return View(locationListing);
        }
        public IActionResult Create() {
            var model = new LocationEditorViewModel() 
            { 
                EditorAttributes = new EditorAttributes { ActionUrl = "Create", Caption = "Create Location"} 
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(LocationEditorViewModel model) {
            if(!ModelState.IsValid)
            {
                model.EditorAttributes = new EditorAttributes { ActionUrl = "Create", Caption = "Create Location"};
                return View(model);
            } 

            var location = _mapper.Map<LocationEditorViewModel, Location>(model);
            var vm = _mapper.Map<Location, LocationEditorViewModel>(location);
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(LocationController.Index));
        }
        [HttpGet]
        public IActionResult Edit(int id) {
            var location = _context.Locations.Find(id);
            if(location == null){
                return NotFound();
            }
            var model = _mapper.Map<Location, LocationEditorViewModel>(location);
            model.EditorAttributes = new EditorAttributes { ActionUrl = "Edit", Caption = "Edit Location"};
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, LocationEditorViewModel model) {
            var location = _context.Locations.AsNoTracking().Where(l => l.Id == id).FirstOrDefault();
            if(location == null)
            {
                return NotFound();
            };
            if(!ModelState.IsValid)
            {
                model.EditorAttributes = new EditorAttributes { ActionUrl = "Edit", Caption = "Edit Location"};
                return View(model);
            }
            var result = _mapper.Map<LocationEditorViewModel, Location>(model);            
            _context.Attach(result);
            _context.Entry(result).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(LocationController.Index));
        }



    }
}