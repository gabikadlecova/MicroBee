using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MicroBee.Web.DAL.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public bool Valid { get; set; }
		public List<MicroItem> CreatedItems { get; set; }
		public List<MicroItem> AcceptedItems { get; set; }
	}
}
