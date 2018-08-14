using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MicroBeeWeb.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        // GET api/items
        [HttpGet]
        public IEnumerable<string> Get()
		{
			throw new NotImplementedException();
		}

        // GET api/items/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
			throw new NotImplementedException();
        }

        // POST api/items
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/items/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/items/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
