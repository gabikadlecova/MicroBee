using System.Collections.Generic;
using System.Threading.Tasks;
using MicroBee.Data.Models;

namespace MicroBee.Data
{
	public interface IMicroItemService
	{
		Task<MicroItem> GetMicroItemAsync(int id);
		Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize);
		Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize, string category);
		Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize, string category, string titleFilter);
		Task AddMicroItemAsync(MicroItem item, byte[] image);
		Task UpdateMicroItemAsync(MicroItem item, byte[] image);
		Task DeleteMicroItemAsync(int id);
		Task SetAsWorkerAsync(int itemId);

		Task<List<ItemCategory>> GetCategoriesAsync();
		Task<byte[]> GetImageAsync(int id);
	}
}
