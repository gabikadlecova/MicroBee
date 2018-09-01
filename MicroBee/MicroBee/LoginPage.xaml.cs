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
		public delegate void LoginSubmitted(object sender, EventArgs e);

		public event LoginSubmitted LoginEventHandler;
		public LoginModel LoginData { get; set; }
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
				LoginEventHandler?.Invoke(this, EventArgs.Empty);
				await Navigation.PopModalAsync();
			}
		}
	}
}