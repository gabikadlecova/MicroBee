using System.Collections.Generic;

namespace MicroBee.Data.Models
{
	/// <summary>
	/// Represents a user profile model obtained from the API
	/// </summary>
	public class UserProfile
	{
		public string Username { get; set; }
		public string Email { get; set; }
		/// <summary>
		/// Is set to true if the user is eligible to create and accept jobs.
		/// To be added
		/// </summary>
		public bool Valid { get; set; }
		/// <summary>
		/// List of jobs which the user has created
		/// </summary>
		public List<MicroItem> CreatedItems { get; set; }
		/// <summary>
		/// List of jobs which has the user accepted
		/// </summary>
		public List<MicroItem> AcceptedItems { get; set; }
	}
}
