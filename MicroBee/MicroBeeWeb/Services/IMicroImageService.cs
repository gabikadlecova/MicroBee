using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Entities;

namespace MicroBee.Web.Services
{
	public interface IMicroImageService
	{
		Task<ItemImage> FindImageAsync(int id);
		IEnumerable<ItemImage> GetImages();
		Task<ItemImage> InsertImageAsync(ItemImage image);
		Task<ItemImage> UpdateImageAsync(ItemImage image);
		Task DeleteImageAsync(int id);
	}
}
