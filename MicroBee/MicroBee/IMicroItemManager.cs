using System.Collections.Generic;
using System.Threading.Tasks;
using MicroBee.Web.Models;

namespace MicroBee
{
	public interface IMicroItemManager
    {
		Task<List<MicroItem>> GetItemListAsync();
		Task SaveItemAsync(MicroItem item, bool addedNew);
		Task DeleteItemAsync(string id);
    }
}
