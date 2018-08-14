using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Entities;

namespace MicroBee.Abstraction
{
    interface IMicroItemRepository
    {
		MicroItem Find(string id);
		IEnumerable<MicroItem> GetAll();
		MicroItem Insert(MicroItem item);
		MicroItem Update(MicroItem item);
		void Delete(string id);
	}
}
