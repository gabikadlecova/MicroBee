using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using MicroBee.Web.Abstraction;
using MicroBee.Web.Models;

namespace MicroBee.Web.Services
{
	public class MicroItemRepository : IMicroItemRepository
	{
		public Task DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<MicroItem> FindAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IQueryable<MicroItem>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<MicroItem> InsertAsync(MicroItem item)
		{
			throw new NotImplementedException();
		}

		public Task<MicroItem> UpdateAsync(MicroItem item)
		{
			throw new NotImplementedException();
		}
	}
}
