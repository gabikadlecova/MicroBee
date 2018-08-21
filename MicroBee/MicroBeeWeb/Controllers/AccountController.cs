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
using System.Runtime.Serialization.Json;
using MicroBee.Web.DAL.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroBee.Web.Controllers
{
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

		// POST api/<controller>
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Login([FromBody]LoginModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = await _userManager.FindByNameAsync(model.Username);

			var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
			if (!signInResult.Succeeded)
			{
				ModelState.AddModelError("Password", "Invalid password.");
				return BadRequest(ModelState);
			}

			return Ok(CreateJwtToken(user));
		}

		// POST api/<controller>
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Register([FromBody]RegisterModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = new ApplicationUser() { UserName = model.Username, Email = model.Email };

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				foreach (IdentityError error in result.Errors)
				{
					//todo check
					ModelState.AddModelError(error.Code, error.Description);
				}
				return BadRequest(ModelState);
			}

			return Ok(CreateJwtToken(user));
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

			if (!int.TryParse(_configuration["JwtExpire"], out int expireMins))
			{
				throw new InvalidConfigurationException("Invalid jwt token expiration date format in configuration file.");
			}

			var expireDateTime = DateTime.Now + TimeSpan.FromMinutes(expireMins);

			var token = new JwtSecurityToken(
				issuer: _configuration["JwtIssuer"],
				audience: _configuration["JwtAudience"],
				claims: claims,
				expires: expireDateTime,
				signingCredentials: credentials);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

			return Json(new { tokenString, expireDateTime });
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
