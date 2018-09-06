using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MicroBee
{
	/// <summary>
	/// Represents a starting page which either redirects to item page or waits for user authentication
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
			
			//user is immediately logged in if possible
			TryLogin();
		}

		private async void TryLogin()
		{
			//login by stored data
			if (await App.AccountService.TryLoginAsync())
			{
				App.Current.MainPage = new ItemMasterDetailPage();
			}
		}

		private void LoginButton_OnClicked(object sender, EventArgs e)
		{
			//after login, user can continue to use the app
			var loginPage = new LoginPage();
			loginPage.LoginSucceeded += async (senderL, eL) =>
			{
				await Navigation.PopAsync();
				App.Current.MainPage = new ItemMasterDetailPage();
			};

			Navigation.PushAsync(loginPage);
		}

		private void RegisterButton_OnClicked(object sender, EventArgs e)
		{
			//after register, user can continue to use the app
			var registerPage = new RegisterPage();
			registerPage.RegisterSucceeded += async (senderR, eR) =>
			{
				await Navigation.PopAsync();
				App.Current.MainPage = new ItemMasterDetailPage();
			};

			Navigation.PushAsync(registerPage);
		}
	}
}