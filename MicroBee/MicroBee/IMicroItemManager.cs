using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroBee
{
	interface IMicroItemManager
    {
		Task<List<MicroItem>> GetItemListAsync();
		Task SaveItemAsync(MicroItem item, bool addedNew);
		Task DeleteItemAsync(string id);

    }
}
