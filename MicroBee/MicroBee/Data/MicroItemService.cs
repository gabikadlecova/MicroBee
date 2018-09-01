using System.Collections.Generic;
using System.Threading.Tasks;
using MicroBee.Data.Models;

namespace MicroBee.Data
{
	class MicroItemService : IMicroItemService
	{
		private HttpService Service { get; }

		public MicroItemService(HttpService service)
		{
			Service = service;
		}

		public async Task<MicroItem> GetMicroItemAsync(int id)
		{
			return await Service.GetAsync<MicroItem>("api/items/detail/" + id);
		}

		public async Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize)
		{
			List<KeyValuePair<string, object> > parameters = new List<KeyValuePair<string, object>>()
			{
				new KeyValuePair<string, object>("pageNumber", pageNumber),
				new KeyValuePair<string, object>("pageSize", pageSize)
			};

			return await Service.GetAsync<List<MicroItem>>("api/items/", parameters);
		}

		public async Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize, string category)
		{
			List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>()
			{
				new KeyValuePair<string, object>("pageNumber", pageNumber),
				new KeyValuePair<string, object>("pageSize", pageSize)
			};

			return await Service.GetAsync<List<MicroItem>>("api/items/" + category, parameters);
		}

		public async Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize, string category, string titleFilter)
		{
			List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>()
			{
				new KeyValuePair<string, object>("pageNumber", pageNumber),
				new KeyValuePair<string, object>("pageSize", pageSize),
				new KeyValuePair<string, object>("titlefilter", titleFilter)
			};

			return await Service.GetAsync<List<MicroItem>>("api/items/" + category, parameters);
		}
		

		public async Task AddMicroItemAsync(MicroItem item, byte[] image = null)
		{
			// result id needed in order to assign image
			MicroItem resultItem = await Service.PostAsync<MicroItem, MicroItem>("api/items/", item, authorize: true);
			if (image != null)
			{
				await Service.PostAsync("api/items/image/" + resultItem.Id, image, authorize: true);
			}
		}

		public async Task UpdateMicroItemAsync(MicroItem item, byte[] image = null)
		{
			await Service.PutAsync("api/items/", item, authorize: true);
			if (image != null)
			{
				await Service.PostAsync("api/items/image/" + item.Id, image, authorize: true);
			}
		}

		public async Task DeleteMicroItemAsync(int id)
		{
			await Service.DeleteAsync("api/items/", id, authorize: true);
		}

		public async Task SetAsWorkerAsync(int itemId)
		{
			await Service.PostAsync("api/items/worker/", itemId, authorize: true);
		}

		public async Task<List<ItemCategory>> GetCategoriesAsync()
		{
			return await Service.GetAsync<List<ItemCategory>>("api/items/categories/");
		}

		public async Task<byte[]> GetImageAsync(int id)
		{
			return await Service.GetByteArrayAsync("api/items/image/" + id);
		}
		
	}
}
