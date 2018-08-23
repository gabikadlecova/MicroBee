using System.ComponentModel.DataAnnotations;

namespace MicroBee.Web.DAL.Entities
{
	public class MicroItem
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		[DataType(DataType.Currency)]
	    public decimal Price { get; set; }
		public ItemCategory Category { get; set; }
		public ItemStatus Status { get; set; }
		public string ImageAddress { get; set; }

		[Required]
		public string OwnerName { get; set; }
		public string WorkerName { get; set; }
    }

	public enum ItemStatus
	{
		Open, Accepted, Completed, Deleted
	}
}
