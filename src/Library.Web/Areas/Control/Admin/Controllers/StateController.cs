using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Models.BootstrapModels;
using Library.Web.Models.DashboardViewModels;
using Library.Web.Models.StateViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Areas.Control.Admin.Controllers
{
    [Area(SiteAreas.Admin)]
    [Authorize(Policy="AdministratorOnly")]
    public class StateController: Controller {
        LibraryDbContext _context;
        private readonly IMapper _mapper;
        
        public StateController(LibraryDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(){
            var stateListing = new StateListingViewModel();
            stateListing.States = await _context.States.OrderBy(s => s.Name).ToListAsync();
            return View(stateListing);
        }
        public async Task<IActionResult> Create() {
            var model = new StateEditorViewModel() { 
                EditorAttributes = new EditorAttributes { ActionUrl = "Create", Caption = "Create State"} 
            };
            await PopulateDropdowns(model);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(StateEditorViewModel model) {
            if(!ModelState.IsValid)
            {
                model.EditorAttributes = new EditorAttributes { ActionUrl = "Create", Caption = "Create State"};
                await PopulateDropdowns(model);
                return View(model);
            } 

            var state = _mapper.Map<StateEditorViewModel, State>(model);
            var vm = _mapper.Map<State, StateEditorViewModel>(state);
            _context.States.Add(state);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(LocationController.Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var state = _context.States.Find(id);
            if(state == null){
                return NotFound();
            }
            var model = _mapper.Map<State, StateEditorViewModel>(state);
            model.EditorAttributes = new EditorAttributes { ActionUrl = "Edit", Caption = "Edit State"};
            await PopulateDropdowns(model);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, StateEditorViewModel model) {
            var state = _context.States.AsNoTracking().Where(l => l.Id == id).FirstOrDefault();
            if(state == null)
            {
                return NotFound();
            };
            if(!ModelState.IsValid)
            {
                model.EditorAttributes = new EditorAttributes { ActionUrl = "Edit", Caption = "Edit Location"};
                await PopulateDropdowns(model);
                return View(model);
            }
            var result = _mapper.Map<StateEditorViewModel, State>(model);            
            _context.Attach(result);
            _context.Entry(result).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(StateController.Index));
        }

        private async Task PopulateDropdowns(StateEditorViewModel model)
        {
           model.Countries = await _context.Countries.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync();                       
        }



    }
}