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

		// GET api/items/itemId

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

			return Ok(item);
		}

		// PUT api/items/itemId
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

		// PUT api/items/worker/itemId
		[HttpPost("worker")]
		[Authorize]
		public async Task<IActionResult> Worker(int itemId)
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				return Unauthorized();
			}

			var user = await _userManager.FindByIdAsync(userId);

			// owner cannot be worker!
			bool isOwner = await IsUserOwnerAsync(userId, itemId);
			if (isOwner)
			{
				return BadRequest();
			}

			MicroItem oldItem = await _itemService.FindItemAsync(itemId);
			oldItem.WorkerName = user.UserName;

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

			await DeleteImage(itemId);
			await _itemService.DeleteItemAsync(itemId);
			return Ok();
		}

		[HttpGet("categories")]
		public ActionResult<List<ItemCategory>> GetCategories()
		{
			return _categoryService.GetCategories().ToList();
		}

		[HttpGet("image/{imageId}")]
		public async Task<IActionResult> Image(int imageId)
		{
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


			if (item.ImageId == null)
			{
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

			var item = await _itemService.FindItemAsync(itemId);
			if (item.ImageId == null)
			{
				return BadRequest();
			}

			var deleteTask = _imageService.DeleteImageAsync(item.ImageId.Value);
			item.ImageId = null;
			await _itemService.UpdateItemAsync(item);
			await deleteTask;

			return Ok();
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
