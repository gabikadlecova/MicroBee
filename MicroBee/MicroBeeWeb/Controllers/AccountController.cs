using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Runtime.Serialization;

using MicroBee.Web.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroBee.Web.Controllers
{
	[Route("api/[controller]")]
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private IConfiguration _configuration;

		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
		}
		
		// POST api/<controller>
		[AllowAnonymous]
		[HttpPost]
		public Task<IActionResult> Login([FromBody]string value)
		{
			throw new NotImplementedException();
		}

		// POST api/<controller>
		[AllowAnonymous]
		[HttpPost]
		public Task<IActionResult> Register([FromBody]string value)
		{
			throw new NotImplementedException();
		}

		private object CreateJwtToken(ApplicationUser user)
		{
			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
				
				//todo admin Claim
			};

			var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
			var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
			
			if(!int.TryParse(_configuration["JwtExpire"], out int expireMins))
			{
				throw new InvalidConfigurationException("Invalid jwt token expiration date format in configuration file.");
			}

			var expireTimeSpan = TimeSpan.FromMinutes(expireMins);

			var token = new JwtSecurityToken(
				issuer: _configuration["JwtIssuer"],
				audience: _configuration["JwtAudience"],
				claims: claims,
				expires: DateTime.Now + expireTimeSpan,
				signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public class InvalidConfigurationException : Exception
		{
			public InvalidConfigurationException()
			{
			}

			public InvalidConfigurationException(string message) : base(message)
			{
			}

			public InvalidConfigurationException(string message, Exception innerException) : base(message, innerException)
			{
			}

			protected InvalidConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}
	}
}
