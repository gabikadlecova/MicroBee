using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MicroBee.Data
{
	class MicroItemService : IMicroItemService
	{
		private string HostName { get; }

		private HttpClient Client { get; }

		public MicroItemService(string hostName, HttpClient client)
		{
			HostName = hostName;
			Client = client;
		}

		public async Task<MicroItem> GetMicroItemAsync(int id)
		{
			var result = await Client.GetAsync(HostName + "api/items/" + id);
			
			return JsonConvert.DeserializeObject<MicroItem>(result.Content.ToString());
		}

		public async Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize)
		{
			var response = await Client.GetAsync(HostName + "api/items?" + "pageNumber=" + pageNumber + "&pageSize=" + pageSize);

			if (response.IsSuccessStatusCode)
			{
				var items = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<List<MicroItem>>(items);
			}

			throw new InvalidResponseException();
		}

		public async Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize, string category)
		{
			var response = await Client.GetAsync(HostName + "api/items/" + category + "?pageNumber=" + pageNumber + "&pageSize=" + pageSize);

			if (response.IsSuccessStatusCode)
			{
				var items = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<List<MicroItem>>(items);
			}

			throw new InvalidResponseException();
		}

		public async Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize, string category, string titleFilter)
		{
			throw new NotImplementedException();
		}

		public async Task AddMicroItemAsync(MicroItem item)
		{
			throw new NotImplementedException();
		}

		public async Task UpdateMicroItemAsync(MicroItem item)
		{
			throw new NotImplementedException();
		}

		public async Task DeleteMicroItemAsync(int id)
		{
			throw new NotImplementedException();
		}
		
	}
}
