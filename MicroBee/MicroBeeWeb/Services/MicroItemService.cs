using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.Models;
using MicroBee.Web.Abstraction;

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

		public IEnumerable<MicroItem> FindItems(string substr)
		{
			var items = OpenItems();
			return items.Where(m => m.Title.Contains(substr));
		}

		public IEnumerable<MicroItem> GetAllItems()
		{
			return _repository.GetAll();
		}

		public IEnumerable<MicroItem> GetOpenItems()
		{
			//todo filtering, Linq to db
			return OpenItems();
		}

		public IEnumerable<MicroItem> GetOpenItems(string category)
		{
			var items = _repository.GetAll();
			return items.Where(item => item.Category.Name == category);
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
