using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Entities;

namespace MicroBee.Web.Services
{
	public interface ICategoryService
	{
		Task<ItemCategory> FindCategoryAsync(int id);
		IEnumerable<ItemCategory> GetCategories();
		Task<ItemCategory> InsertCategoryAsync(ItemCategory category);
		Task<ItemCategory> UpdateCategoryAsync(ItemCategory category);
		Task DeleteCategoryAsync(int id);
	}
}
