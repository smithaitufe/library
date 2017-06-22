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
    [Authorize(Policy="AdministratorOnly")]
    public class TermController: Controller {
        LibraryDbContext _context;
        TermService termService;
        public TermController(LibraryDbContext context) {
            _context = context;
            termService = new TermService(context);
        }
        public IActionResult Index(string name) {
            var model = new TermListingViewModel();
            if(!string.IsNullOrEmpty(name)){

            }
            PopulateDropdowns(model);
            return View(model);
        }
        [HttpPost]
        public IActionResult GetTerms(int id) {
            var terms = termService.GetTermsBySet(id).MapToTermViewModel().ToList();
            var termListing = new TermListingViewModel(){ TermSetId = id };
            termListing.Terms = terms;
            return PartialView("_Terms", termListing);
        }
        [HttpGet]
        public IActionResult AddEditTerm(int termSetId, int? termId) {
            var model = new TermFormViewModel();
            model.TermSetId = termSetId;
            if(termId.HasValue) {
                var term = termService.GetAllTerms().SingleOrDefault(t => t.Id == termId.Value);
                model.TermId = term.Id;
                model.Name = term.Name;
                model.TermSetId = term.TermSetId;
            }
            return PartialView("_AddEditTerm", model);
        }
        [HttpPost]
        public IActionResult AddEditTerm(int termSetId, int? termId, TermFormViewModel model) {
            if(ModelState.IsValid) {
                bool isNew = !(termId.HasValue && termId.Value != 0);
                var term = isNew ? new Term() : termService.GetTermById(termId.Value).SingleOrDefault();
                term.Name = model.Name;
                term.TermSetId = model.TermSetId;
                if(isNew){
                    _context.Add(term);
                }else{
                    _context.Entry(term).State = EntityState.Modified;
                }                 
                _context.SaveChanges();
                return Json(new StatusViewModel{ Successful = true });
            }
            return PartialView("_AddEditTerm", model);
        }
        [HttpGet]
        public IActionResult Delete(int id) {
            var model = termService.GetTermById(id).MapToTermViewModel().SingleOrDefault();
            return PartialView("_Delete", model);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteAction(int id, IFormCollection form) {
            termService.DeleteTerm(id);
            return Json(new StatusViewModel { Successful = true});
        }
        private void PopulateDropdowns(TermListingViewModel model) {
            model.TermSets = _context.TermSets.Select(ts => new SelectListItem { Value = ts.Id.ToString(), Text = ts.Name.ToUpper().Replace('-', ' ') }).ToList();
        }
    }
}