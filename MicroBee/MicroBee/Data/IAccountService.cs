using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MicroBee.Data.Models;

namespace MicroBee.Data
{
	/// <summary>
	/// Represents a service interface which provides basic account bound operations
	/// </summary>
    public interface IAccountService
    {
		/// <summary>
		/// Authenticates the user with the provided login data
		/// </summary>
		/// <param name="model">Login data to authenticate with</param>
		/// <returns>True if the login succeeded</returns>
	    Task<bool> LoginAsync(LoginModel model);
		/// <summary>
		/// Tries to log in with data saved from previous session.
		/// </summary>
		/// <returns>False if either the login failed or if there is no login data saved in the device, true if login succeeded.</returns>
	    Task<bool> TryLoginAsync();
		/// <summary>
		/// Logs the user out by deleting login data from the device.
		/// </summary>
	    void Logout();
		/// <summary>
		/// Registers a new user with credentials specified in the model; user is immediately logged in if the registration succeeds.
		/// </summary>
		/// <param name="model">Register data of the new user</param>
		/// <returns>True if the registration succeeded.</returns>
	    Task<bool> RegisterAsync(RegisterModel model);
		/// <summary>
		/// Gets the profile of the currently logged in user.
		/// </summary>
		/// <returns>User profile of current user</returns>
	    Task<UserProfile> GetUserProfileAsync();
		/// <summary>
		/// Updates the profile of the current user.
		/// </summary>
		/// <param name="profile">New profile data; accepted or created jobs should not be changed this way</param>
		/// <returns></returns>
	    Task UpdateUserProfileAsync(UserProfile profile);
    }
}
