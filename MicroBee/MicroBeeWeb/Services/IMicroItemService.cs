using System.Collections.Generic;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Entities;

namespace MicroBee.Web.Services
{
    public interface IMicroItemService
    {
		Task<MicroItem> FindItemAsync(int id);
		IEnumerable<MicroItem> GetAllItems(int pageNumber, int pageSize);
		IEnumerable<MicroItem> GetOpenItems(int pageNumber, int pageSize);

		IEnumerable<MicroItem> GetOpenItems(int pageNumber, int pageSize, string category);
		IEnumerable<MicroItem> GetOpenItems(int pageNumber, int pageSize, string category, string titleFilter);
		Task<MicroItem> InsertItemAsync(MicroItem item);
		Task<MicroItem> UpdateItemAsync(MicroItem item);
		Task DeleteItemAsync(int id);
    }
}
