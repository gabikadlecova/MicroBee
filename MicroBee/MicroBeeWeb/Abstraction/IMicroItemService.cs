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
		IEnumerable<MicroItem> FindItems(string substr);
		IEnumerable<MicroItem> GetAllItems();
		IEnumerable<MicroItem> GetOpenItems();

		IEnumerable<MicroItem> GetOpenItems(string category);
		Task<MicroItem> InsertItemAsync(MicroItem item);
		Task<MicroItem> UpdateItemAsync(MicroItem item);
		Task DeleteItemAsync(int id);
    }
}
