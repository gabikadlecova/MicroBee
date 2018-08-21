using System.Collections.Generic;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Entities;

namespace MicroBee.Web.DAL.Repositories
{
    public interface IMicroItemService
    {
		Task<MicroItem> FindItemAsync(int id);
		IEnumerable<MicroItem> FindItems(string substr);
		IEnumerable<MicroItem> GetAllItems();
		IEnumerable<MicroItem> GetOpenItems();

		IEnumerable<MicroItem> GetOpenItems(string category);
		Task<MicroItem> InsertItemAsync(MicroItem item);
		Task<MicroItem> UpdateItemAsync(MicroItem item);
		Task DeleteItemAsync(int id);
    }
}
