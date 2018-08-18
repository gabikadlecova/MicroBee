using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using MicroBee.Web.Abstraction;
using MicroBee.Web.Models;


namespace MicroBee.Web.Controllers
{
	[Route("api/[controller]")]
    public class ItemsController : Controller
    {
		//TODO rename async methods! Better names
		//TODO IActionResults!!!


		private readonly IMicroItemService _itemService;
		public ItemsController(IMicroItemService itemService)
		{
			_itemService = itemService;
		}


        // GET api/items
		
        [HttpGet]
        public ActionResult<IEnumerable<MicroItem>> Get()
		{
			return _itemService.GetAllItems().ToList();
		}

        // GET api/items/5
		
        [HttpGet("{id}")]
        public async Task<ActionResult<MicroItem>> Get(int id)
        {
			return await _itemService.FindItemAsync(id);
        }

		// GET api/items/category
		[HttpGet("{category}")]
		public ActionResult<IEnumerable<MicroItem>> Get(string category)
		{
			return _itemService.GetOpenItems(category).ToList();
		}

		// POST api/items
		[HttpPost]
        public async Task Post([FromBody]MicroItem value)
        {
			await _itemService.InsertItemAsync(value);
        }

        // PUT api/items/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]MicroItem value)
        {
			await _itemService.UpdateItemAsync(value);
        }

        // DELETE api/items/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
			await _itemService.DeleteItemAsync(id);
        }

    }
}
