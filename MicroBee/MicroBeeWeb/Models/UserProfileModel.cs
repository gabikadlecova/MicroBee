using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Entities;

namespace MicroBee.Web.Models
{
	public class UserProfileModel
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public bool Valid { get; set; }
		public List<MicroItem> CreatedItems { get; set; }
		public List<MicroItem> AcceptedItems { get; set; }
	}
}
