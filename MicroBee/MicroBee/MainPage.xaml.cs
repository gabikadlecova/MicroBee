using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MicroBee
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
			TryLogin();
		}

		private async void TryLogin()
		{
			if (await App.AccountService.TryLoginAsync())
			{
				App.Current.MainPage = new ItemMasterDetailPage();
			}
		}

		private void LoginButton_OnClicked(object sender, EventArgs e)
		{
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