using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Entities;
using MicroBee.Web.DAL.Repositories;

namespace MicroBee.Web.Services
{
	class MicroItemService : IMicroItemService
	{
		private readonly IMicroItemRepository _repository;
		public MicroItemService(IMicroItemRepository repository)
		{
			_repository = repository;
		}

		public async Task DeleteItemAsync(int id)
		{
			await _repository.DeleteAsync(id);
		}

		public async Task<MicroItem> FindItemAsync(int id)
		{
			return await _repository.FindAsync(id);
		}

		public IEnumerable<MicroItem> GetAllItems(int pageNumber, int pageSize)
		{
			return _repository.GetAll().OrderByDescending(m => m.Id).Skip(pageNumber * pageSize).Take(pageSize);
		}

		public IEnumerable<MicroItem> GetOpenItems(int pageNumber, int pageSize)
		{
			return OpenItems().OrderByDescending(m => m.Id).Skip(pageNumber * pageSize).Take(pageSize);
		}

		public IEnumerable<MicroItem> GetOpenItems(int pageNumber, int pageSize, string category)
		{
			var items = _repository.GetAll().Where(item => item.Category.Name == category);
			return items.OrderByDescending(m => m.Id).Skip(pageNumber * pageSize).Take(pageSize);
		}

		public IEnumerable<MicroItem> GetOpenItems(int pageNumber, int pageSize, string category, string titleFilter)
		{
			IQueryable<MicroItem> items;
			if (category == null)
			{
				items = _repository.GetAll().Where(item => item.Title.Contains(titleFilter));
			}
			else
			{
				items = _repository.GetAll().Where(item => item.Category.Name == category && item.Title.Contains(titleFilter));
			}
			return items.OrderByDescending(m => m.Id).Skip(pageNumber * pageSize).Take(pageSize);
		}

		public async Task<MicroItem> InsertItemAsync(MicroItem item)
		{
			return await _repository.AddAsync(item);
		}

		public async Task<MicroItem> UpdateItemAsync(MicroItem item)
		{
			return await _repository.UpdateAsync(item);
		}

		private IQueryable<MicroItem> OpenItems()
		{
			var items = _repository.GetAll();
			return items.Where(item => item.Status == ItemStatus.Open);
		}
	}
}
