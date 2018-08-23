using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroBee.Data
{
	public interface IMicroItemService
	{
		Task<MicroItem> GetMicroItemAsync(int id);
		Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize);
		Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize, string category);
		Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize, string category, string titleFilter);
		Task AddMicroItemAsync(MicroItem item);
		Task UpdateMicroItemAsync(MicroItem item);
		Task DeleteMicroItemAsync(int id);
	}
}
