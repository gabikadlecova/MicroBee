using System;
using MicroBee.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace MicroBee
{
	public partial class App : Application
	{
		public static readonly IMicroItemService Service = new MicroItemService(new HttpService("http://localhost:56632/", "api/account/login/", "api/account/register/"));

		public App ()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new MainPage());
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
