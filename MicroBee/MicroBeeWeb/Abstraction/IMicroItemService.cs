using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Model;

namespace MicroBee.Services
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
