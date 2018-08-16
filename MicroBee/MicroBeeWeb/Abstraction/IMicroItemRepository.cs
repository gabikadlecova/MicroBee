using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.Models;

namespace MicroBee.Web.Abstraction
{
    public interface IMicroItemRepository
    {
		Task<MicroItem> FindAsync(int id);
		Task<IQueryable<MicroItem>> GetAllAsync();
		Task<MicroItem> InsertAsync(MicroItem item);
		Task<MicroItem> UpdateAsync(MicroItem item);
		Task DeleteAsync(int id);
	}
}
