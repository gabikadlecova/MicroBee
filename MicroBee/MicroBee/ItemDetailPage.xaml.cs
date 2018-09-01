using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MicroBee.Data.Models;
using MicroBee.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace MicroBee
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemDetailPage : CarouselPage
	{
		public ObservableCollection<DetailViewModel> Pages { get; set; }
		public DetailViewModel Selected { get; set; }
		public ItemDetailPage ()
		{
			InitializeComponent ();
		}

		protected override void OnAppearing()
		{
			BindingContext = this;
		}

		private void Button_OnClicked(object sender, EventArgs e)
		{
			if (!App.IsUserAuthenticated)
			{
				LoginPage page = new LoginPage();
				page.LoginEventHandler += UpdateUserHandler;
				Navigation.PushModalAsync(page);
			}
		}

		private void UpdateUserHandler(object sender, EventArgs e)
		{
			Pages.ForEach((DetailViewModel model) =>
			{
				model.UpdateUser();
			});
		}
	}
}