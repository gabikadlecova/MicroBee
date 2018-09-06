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
	/// <summary>
	/// Provides item bound API
	/// </summary>
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
		public ActionResult<List<MicroItem>> Get(int pageNumber, int pageSize, string title)
		{
			//todo to be redone (filtering)

			// does not apply search filter
			if (string.IsNullOrEmpty(title))
			{
				return _itemService.GetOpenItems(pageNumber, pageSize).ToList();
			}

			// returns only items which have no worker user assigned
			return _itemService.GetOpenItems(pageNumber, pageSize, null, title).ToList();
		}

		// GET api/items/detail/itemId
		[HttpGet("detail/{itemId}")]
		public async Task<ActionResult<MicroItem>> Get(int itemId)
		{
			var item = await _itemService.FindItemAsync(itemId);

			if (item == null)
			{
				return NotFound();
			}

			return Ok(item);
		}

		// GET api/items/category
		[HttpGet("{category}")]
		public ActionResult<List<MicroItem>> Get(string category, int pageNumber, int pageSize, string title)
		{
			//TODO to be redone (filtering)

			//gets items from a specified category with or without a search filter applied
			return string.IsNullOrEmpty(title)
				? _itemService.GetOpenItems(pageNumber, pageSize, category).ToList() 
				: _itemService.GetOpenItems(pageNumber, pageSize, category, title).ToList();
		}

		// POST api/items
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Post([FromBody]MicroItem model)
		{
			//only authenticated users can post an item
			string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (id == null)
			{
				return Unauthorized();
			}

			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			//authorized user is the owner of the item
			var user = await _userManager.FindByIdAsync(id);
			model.OwnerName = user.UserName;
			model.WorkerName = null;

			//item added
			var item = await _itemService.InsertItemAsync(model);
			if (item == null)
			{
				return BadRequest();
			}

			return Ok(item);
		}

		// PUT api/items
		[HttpPut]
		[Authorize]
		public async Task<IActionResult> Put([FromBody]MicroItem model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			//user must be authorized and the owner of the item
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				return Unauthorized();
			}

			if (!await IsUserOwnerAsync(userId, model.Id))
			{
				return Forbid();
			}

			//item to be updated
			MicroItem oldItem = await _itemService.FindItemAsync(model.Id);

			// assigning to properties which are allowed to be modified
			oldItem.Category = model.Category;
			oldItem.Description = model.Description;
			oldItem.Price = model.Price;
			oldItem.Title = model.Title;

			//update
			var item = await _itemService.UpdateItemAsync(oldItem);
			if (item == null)
			{
				return BadRequest();
			}

			return NoContent();
		}

		// PUT api/items/worker
		[HttpPost("worker")]
		[Authorize]
		public async Task<IActionResult> Worker([FromBody]int itemId)
		{
			//anonymous workers are not allowed
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				return Unauthorized();
			}

			var user = await _userManager.FindByIdAsync(userId);

			// owner cannot be a worker!
			bool isOwner = await IsUserOwnerAsync(userId, itemId);
			if (isOwner)
			{
				return BadRequest();
			}

			//item must be updated
			MicroItem oldItem = await _itemService.FindItemAsync(itemId);
			oldItem.WorkerName = user.UserName;
			oldItem.Status = ItemStatus.Accepted;

			var item = await _itemService.UpdateItemAsync(oldItem);
			if (item == null)
			{
				return BadRequest();
			}
			
			return Ok();
		}

		// DELETE api/items/itemId
		[HttpDelete("{itemId}")]
		[Authorize]
		public async Task<IActionResult> Delete(int itemId)
		{
			//only owners can delete items
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				return Unauthorized();
			}

			bool isOwner = await IsUserOwnerAsync(userId, itemId);
			if (!isOwner)
			{
				return Forbid();
			}

			//item image and item are both deleted
			await DeleteImage(itemId);
			await _itemService.DeleteItemAsync(itemId);

			return Ok();
		}

		[HttpGet("categories")]
		public ActionResult<List<ItemCategory>> GetCategories()
		{
			//gets all categories
			return _categoryService.GetCategories().ToList();
		}

		[HttpGet("image/{imageId}")]
		public async Task<IActionResult> Image(int imageId)
		{
			//gets an image with a specified id
			var image = await _imageService.FindImageAsync(imageId);
			if (image == null)
			{
				return NotFound();
			}

			return File(image.Data, "image/png");
		}

		[HttpPost("image/{itemId}")]
		[Authorize]
		public async Task<IActionResult> PostImage(int itemId, [FromBody]byte[] imageData)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			//only user owner can update the image of an item
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

			//ad or update item image
			var item = await _itemService.FindItemAsync(itemId);
			if (item.ImageId == null)
			{
				//add
				var uploaded = await _imageService.InsertImageAsync(new ItemImage() { Data = imageData });
				if (uploaded != null)
				{
					item.ImageId = uploaded.Id;
					await _itemService.UpdateItemAsync(item);

					return Ok();
				}
			}
			else
			{
				//update
				ItemImage image = new ItemImage()
				{
					Id = item.ImageId.Value,
					Data = imageData
				};
				var updated = await _imageService.UpdateImageAsync(image);
				if (updated != null)
				{
					return Ok();
				}
			}

			return BadRequest();
		}

		// DELETE api/items/image/itemId
		[HttpDelete("image/{itemId}")]
		[Authorize]
		public async Task<IActionResult> DeleteImage(int itemId)
		{
			//only owner can delete the image
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				return Unauthorized();
			}

			bool isOwner = await IsUserOwnerAsync(userId, itemId);
			if (!isOwner)
			{
				return Forbid();
			}

			//delete image
			var item = await _itemService.FindItemAsync(itemId);
			if (item.ImageId == null)
			{
				return BadRequest();
			}

			var deleteTask = _imageService.DeleteImageAsync(item.ImageId.Value);
			item.ImageId = null;

			//item has no image id associated with it now
			await _itemService.UpdateItemAsync(item);
			await deleteTask;

			return Ok();
		}

		private async Task<bool> IsUserOwnerAsync(string userId, int itemId)
		{
			//checks whether Username and OwnerName match
			var userTask = _userManager.FindByIdAsync(userId);
			var item = await _itemService.FindItemAsync(itemId);

			string userName = (await userTask)?.UserName;

			return userName == item?.OwnerName;
		}
	}
}
