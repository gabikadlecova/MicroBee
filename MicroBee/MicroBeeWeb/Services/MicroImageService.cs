using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Entities;
using MicroBee.Web.DAL.Repositories;

namespace MicroBee.Web.Services
{
	public class MicroImageService : IMicroImageService
	{
		private readonly IMicroImageRepository _repository;

		public MicroImageService(IMicroImageRepository repository)
		{
			_repository = repository;
		}

		public async Task<ItemImage> FindImageAsync(int id)
		{
			return await _repository.FindAsync(id);
		}

		public IEnumerable<ItemImage> GetImages()
		{
			return _repository.GetAll();
		}

		public async Task<ItemImage> InsertImageAsync(ItemImage image)
		{
			return await _repository.AddAsync(image);
		}

		public async Task<ItemImage> UpdateImageAsync(ItemImage image)
		{
			return await _repository.UpdateAsync(image);
		}

		public async Task DeleteImageAsync(int id)
		{
			await _repository.DeleteAsync(id);
		}
	}
}
