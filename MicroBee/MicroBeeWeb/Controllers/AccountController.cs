using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroBee.Web.Controllers
{
	[Route("api/[controller]")]
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;

		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		
		// POST api/<controller>
		[AllowAnonymous]
		[HttpPost]
		public Task<IActionResult> Login([FromBody]string value)
		{
		
		}

		// POST api/<controller>
		[AllowAnonymous]
		[HttpPost]
		public Task<IActionResult> Register([FromBody]string value)
		{
		}

		private async Task<object> CreateJwtToken(IdentityUser user)
		{
			List<Claim> claims = new List<Claim>()
			{

			};
		}
	}
}
