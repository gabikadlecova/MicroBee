using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroBee.Data.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MicroBee
{
	/// <summary>
	/// Represents a user profile detail page and logout interface
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage ()
		{
			InitializeComponent ();
		}

		protected override async void OnAppearing()
		{
			if (!App.IsUserAuthenticated)
			{
				throw new InvalidOperationException("User must be logged in to access this page");
			}

			Model.Profile = await App.AccountService.GetUserProfileAsync();
		}

		private void LogoutButton_OnClicked(object sender, EventArgs e)
		{
			App.AccountService.Logout();
			App.Current.MainPage = new NavigationPage(new MainPage());
		}
	}
}