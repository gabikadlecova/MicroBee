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
using MicroBee.Web.DAL.Entities;
using MicroBee.Web.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroBee.Web.Controllers
{
	/// <summary>
	/// Provides account bound API
	/// </summary>
	[Route("api/[controller]/[action]")]
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private IConfiguration _configuration;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
		}

		// POST api/account/login
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Login([FromBody]LoginModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			//login procedure
			var user = await _userManager.FindByNameAsync(model.Username);

			var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
			if (!signInResult.Succeeded)
			{
				ModelState.AddModelError("Password", "Invalid user or password.");
				return BadRequest(ModelState);
			}

			//result bearer token
			return Ok(CreateJwtToken(user));
		}

		// POST api/account/register
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Register([FromBody]RegisterModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			//user email cannot be duplicate
			if (await _userManager.FindByEmailAsync(model.Email) != null)
			{
				return BadRequest();
			}

			//register procedure
			var user = new ApplicationUser() { UserName = model.Username, Email = model.Email };

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError(error.Code, error.Description);
				}
				return BadRequest(ModelState);
			}

			//register is followed by immediate login
			var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
			if (!signInResult.Succeeded)
			{
				ModelState.AddModelError("Password", "Invalid password.");
				return BadRequest(ModelState);
			}

			//result bearer token
			return Ok(CreateJwtToken(user));
		}

		// GET api/account/profile
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Profile()
		{
			//user must be valid
			string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (id == null)
			{
				return Unauthorized();
			}

			//manually getting the user as to enable related data fetcg
			var user = await _userManager.Users
				.Where(u => u.Id == id)
				.Include(u => u.AcceptedItems)
					.ThenInclude(i => i.Category)
				.Include(u => u.CreatedItems)
					.ThenInclude(i => i.Category)
				.FirstOrDefaultAsync();

			//result model
			UserProfileModel model = new UserProfileModel()
			{
				Username = user.UserName,
				AcceptedItems = user.AcceptedItems,
				CreatedItems = user.CreatedItems,
				Valid = user.Valid,
				Email = user.Email
			};

			return Ok(model);
		}

		// POST api/account/update
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Update([FromBody]UserProfileModel model)
		{
			//user must be valid to update profile
			string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (id == null)
			{
				return Unauthorized();
			}

			// model Username and token Username don't match
			var user = await _userManager.FindByIdAsync(id);
			if (user.UserName != model.Username)
			{
				return Unauthorized();
			}

			user.Email = model.Email;

			//update
			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded)
			{
				return BadRequest();
			}

			return await Profile();
		}

		private object CreateJwtToken(ApplicationUser user)
		{
			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
				
				//todo admin Claim
			};

			//creating credentials
			var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
			var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

			//expiration data
			if (!int.TryParse(_configuration["JwtExpire"], out int expireMins))
			{
				throw new InvalidConfigurationException("Invalid jwt token expiration date format in configuration file.");
			}

			var expireDateTime = DateTime.Now + TimeSpan.FromMinutes(expireMins);

			//token creation
			var token = new JwtSecurityToken(
				issuer: _configuration["JwtIssuer"],
				audience: _configuration["JwtAudience"],
				claims: claims,
				expires: expireDateTime,
				signingCredentials: credentials);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

			return new { tokenString, expireDateTime };
		}

		/// <summary>
		/// Is thrown if the config data is incorrect or missing
		/// </summary>
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
