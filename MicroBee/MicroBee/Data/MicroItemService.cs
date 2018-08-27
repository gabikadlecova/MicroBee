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

		private HttpService Service { get; }

		public MicroItemService(string hostName, HttpService service)
		{
			HostName = hostName;
			Service = service;
		}

		public async Task<MicroItem> GetMicroItemAsync(int id)
		{
			return await Service.GetAsync<MicroItem>("api/items/" + id, null);
		}

		public async Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize)
		{
			List<KeyValuePair<string, object> > parameters = new List<KeyValuePair<string, object>>()
			{
				new KeyValuePair<string, object>("pageNumber", pageNumber),
				new KeyValuePair<string, object>("pageSize", pageSize)
			};

			return await Service.GetAsync<List<MicroItem>>(HostName + "api/items/", parameters);
		}

		public async Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize, string category)
		{
			List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>()
			{
				new KeyValuePair<string, object>("pageNumber", pageNumber),
				new KeyValuePair<string, object>("pageSize", pageSize),
				new KeyValuePair<string, object>("category", category)
			};

			return await Service.GetAsync<List<MicroItem>>(HostName + "api/items/", parameters);
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
		
		//todo categories
	}
}
