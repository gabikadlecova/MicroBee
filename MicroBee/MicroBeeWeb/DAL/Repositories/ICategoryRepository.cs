using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Entities;

namespace MicroBee.Web.DAL.Repositories
{
	public interface ICategoryRepository
	{
		Task<ItemCategory> FindAsync(int id);
		IQueryable<ItemCategory> GetAll();
		Task<ItemCategory> AddAsync(ItemCategory category);
		Task<ItemCategory> UpdateAsync(ItemCategory category);
		Task DeleteAsync(int id);
	}
}
