namespace MicroBee.Data.Models
{
	/// <summary>
	/// Represents a login model which can be sent to the API in order to login.
	/// </summary>
	public class LoginModel
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
