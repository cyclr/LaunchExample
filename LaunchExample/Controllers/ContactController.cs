using Cyclr.LaunchExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cyclr.LaunchExample.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly LaunchExampleContext _context = new LaunchExampleContext();

        // GET: Contact
        public ActionResult Index()
        {
            return View(_context.Contacts.ToList());
        }

        [HttpGet]
        public ActionResult Add()
        {
            var model = new AddOrUpdateContactModel
            {
                Contact = new Contact(),
                Organisations = _context.Organisations.Select(o => new SelectListItem { Text = o.Name, Value = o.Id.ToString() }).OrderBy(o => o.Text)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddOrUpdateContactModel model)
        {
            if (AddOrUpdateHasError(model, -1))
                return View(model);

            model.Contact.Organisation = await _context.Organisations.FindAsync(model.Contact.Organisation.Id);
            _context.Contacts.Add(model.Contact);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var model = new AddOrUpdateContactModel
            {
                Contact = await _context.Contacts.FindAsync(id),
                Organisations = _context.Organisations.Select(o => new SelectListItem { Text = o.Name, Value = o.Id.ToString() }).OrderBy(o => o.Text)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, AddOrUpdateContactModel model)
        {
            if (AddOrUpdateHasError(model, id))
                return View(model);

            var contact = await _context.Contacts.FindAsync(id);
            contact.EmailAddress = model.Contact.EmailAddress;
            contact.FirstName = model.Contact.FirstName;
            contact.LastName = model.Contact.LastName;
            contact.PhoneNumber = model.Contact.PhoneNumber;
            if(contact.Organisation.Id != model.Contact.Organisation.Id)
                contact.Organisation = await _context.Organisations.FindAsync(model.Contact.Organisation.Id);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AddOrUpdateHasError(AddOrUpdateContactModel model, int contactId)
        {
            if (string.IsNullOrEmpty(model.Contact.EmailAddress))
            {
                ModelState.AddModelError("Contact.EmailAddress", "Required");
                model.Organisations = _context.Organisations.Select(o => new SelectListItem { Text = o.Name, Value = o.Id.ToString() }).OrderBy(o => o.Text);
                return true;
            }

            if (_context.Contacts.Any(c => c.Id != contactId && c.EmailAddress == model.Contact.EmailAddress))
            {
                ModelState.AddModelError("Contact.EmailAddress", "Email address already in use");
                model.Organisations = _context.Organisations.Select(o => new SelectListItem { Text = o.Name, Value = o.Id.ToString() }).OrderBy(o => o.Text);
                return true;
            }

            return false;
        }
    }
}