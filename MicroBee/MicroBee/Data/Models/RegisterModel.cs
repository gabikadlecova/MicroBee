namespace MicroBee.Data.Models
{
	/// <summary>
	/// Represents the register model which can be used in api registration method.
	/// </summary>
	public class RegisterModel
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
	}
}
