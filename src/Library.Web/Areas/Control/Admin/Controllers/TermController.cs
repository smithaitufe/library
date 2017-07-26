using System.Linq;
using System.Threading.Tasks;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Extensions;
using Library.Web.Models.StatusViewModels;
using Library.Web.Models.TermSetViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Areas.Control.Admin.Controllers
{
    [Area(SiteAreas.Admin)]
    [Authorize(Policy = "AdministratorOnly")]
    [Route("Admin/Settings")]
    public class TermController : Controller
    {
        LibraryDbContext _context;
        TermService termService;
        public TermController(LibraryDbContext context)
        {
            _context = context;
            termService = new TermService(context);
        }
        [HttpGet("")]
        public IActionResult Index(string name)
        {
            var model = new TermListingViewModel();
            if (!string.IsNullOrEmpty(name))
            {

            }
            PopulateDropdowns(model);
            return View(model);
        }
        [HttpPost("GetTerms")]
        public IActionResult GetTerms(int id)
        {
            var terms = termService.GetTermsBySet(id).MapToTermViewModel().ToList();
            var termListing = new TermListingViewModel() { TermSetId = id };
            termListing.Terms = terms;
            return PartialView("_Terms", termListing);
        }
        [HttpGet("{id:int}/Create")]
        public async Task<IActionResult> Create(int id)
        {
            var model = new TermEditorViewModel();
            var termSet = await _context.TermSets.FindAsync(id);
            if (termSet == null)
            {
                return NotFound();
            }
            model.TermSet = termSet;
            model.TermSetId = id;
            return View(model);
        }
        [HttpPost("{id:int}/Create")]
        public async Task<IActionResult> Create(int id, TermEditorViewModel model)
        {
            if(!ModelState.IsValid)
            {
                TempData["FlashMessage"] = null;
                var termSet = await _context.TermSets.FindAsync(id);
                model.TermSet = termSet;
                return View(model);
            }
            var term = new Term();
            term.Name = model.Name;
            term.TermSetId = model.TermSetId;
            _context.Terms.Add(term);
            await _context.SaveChangesAsync();
            TempData["FlashMessage"] = "Term Added Successfully";
            return RedirectToAction(nameof(TermController.Create),new{id = id});
        }


        // [HttpGet("AddEditTerm")]
        // public IActionResult AddEditTerm(int termSetId, int? termId)
        // {
        //     var model = new TermFormViewModel();
        //     model.TermSetId = termSetId;
        //     if (termId.HasValue)
        //     {
        //         var term = termService.GetAllTerms().SingleOrDefault(t => t.Id == termId.Value);
        //         model.TermId = term.Id;
        //         model.Name = term.Name;
        //         model.TermSetId = term.TermSetId;
        //     }
        //     return PartialView("_AddEditTerm", model);
        // }
        // [HttpPost("{termSetId:int}/Create")]
        // public IActionResult Create(int termSetId, int? termId, TermFormViewModel model)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         bool isNew = !(termId.HasValue && termId.Value != 0);
        //         var term = isNew ? new Term() : termService.GetTermById(termId.Value).SingleOrDefault();
        //         term.Name = model.Name;
        //         term.TermSetId = model.TermSetId;
        //         if (isNew)
        //         {
        //             _context.Add(term);
        //         }
        //         else
        //         {
        //             _context.Entry(term).State = EntityState.Modified;
        //         }
        //         _context.SaveChanges();
        //         return Json(new StatusViewModel { Successful = true });
        //     }
        //     return PartialView("_AddEditTerm", model);
        // }

        [HttpGet("{id:int}/Terms/{termId:int}/Edit")]
        public async Task<IActionResult> Edit(int id, int termId)
        {
            var model = new TermEditorViewModel();
            var term = await _context.Terms.Include(t=>t.TermSet).Where(t=>t.Id == termId && t.TermSetId == id).SingleOrDefaultAsync();

            if (term == null)
            {
                return NotFound();
            }
            model.TermId = term.Id;
            model.Name = term.Name;
            model.TermSet = term.TermSet;
            model.TermSetId = id;
            return View(model);
        }

        [HttpPost("{id:int}/Terms/{termId:int}/Edit")]
        public async Task<IActionResult> Edit(int id, int termId, TermEditorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);                
            }
            var term = await _context.Terms.SingleOrDefaultAsync(t=>t.Id == termId && t.TermSetId == id);
            if (await (TryUpdateModelAsync<Term>(term, string.Empty, t => t.Name, t => t.TermSetId)))
            {
                await _context.SaveChangesAsync();
            }
            TempData["FlashMessage"] = "Term Edited Successfully";
            return RedirectToAction(nameof(TermController.Index),new{id = id});
        }

        [HttpGet("{id:int}/Delete")]
        public IActionResult Delete(int id)
        {
            var model = termService.GetTermById(id).MapToTermViewModel().SingleOrDefault();
            return PartialView("_Delete", model);
        }
        [HttpPost("{id:int}/Delete")]
        [ActionName("Delete")]
        public IActionResult DeleteAction(int id, IFormCollection form)
        {
            termService.DeleteTerm(id);
            return Json(new StatusViewModel { Successful = true });
        }
        private void PopulateDropdowns(TermListingViewModel model)
        {
            model.TermSets = _context.TermSets.Select(ts => new SelectListItem { Value = ts.Id.ToString(), Text = ts.Name.ToUpper().Replace('-', ' ') }).ToList();
        }
    }
}