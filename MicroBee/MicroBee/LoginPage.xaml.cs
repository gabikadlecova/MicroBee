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
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginModel LoginData { get; }
		public LoginPage ()
		{
			InitializeComponent ();
			BindingContext = this;
			LoginData = new LoginModel();
		}

		private async void LoginSubmitButton_OnClicked(object sender, EventArgs e)
		{
			bool succeeded = await App.AccountService.LoginAsync(LoginData);
			if (succeeded)
			{
				await Navigation.PopModalAsync();
			}
			else
			{
				errorLabel.IsVisible = true;
			}
		}

		private async void RegisterButton_OnClicked(object sender, EventArgs e)
		{
			var page = new RegisterPage();
			page.RegisterHandler += async () => { await Navigation.PopModalAsync(); };
			await Navigation.PushModalAsync(page);
			
		}
	}
}