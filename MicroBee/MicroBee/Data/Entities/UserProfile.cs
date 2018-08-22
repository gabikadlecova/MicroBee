using System.Collections.Generic;

namespace MicroBee.Data.Entities
{
	public class UserProfile
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public bool Valid { get; set; }
		public List<MicroItem> CreatedItems { get; set; }
		public List<MicroItem> AcceptedItems { get; set; }
	}
}
