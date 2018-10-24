using Cyclr.LaunchExample.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cyclr.LaunchExample.Controllers.API
{
    [BasicAuthentication]
    public class OrganisationController : ApiController
    {
        private readonly LaunchExampleContext _context = new LaunchExampleContext();

        public IEnumerable<Organisation> Get([FromUri]int page = 1, [FromUri]int pageSize = 10)
        {
            return _context.Organisations.OrderBy(o => o.Id).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<Organisation> Get(int id)
        {
            return await _context.Organisations.FindAsync(id);
        }
        
        public async Task<Organisation> Post([FromBody]Organisation model)
        {
            _context.Organisations.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }
        
        public async Task<Organisation> Put(int id, [FromBody]Organisation model)
        {
            var org = await _context.Organisations.FindAsync(id);
            if (org == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            org.Address = model.Address;
            org.Email = model.Email;
            org.Name = model.Name;
            org.Phone = model.Phone;
            await _context.SaveChangesAsync();
            return org;
        }
        
        public async Task Delete(int id)
        {
            var org = await _context.Organisations.FindAsync(id);
            if (org == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Organisations.Remove(org);
            await _context.SaveChangesAsync();
        }
    }
}