using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MicroBee.Web.DAL.Entities;
using MicroBee.Web.Services;
using Microsoft.AspNetCore.Authorization;


namespace MicroBee.Web.Controllers
{
	[Route("api/[controller]")]
    public class ItemsController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMicroItemService _itemService;
		private readonly IMicroImageService _imageService;
		private readonly ICategoryService _categoryService;
		public ItemsController(IMicroItemService itemService, IMicroImageService imageService, ICategoryService categoryService, UserManager<ApplicationUser> userManager)
		{
			_itemService = itemService;
			_userManager = userManager;

			_imageService = imageService;
			_categoryService = categoryService;
		}


        // GET api/items
		
        [HttpGet]
        public ActionResult<List<MicroItem>> Get(int pageNumber, int pageSize)
		{
			// returns only items which have no worker user assigned
			return _itemService.GetOpenItems(pageNumber, pageSize).ToList();
		}

        // GET api/items/id
		
        [HttpGet("detail/{id}")]
        public async Task<ActionResult<MicroItem>> Get(int id)
        {
			var item = await _itemService.FindItemAsync(id);

	        if (item == null)
	        {
		        return NotFound();
	        }

	        return Ok(item);
        }

		// GET api/items/category
		[HttpGet("{category}")]
		public ActionResult<List<MicroItem>> Get(string category, int pageNumber, int pageSize)
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

	        if (!ModelState.IsValid)
	        {
		        return BadRequest();
	        }

	        var user = await _userManager.FindByIdAsync(id);
	        model.OwnerName = user.UserName;
	        model.WorkerName = null;

			var item = await _itemService.InsertItemAsync(model);
	        if (item == null)
	        {
		        return BadRequest();
	        }

	        return Ok();
        }

        // PUT api/items/id
        [HttpPut]
		[Authorize]
        public async Task<IActionResult> Put([FromBody]MicroItem model)
        {
	        if (!ModelState.IsValid)
	        {
		        return BadRequest();
	        }
			

	        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
	        if (userId == null)
	        {
		        return Unauthorized();
	        }

	        if (!await IsUserOwnerAsync(userId, model.Id))
	        {
		        return Forbid();
	        }

			MicroItem oldItem = await _itemService.FindItemAsync(model.Id);

			// assigning to properties which are allowed to be modified
	        oldItem.Category = model.Category;
	        oldItem.Description = model.Description;
	        oldItem.Price = model.Price;
	        oldItem.Title = model.Title;

			var item = await _itemService.UpdateItemAsync(oldItem);
	        if (item == null)
	        {
		        return BadRequest();
	        }

	        return NoContent();
        }

		// PUT api/items/id
		[HttpPost("worker")]
		[Authorize]
		public async Task<IActionResult> Worker(int id)
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				return Unauthorized();
			}

			var user = await _userManager.FindByIdAsync(userId);

			// owner cannot be worker!
			bool isOwner = await IsUserOwnerAsync(userId, id);
			if (isOwner)
			{
				return BadRequest();
			}

			MicroItem oldItem = await _itemService.FindItemAsync(id);
			oldItem.WorkerName = user.UserName;

			var item = await _itemService.UpdateItemAsync(oldItem);
			if (item == null)
			{
				return BadRequest();
			}

			return Ok();
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

			bool isOwner = await IsUserOwnerAsync(userId, id);
			if (!isOwner)
			{
				return Forbid();
			}

			
			await _itemService.DeleteItemAsync(id);
			return Ok("Item was deleted.");
		}

		[HttpGet("categories")]
		public ActionResult<List<ItemCategory>> GetCategories()
		{
			return _categoryService.GetCategories().ToList();
		}

		[HttpGet("image/{imageId}")]
		public async Task<IActionResult> Image(int imageId)
		{
			var item = await _imageService.FindImageAsync(imageId);
			if (item == null)
			{
				return NotFound();
			}

			return Ok(item);
;		}

		[HttpPost("detail/image/{itemId}")]
		[Authorize]
		public async Task<IActionResult> UpdateImage(int itemId, [FromBody]ItemImage image)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				return Unauthorized();
			}

			bool owner = await IsUserOwnerAsync(userId, itemId);
			if (!owner)
			{
				return Forbid();
			}

			var item = await _itemService.FindItemAsync(itemId);

			if (item.ImageAddress == null)
			{
				var uploaded = await _imageService.InsertImageAsync(image);
				if (uploaded != null)
				{
					return CreatedAtAction("Image", uploaded.Id);
				}
			}
			else
			{
				var updated = await _imageService.UpdateImageAsync(image);
				if (updated != null)
				{
					return Ok();
				}
			}

			return BadRequest();
		}


		private async Task<bool> IsUserOwnerAsync(string userId, int itemId)
		{
			var userTask = _userManager.FindByIdAsync(userId);
			var item = await _itemService.FindItemAsync(itemId);

			string userName = (await userTask)?.UserName;

			return userName == item?.OwnerName;
		}
    }
}
