using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Joney.WebAPI.Controllers
{
    public class JProController : ApiController
    {
        // GET: api/JPro
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/JPro/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/JPro
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/JPro/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/JPro/5
        public void Delete(int id)
        {
        }
    }
}
