using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using OnepMini.OrmNhib.Initializer;

namespace OnepMini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworksController : ControllerBase
    {
        private readonly INHibernateInitializer _nHibernateInitializer;
        private readonly ISessionFactory _sessionFactory;

        public NetworksController(
            INHibernateInitializer nHibernateInitializer,
            ISessionFactory sessionFactory)
        {
            this._nHibernateInitializer = nHibernateInitializer ?? throw new ArgumentNullException(nameof(nHibernateInitializer));
            this._sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
        }
        // GET: api/Networks
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Networks/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Networks
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Networks/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
