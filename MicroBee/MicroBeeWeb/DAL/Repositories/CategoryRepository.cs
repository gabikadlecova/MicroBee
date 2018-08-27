using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Context;
using MicroBee.Web.DAL.Entities;

namespace MicroBee.Web.DAL.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly MicroBeeDbContext _context;
		public CategoryRepository(MicroBeeDbContext context)
		{
			_context = context;
		}

		public async Task<ItemCategory> FindAsync(int id)
		{
			return await _context.Categories.FindAsync(id);
		}

		public IQueryable<ItemCategory> GetAll()
		{
			return _context.Categories.AsQueryable();
		}

		public async Task<ItemCategory> AddAsync(ItemCategory category)
		{
			var added = await _context.Categories.AddAsync(category);
			await _context.SaveChangesAsync();
			return added.Entity;
		}

		public async Task<ItemCategory> UpdateAsync(ItemCategory category)
		{
			var updated = _context.Categories.Update(category);
			await _context.SaveChangesAsync();
			return updated.Entity;
		}

		public async Task DeleteAsync(int id)
		{
			var category = await _context.Categories.FindAsync(id);
			_context.Categories.Remove(category);
			await _context.SaveChangesAsync();
		}
	}
}
