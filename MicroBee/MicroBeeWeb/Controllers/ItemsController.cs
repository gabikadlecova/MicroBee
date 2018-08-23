using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MicroBee.Web.DAL.Entities;
using MicroBee.Web.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;


namespace MicroBee.Web.Controllers
{
	[Route("api/[controller]")]
    public class ItemsController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMicroItemService _itemService;
		public ItemsController(IMicroItemService itemService, UserManager<ApplicationUser> userManager)
		{
			_itemService = itemService;
			_userManager = userManager;
		}


        // GET api/items
		
        [HttpGet]
        public ActionResult<List<MicroItem>> Get(int pageNumber, int pageSize)
		{
			// returns only items which have no worker user assigned
			return _itemService.GetOpenItems(pageNumber, pageSize).ToList();
		}

        // GET api/items/id
		
        [HttpGet("{id}")]
        public async Task<ActionResult<MicroItem>> Get(int id)
        {
			return await _itemService.FindItemAsync(id);
        }

		// GET api/items/category
		[HttpGet("{category}")]
		public ActionResult<List<MicroItem>> Get([FromQuery]string category, int pageNumber, int pageSize)
		{
			return _itemService.GetOpenItems(pageNumber, pageSize, category).ToList();
		}

		// POST api/items
		[HttpPost]
		[Authorize]
        public async Task<IActionResult> Post([FromBody]MicroItem model)
        {
	        string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
	        if (id == null)
	        {
		        return Unauthorized();
	        }

	        var user = await _userManager.FindByIdAsync(id);
	        model.OwnerName = user.UserName;
	        model.WorkerName = null;

			// todo upload image

			await _itemService.InsertItemAsync(model);
	        return Ok();
        }

        // PUT api/items/id
        [HttpPut("{id}")]
		[Authorize]
        public async Task<IActionResult> Put(int id, [FromBody]MicroItem model)
        {
	        if (id != model.Id)
	        {
		        return BadRequest();
	        }

	        MicroItem oldItem = await _itemService.FindItemAsync(id);

			// assigning to properties which are allowed to be modified
	        oldItem.Category = model.Category;
	        oldItem.Description = model.Description;
	        oldItem.Price = model.Price;
	        oldItem.Title = model.Title;

			await _itemService.UpdateItemAsync(oldItem);
	        return NoContent();
        }

        // DELETE api/items/id
        [HttpDelete("{id}")]
		[Authorize]
        public async Task<IActionResult> Delete(int id)
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				return Unauthorized();
			}

			var userTask = _userManager.FindByIdAsync(userId);
			var item = await _itemService.FindItemAsync(id);

			if ((await userTask).UserName != item.OwnerName)
			{
				return Unauthorized();
			}

			await _itemService.DeleteItemAsync(id);
			return Ok("Item was deleted.");
		}

    }
}
