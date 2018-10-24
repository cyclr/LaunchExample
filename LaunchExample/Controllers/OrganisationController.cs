using Cyclr.LaunchExample.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cyclr.LaunchExample.Controllers
{
    [Authorize]
    public class OrganisationController : Controller
    {
        private readonly LaunchExampleContext _context = new LaunchExampleContext();

        // GET: Contact
        public ActionResult Index()
        {
            return View(_context.Organisations.ToList());
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View(new Organisation());
        }

        [HttpPost]
        public async Task<ActionResult> Add(Organisation model)
        {
            if(string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "Required");
                return View(model);
            }

            _context.Organisations.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var organisation = await _context.Organisations.FindAsync(id);
            if (organisation != null)
            {
                _context.Organisations.Remove(organisation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var organisation = await _context.Organisations.FindAsync(id);
            return View(organisation);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, Organisation model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "Required");
                return View(model);
            }

            var organisation = await _context.Organisations.FindAsync(id);
            organisation.Address = model.Address;
            organisation.Email = model.Email;
            organisation.Name = model.Name;
            organisation.Phone = model.Phone;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}