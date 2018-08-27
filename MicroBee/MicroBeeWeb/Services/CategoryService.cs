using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Entities;
using MicroBee.Web.DAL.Repositories;

namespace MicroBee.Web.Services
{
	class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _repository;

		public CategoryService(ICategoryRepository repository)
		{
			_repository = repository;
		}

		public async Task<ItemCategory> FindCategoryAsync(int id)
		{
			return await _repository.FindAsync(id);
		}

		public IEnumerable<ItemCategory> GetCategories()
		{
			return _repository.GetAll();
		}

		public async Task<ItemCategory> InsertCategoryAsync(ItemCategory category)
		{
			return await _repository.AddAsync(category);
		}

		public async Task<ItemCategory> UpdateCategoryAsync(ItemCategory category)
		{
			return await _repository.UpdateAsync(category);
		}

		public async Task DeleteCategoryAsync(int id)
		{
			await _repository.DeleteAsync(id);
		}
	}
}
