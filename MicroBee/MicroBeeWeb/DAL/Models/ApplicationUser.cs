﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace MicroBee.Web.Models
{
	public class ApplicationUser : IdentityUser
	{
		public bool Valid { get; set; }
	}
}