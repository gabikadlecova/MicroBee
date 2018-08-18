namespace MicroBee.Data
{
	public class MicroItem
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public ItemCategory Category { get; set; }
		public ItemStatus Status { get; set; }
		public byte[] Image { get; set; }
    }

	public enum ItemStatus
	{
		Open, Accepted, Completed, Deleted
	}

	public class ItemCategory
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
