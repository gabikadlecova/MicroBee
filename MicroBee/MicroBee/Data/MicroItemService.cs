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
		private HttpClient _client = new HttpClient();
		private string _hostName;
		public MicroItemService(string hostName)
		{
			_hostName = hostName;
		}

		public async Task<MicroItem> GetMicroItemAsync(int id)
		{
			var result = await _client.GetAsync(_hostName + "api/items/" + id);
			
			return JsonConvert.DeserializeObject<MicroItem>(result.Content.ToString());
		}

		public async Task<List<MicroItem>> GetMicroItemsAsync()
		{
			var response = await _client.GetAsync(_hostName + "api/items");

			if (response.IsSuccessStatusCode)
			{
				var items = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<List<MicroItem>>(items);
			}

			throw new InvalidOperationException("todo");
		}

		public async Task<List<MicroItem>> GetMicroItemsAsync(string category)
		{
			var result = await _client.GetAsync(_hostName + "api/items/" + category);

			return JsonConvert.DeserializeObject<List<MicroItem>>(result.Content.ToString());
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
