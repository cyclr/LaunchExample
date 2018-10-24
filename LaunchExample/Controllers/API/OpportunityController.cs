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
    public class OpportunityController : ApiController
    {
        private readonly LaunchExampleContext _context = new LaunchExampleContext();

        public IEnumerable<Opportunity> Get([FromUri]int page = 1, [FromUri]int pageSize = 10, int? contactId = null)
        {
            return _context.Opportunities.Where(o => contactId == null || o.Contact.Id == contactId).OrderBy(o => o.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<Opportunity> Get(int id)
        {
            return await _context.Opportunities.FindAsync(id);
        }
        
        public async Task<Opportunity> Post([FromBody]Opportunity model)
        {
            if(model.Contact?.Id != null)
            {
                var contact = await _context.Contacts.FindAsync(model.Contact.Id);
                model.Contact = contact ?? throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Contact not found") });
            }

            _context.Opportunities.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }
        
        public async Task<Opportunity> Put(int id, [FromBody]Opportunity model)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            opportunity.Description = model.Description;
            opportunity.DueDate = model.DueDate;
            opportunity.Name = model.Name;
            opportunity.Status = model.Status;
            opportunity.Value = model.Value;

            if (model.Contact == null)
                opportunity.Contact = null;
            else if(model.Contact.Id != opportunity.Contact?.Id)
            {
                var contact = await _context.Contacts.FindAsync(model.Contact.Id);
                opportunity.Contact = contact ?? throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Contact not found") });
            }

            await _context.SaveChangesAsync();
            return opportunity;
        }
        
        public async Task Delete(int id)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Opportunities.Remove(opportunity);
            await _context.SaveChangesAsync();
        }
    }
}