using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Entities;

namespace MicroBee.Web.DAL.Repositories
{
	public interface IMicroImageRepository
	{
		Task<ItemImage> FindAsync(int id);
		IQueryable<ItemImage> GetAll();
		Task<ItemImage> AddAsync(ItemImage image);
		Task<ItemImage> UpdateAsync(ItemImage image);
		Task DeleteAsync(int id);
	}
}
