using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.Models;

namespace MicroBee.Web.Abstraction
{
    public interface IMicroItemService
    {
		Task<MicroItem> FindItemAsync(int id);
		Task<IEnumerable<MicroItem>> FindItemsAsync(string substr);
		Task<IEnumerable<MicroItem>> GetAllItemsAsync();
		Task<IEnumerable<MicroItem>> GetOpenItemsAsync();

		Task<IEnumerable<MicroItem>> GetOpenItemsAsync(string category);
		Task<MicroItem> InsertItemAsync(MicroItem item);
		Task<MicroItem> UpdateItemAsync(MicroItem item);
		Task DeleteItemAsync(int id);
    }
}
