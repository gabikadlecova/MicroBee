using System;
using System.Net;
using System.Collections.Generic;

using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Mvc;

using MicroBee.Web.Abstraction;
using MicroBee.Web.Models;


namespace MicroBee.Web.Controllers
{
	[Route("api/[controller]")]
    public class MicroItemsController : Controller
    {
		private readonly IMicroItemService _itemService;
		public MicroItemsController(IMicroItemService itemService)
		{
			_itemService = itemService;
		}


        // GET api/items
		
        [HttpGet]
        public IEnumerable<MicroItem> Get()
		{
			return _itemService.GetOpenItemsAsync();
		}

        // GET api/items/5
		
        [HttpGet("{id}")]
        public MicroItem Get(int id)
        {
			return _itemService.FindItemAsync(id);
        }

		// GET api/items/category
		[HttpGet("{category}")]
		public IEnumerable<MicroItem> Get(string category)
		{
			return _itemService.GetOpenItemsAsync(category);
		}

		// POST api/items
		[HttpPost]
        public void Post([FromBody]MicroItem value)
        {
			_itemService.InsertItemAsync(value);
        }

        // PUT api/items/5
        [HttpPut("{id}")]
        public async void Put([FromBody]MicroItem value)
        {
			await _itemService.UpdateItemAsync(value);
        }

        // DELETE api/items/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
			await _itemService.DeleteItemAsync(id);
        }
    }
}
