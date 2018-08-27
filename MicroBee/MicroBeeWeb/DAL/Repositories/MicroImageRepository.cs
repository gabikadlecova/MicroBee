using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Context;
using MicroBee.Web.DAL.Entities;

namespace MicroBee.Web.DAL.Repositories
{
	public class MicroImageRepository : IMicroImageRepository
	{
		private readonly MicroBeeDbContext _context;

		public MicroImageRepository(MicroBeeDbContext context)
		{
			_context = context;
		}

		public async Task<ItemImage> FindAsync(int id)
		{
			return await _context.Images.FindAsync(id);
		}

		public IQueryable<ItemImage> GetAll()
		{
			return _context.Images.AsQueryable();
		}

		public async Task<ItemImage> AddAsync(ItemImage image)
		{
			var added = await _context.Images.AddAsync(image);
			await _context.SaveChangesAsync();
			return added.Entity;
		}

		public async Task<ItemImage> UpdateAsync(ItemImage image)
		{
			var updated = _context.Images.Update(image);
			await _context.SaveChangesAsync();
			return updated.Entity;
		}

		public async Task DeleteAsync(int id)
		{
			var image = await _context.Images.FindAsync(id);
			_context.Images.Remove(image);
			await _context.SaveChangesAsync();
		}
	}
}
