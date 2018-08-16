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

		public async Task<IEnumerable<MicroItem>> FindItemsAsync(string substr)
		{
			var items = await OpenItemsAsync();
			return items.Where(m => m.Title.Contains(substr));
		}

		public async Task<IEnumerable<MicroItem>> GetAllItemsAsync()
		{
			return await _repository.GetAllAsync();
		}

		public async Task<IEnumerable<MicroItem>> GetOpenItemsAsync()
		{
			//todo filtering, Linq to db
			return await OpenItemsAsync();
		}

		public async Task<IEnumerable<MicroItem>> GetOpenItemsAsync(string category)
		{
			var items = await _repository.GetAllAsync();
			return items.Where(item => item.Category.Name == category);
		}

		public async Task<MicroItem> InsertItemAsync(MicroItem item)
		{
			return await _repository.InsertAsync(item);
		}

		public async Task<MicroItem> UpdateItemAsync(MicroItem item)
		{
			return await _repository.UpdateAsync(item);
		}

		private async Task<IQueryable<MicroItem>> OpenItemsAsync()
		{
			var items = await _repository.GetAllAsync();
			return items.Where(item => item.Status == ItemStatus.Open);
		}
	}
}
