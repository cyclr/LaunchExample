using Cyclr.LaunchExample.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cyclr.LaunchExample.Controllers
{
    [Authorize]
    public class OpportunityController : Controller
    {
        private readonly LaunchExampleContext _context = new LaunchExampleContext();

        // GET: Contact
        public ActionResult Index()
        {
            return View(_context.Opportunities.ToList());
        }

        [HttpGet]
        public ActionResult Add()
        {
            var model = new AddOrUpdateOpportunity
            {
                Contacts = _context.Contacts.Select(c=>new SelectListItem { Text = c.EmailAddress, Value = c.Id.ToString() }),
                Opportunity = new Opportunity()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddOrUpdateOpportunity model)
        {
            if (AddOrEditHasError(model))
                return View(model);

            model.Opportunity.Contact = await _context.Contacts.FindAsync(model.Opportunity.Contact.Id);
            _context.Opportunities.Add(model.Opportunity);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity != null)
            {
                _context.Opportunities.Remove(opportunity);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var model = new AddOrUpdateOpportunity
            {
                Contacts = _context.Contacts.Select(c => new SelectListItem { Text = c.EmailAddress, Value = c.Id.ToString() }),
                Opportunity = await _context.Opportunities.FindAsync(id)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, AddOrUpdateOpportunity model)
        {
            if (AddOrEditHasError(model))
                return View(model);

            var opportunity = await _context.Opportunities.FindAsync(id);
            opportunity.Description = model.Opportunity.Description;
            opportunity.DueDate = model.Opportunity.DueDate;
            opportunity.Name = model.Opportunity.Name;
            opportunity.Status = model.Opportunity.Status;
            opportunity.Value = model.Opportunity.Value;
            if (opportunity.Contact.Id != model.Opportunity.Contact.Id)
                opportunity.Contact = await _context.Contacts.FindAsync(model.Opportunity.Contact.Id);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AddOrEditHasError(AddOrUpdateOpportunity model)
        {
            bool error = false;
            if (string.IsNullOrEmpty(model.Opportunity.Name))
            {
                ModelState.AddModelError("Opportunity.Name", "Required");
                error = true;
            }
            if (model.Opportunity.Contact == null)
            {
                ModelState.AddModelError("Opportunity.Contact.Id", "The Contact field is required.");
                error = true;
            }

            if(error)
                model.Contacts = _context.Contacts.Select(c => new SelectListItem { Text = c.EmailAddress, Value = c.Id.ToString() });

            return error;
        }
    }
}