using System.ComponentModel.DataAnnotations;

namespace MicroBee.Web.Models
{
	public class RegisterModel
	{
		[Required]
		public string Username { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
	}
}
