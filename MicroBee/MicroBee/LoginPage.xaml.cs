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
	/// Represents a login form page
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public event EventHandler LoginSucceeded;
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
				LoginSucceeded?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				errorLabel.IsVisible = true;
			}
		}
	}
}