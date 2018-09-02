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
	public partial class RegisterPage : ContentPage
	{
		public delegate void RegisterCompleted();

		public event RegisterCompleted RegisterHandler;
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
				await Navigation.PopModalAsync();
				RegisterHandler?.Invoke();
			}
			else
			{
				errorLabel.IsVisible = true;
			}
		}
	}
}