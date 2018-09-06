using System.Collections.Generic;
using System.Threading.Tasks;
using MicroBee.Data.Models;

namespace MicroBee.Data
{
	/// <summary>
	/// Represents a service interface which provides access to job data.
	/// </summary>
	public interface IMicroItemService
	{
		/// <summary>
		/// Gets the job with the specified id.
		/// </summary>
		/// <param name="id">Job id</param>
		/// <returns>The job with the id.</returns>
		Task<MicroItem> GetMicroItemAsync(int id);
		/// <summary>
		/// Gets a page of open jobs.
		/// </summary>
		/// <param name="pageNumber">Page number</param>
		/// <param name="pageSize">Page size</param>
		/// <returns>A page of jobs or less then a page in case of the last page.</returns>
		Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize);
		/// <summary>
		/// Gets a page of open jobs from the specified category.
		/// </summary>
		/// <param name="pageNumber">Page number</param>
		/// <param name="pageSize">Page size</param>
		/// <param name="category">Job category</param>
		/// <returns>A page of jobs or less then a page in case of the last page.</returns>
		Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize, string category);
		/// <summary>
		/// Gets a page of open jobs from the specified category and with a search filter applied to the job title.
		/// </summary>
		/// <param name="pageNumber">Page number</param>
		/// <param name="pageSize">Page size</param>
		/// <param name="category">Job category</param>
		/// <param name="titleFilter">Search filter which will be applied to job titles.</param>
		/// <returns>A page of jobs or less then a page in case of the last page.</returns>
		Task<List<MicroItem>> GetMicroItemsAsync(int pageNumber, int pageSize, string category, string titleFilter);
		/// <summary>
		/// Adds a new job.
		/// </summary>
		/// <param name="item">Item data</param>
		/// <param name="image">Image data; if null, the job is still added (without an image).</param>
		/// <returns></returns>
		Task AddMicroItemAsync(MicroItem item, byte[] image);
		/// <summary>
		/// Updates a job.
		/// </summary>
		/// <param name="item">Item data</param>
		/// <param name="image">Image data (works as with adding)</param>
		/// <returns></returns>
		Task UpdateMicroItemAsync(MicroItem item, byte[] image);
		/// <summary>
		/// Deletes a job
		/// </summary>
		/// <param name="id">Job id</param>
		/// <returns></returns>
		Task DeleteMicroItemAsync(int id);
		/// <summary>
		/// Sets the current user as the worker of the job.
		/// </summary>
		/// <param name="itemId">Job id</param>
		/// <returns></returns>
		Task SetAsWorkerAsync(int itemId);
		
		/// <summary>
		/// Returns the list of all job categories
		/// </summary>
		/// <returns>Job category list</returns>
		Task<List<ItemCategory>> GetCategoriesAsync();
		/// <summary>
		/// Gets the data of the image with the specified ID
		/// </summary>
		/// <param name="id">Image id</param>
		/// <returns>Image data</returns>
		Task<byte[]> GetImageAsync(int id);
	}
}
