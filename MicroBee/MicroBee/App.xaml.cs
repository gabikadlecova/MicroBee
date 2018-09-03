using System;
using MicroBee.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace MicroBee
{
	public partial class App : Application
	{
		private static readonly HttpService HttpService =
			new HttpService("http://localhost:56632/", "api/account/login/", "api/account/register/");
		public static readonly IMicroItemService ItemService = new MicroItemService(HttpService);
		public static readonly IAccountService AccountService= new AccountService(HttpService);

		public static string UserName => HttpService.Username;
		public static bool IsUserAuthenticated => HttpService.Authenticated;

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
