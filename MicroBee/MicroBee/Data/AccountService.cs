using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MicroBee.Data.Models;

namespace MicroBee.Data
{
    class AccountService : IAccountService
    {
		private HttpService Service { get; }
	    public AccountService(HttpService service)
	    {
		    Service = service;
	    }

	    public async Task<bool> LoginAsync(LoginModel model)
	    {
		    try
		    {
			    await Service.LoginAsync(model);
			    return true;
		    }
		    catch (InvalidResponseException)
		    {
			    return false;
		    }
	    }

	    public async Task<bool> RegisterAsync(RegisterModel model)
	    {
			try
			{
				await Service.RegisterAsync(model);
				return true;
			}
			catch (InvalidResponseException)
			{
				return false;
			}
		}

	    public async Task<UserProfile> GetUserProfileAsync()
	    {
		    return await Service.GetAsync<UserProfile>("api/account/profile/", authorize: true);
	    }

	    public async Task UpdateUserProfileAsync(UserProfile profile)
	    {
		    await Service.PostAsync("api/account/update/", profile, authorize: true);
	    }
    }
}
