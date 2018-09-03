using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MicroBee.Data.Models;

namespace MicroBee.Data
{
    public interface IAccountService
    {
	    Task<bool> LoginAsync(LoginModel model);
	    void Logout();
	    Task<bool> RegisterAsync(RegisterModel model);
	    Task<UserProfile> GetUserProfileAsync();
	    Task UpdateUserProfileAsync(UserProfile profile);
    }
}
