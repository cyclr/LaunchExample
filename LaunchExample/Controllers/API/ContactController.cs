using Cyclr.LaunchExample.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cyclr.LaunchExample.Controllers.API
{
    [BasicAuthentication]
    public class ContactController : ApiController
    {
        private readonly LaunchExampleContext _context = new LaunchExampleContext();

        public IEnumerable<Contact> Get([FromUri]int page = 1, [FromUri]int pageSize = 10)
        {
            return _context.Contacts.OrderBy(o => o.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<Contact> Get(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }
        
        public async Task<Contact> Post([FromBody]Contact model)
        {
            if(model.Organisation?.Id != null)
            {
                var organization = await _context.Organisations.FindAsync(model.Organisation.Id);
                model.Organisation = organization ?? throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Organisation not found") });
            }

            _context.Contacts.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }
        
        public async Task<Contact> Put(int id, [FromBody]Contact model)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            contact.EmailAddress = model.EmailAddress;
            contact.FirstName = model.FirstName;
            contact.LastName = model.LastName;
            contact.PhoneNumber = model.PhoneNumber;

            if (model.Organisation == null)
                contact.Organisation = null;
            else if(model.Organisation.Id != contact.Organisation?.Id)
            {
                var organization = await _context.Organisations.FindAsync(model.Organisation.Id);
                contact.Organisation = organization ?? throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Organisation not found") });
            }

            await _context.SaveChangesAsync();
            return contact;
        }
        
        public async Task Delete(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }
    }
}