using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Extensions;
using Library.Web.Models.ShelfViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Areas.Admin.Controllers
{
    [Area(SiteAreas.Admin)]
    [Authorize(Policy = "AdministratorOnly")]
    [Route("Admin/Shelf")]
    public class ShelfController: Controller 
    {
        private LibraryDbContext _context;
        private IMapper _mapper;

        public ShelfController(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        [HttpGet("")]
        public async Task<IActionResult> Index(int? locationId)
        {
            var query = from shelves in _context.Shelves.Include(s=>s.Location).OrderBy(s => s.Location.Name).OrderBy(s=>s.Name)
                        select shelves;

            var listing = new ShelfListingViewModel();
            if(locationId.HasValue)
            {
                query = query.Where(s=>s.LocationId == locationId);
            }
            listing.Shelves = await query.ToListAsync();
            return View(listing);
        }
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var model = new ShelfEditorViewModel();
            await PopulateDropdownAsync(model);
            return View(model);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ShelfEditorViewModel model)
        {
            if(!ModelState.IsValid)
            {
                await PopulateDropdownAsync(model);
                return View(model);
            }
            var shelf = _mapper.Map<Shelf>(model);
            _context.Shelves.Add(shelf);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShelfController.Index));
        }
        [HttpGet("{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var shelf = await _context.Shelves.FirstOrDefaultAsync(s=>s.Id == id);
            if(shelf == null)
            {
                return NotFound();
            }
            var model = new ShelfEditorViewModel();
            model = _mapper.Map<Shelf,ShelfEditorViewModel>(shelf);
            await PopulateDropdownAsync(model);
            return View(model);
        }
        [HttpPost("{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id, ShelfEditorViewModel model)
        {
            if(!ModelState.IsValid)
            {
                await PopulateDropdownAsync(model);
                return View(model);
            }
            var shelf = await _context.Shelves.FindAsync(id);
            if(await TryUpdateModelAsync<Shelf>(shelf, string.Empty, s=>s.Name,s=>s.LocationId, s=>s.Description)){
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ShelfController.Index));
        }


        private async Task PopulateDropdownAsync(ShelfEditorViewModel model)
        {
            model.Locations = await _context.Locations.OrderBy(l=>l.Name).ToListAsync();
        }
    }
}