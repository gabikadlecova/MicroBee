using System;
using System.Collections.Generic;
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
			var result = await _client.GetStringAsync(_hostName + "/api/items/" + id);

			return JsonConvert.DeserializeObject<MicroItem>(result);
		}

		public async Task<IEnumerable<MicroItem>> GetMicroItemsAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<MicroItem>> GetMicroItemsAsync(string category)
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
