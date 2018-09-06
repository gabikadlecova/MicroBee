namespace MicroBee.Data.Models
{
	/// <summary>
	/// Represents a single job obtained from the database.
	/// </summary>
	public class MicroItem
    {
		public int Id { get; set; }
		/// <summary>
		/// Title of the job
		/// </summary>
		public string Title { get; set; }
		/// <summary>
		/// Description of the job
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// Job category
		/// </summary>
		public ItemCategory Category { get; set; }
		/// <summary>
		/// Status of the job (i.e. whether it was accepted, payment information)
		/// </summary>
		public ItemStatus Status { get; set; }
		/// <summary>
		/// Is set to the image ID or to null if there is no image associated with this item.
		/// </summary>
		public int? ImageId { get; set; }
		/// <summary>
		/// 
		/// </summary>
	    public decimal Price { get; set; }
		/// <summary>
		/// Username of the user who created the item; must be specified.
		/// </summary>
		public string OwnerName { get; set; }
		/// <summary>
		/// Username of the user who accepted the job; can be set to null if there is no such user.
		/// </summary>
	    public string WorkerName { get; set; }
	}

	/// <summary>
	/// Represents the status of the job
	/// </summary>
	public enum ItemStatus
	{
		Open, Accepted, Completed, Deleted
	}

	/// <summary>
	/// Represents the job category
	/// </summary>
	public class ItemCategory
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
