using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Context;
using Microsoft.EntityFrameworkCore;
using MicroBee.Web.DAL.Entities;
using MicroBee.Web.DAL.Repositories;

namespace MicroBee.Web.Services
{
	public class MicroItemRepository : IMicroItemRepository
	{
		private readonly MicroBeeDbContext _context;
		public MicroItemRepository(MicroBeeDbContext context)
		{
			_context = context;
		}

		public async Task DeleteAsync(int id)
		{
			var item = _context.MicroItems.FirstOrDefaultAsync(it => it.Id == id);
			_context.MicroItems.Remove(await item);
			await _context.SaveChangesAsync();
		}

		public async Task<MicroItem> FindAsync(int id)
		{
			return await _context.MicroItems.FindAsync(id);

		}

		public IQueryable<MicroItem> GetAll()
		{
			return _context.MicroItems.AsQueryable();
		}

		public async Task<MicroItem> AddAsync(MicroItem item)
		{
			var added = await _context.AddAsync(item);
			await _context.SaveChangesAsync();
			return added.Entity;
		}

		public async Task<MicroItem> UpdateAsync(MicroItem item)
		{
			var updated = _context.MicroItems.Update(item);
			await _context.SaveChangesAsync();
			return updated.Entity;
		}
	}
}
