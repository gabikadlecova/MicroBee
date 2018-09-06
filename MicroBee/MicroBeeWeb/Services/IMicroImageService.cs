using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Entities;

namespace MicroBee.Web.Services
{
	public interface IMicroImageService
	{
		/// <summary>
		/// Find an image with the specified ID
		/// </summary>
		/// <param name="id">ID of the image</param>
		/// <returns>Image with the matching ID</returns>
		Task<ItemImage> FindImageAsync(int id);
		/// <summary>
		/// Gets all item images
		/// </summary>
		/// <returns>IEnumerable of item images</returns>
		IEnumerable<ItemImage> GetImages();
		/// <summary>
		/// Adds a new image
		/// </summary>
		/// <param name="image">Image to be added</param>
		/// <returns>Added image</returns>
		Task<ItemImage> InsertImageAsync(ItemImage image);
		/// <summary>
		/// Updates an image
		/// </summary>
		/// <param name="image">Image to be updated</param>
		/// <returns>Updated image</returns>
		Task<ItemImage> UpdateImageAsync(ItemImage image);
		/// <summary>
		/// Deletes an image with specified ID
		/// </summary>
		/// <param name="id">ID of the image to be deleted</param>
		/// <returns></returns>
		Task DeleteImageAsync(int id);
	}
}
