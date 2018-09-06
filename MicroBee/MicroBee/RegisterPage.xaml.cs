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
	/// Represents a register form page
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPage : ContentPage
	{
		public event EventHandler RegisterSucceeded;
		public RegisterModel RegisterData { get; }
		public RegisterPage()
		{
			InitializeComponent();
			BindingContext = this;
			RegisterData = new RegisterModel();
		}

		private async void RegisterSubmitButton_OnClicked(object sender, EventArgs e)
		{
			bool succeeded = await App.AccountService.RegisterAsync(RegisterData);
			if (succeeded)
			{
				RegisterSucceeded?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				errorLabel.IsVisible = true;
			}
		}
	}
}