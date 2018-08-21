namespace MicroBee.Web.DAL.Entities
{
	public class MicroItem
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public ItemCategory Category { get; set; }
		public ItemStatus Status { get; set; }
		public string ImageAddress { get; set; }

		public int OwnerId { get; set; }
		public int? WorkerId { get; set; }
    }

	public enum ItemStatus
	{
		Open, Accepted, Completed, Deleted
	}
}
