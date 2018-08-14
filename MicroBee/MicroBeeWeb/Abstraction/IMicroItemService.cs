using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBeeWeb.Model;

namespace MicroBeeWeb.Services
{
    interface IMicroItemService
    {
		MicroItem FindItem(string id);
		IEnumerable<MicroItem> GetItems();
		MicroItem InsertItem(MicroItem item);
		MicroItem UpdateItem(MicroItem item);
		void DeleteItem(string id);
    }
}
