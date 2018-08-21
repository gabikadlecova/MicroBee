using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroBee.Data
{
	public interface IMicroItemService
	{
		Task<MicroItem> GetMicroItemAsync(int id);
		Task<IEnumerable<MicroItem>> GetMicroItemsAsync();
		Task<IEnumerable<MicroItem>> GetMicroItemsAsync(string category);
		Task AddMicroItemAsync(MicroItem item);
		Task UpdateMicroItemAsync(MicroItem item);
		Task DeleteMicroItemAsync(int id);
	}
}
