using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Entities;

namespace MicroBee.Web.Services
{
	public interface ICategoryService
	{
		/// <summary>
		/// Finds the category with the specified ID
		/// </summary>
		/// <param name="id">Category ID</param>
		/// <returns>Category with matching ID</returns>
		Task<ItemCategory> FindCategoryAsync(int id);
		/// <summary>
		/// Gets an enumerable of categories
		/// </summary>
		/// <returns>Enumerable of all categories</returns>
		IEnumerable<ItemCategory> GetCategories();
		/// <summary>
		/// Adds a new category
		/// </summary>
		/// <param name="category">Category to be added</param>
		/// <returns>Added category</returns>
		Task<ItemCategory> InsertCategoryAsync(ItemCategory category);
		/// <summary>
		/// Updates an existing category
		/// </summary>
		/// <param name="category">Category to be updated</param>
		/// <returns>Updated category</returns>
		Task<ItemCategory> UpdateCategoryAsync(ItemCategory category);
		/// <summary>
		/// Deletes a category with the specified ID
		/// </summary>
		/// <param name="id">Category id</param>
		/// <returns></returns>
		Task DeleteCategoryAsync(int id);
	}
}
