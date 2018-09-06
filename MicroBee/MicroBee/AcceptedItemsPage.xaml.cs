using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroBee.Data.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MicroBee
{
	/// <summary>
	/// A page which shows a list of accepted jobs of the current user
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AcceptedItemsPage : ContentPage
	{
		public AcceptedItemsPage()
		{
			InitializeComponent();
		}
		protected override async void OnAppearing()
		{
			var profile = await App.AccountService.GetUserProfileAsync();
			Model.UserItems = new ObservableCollection<MicroItem>(profile.AcceptedItems);

		}
		private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			//creates a carousel page with selected item the same as the selected item of the list view (by content).

			var selectedItem = (MicroItem)((ListView)sender).SelectedItem;
			if (selectedItem == null)
			{
				return; ;
			}

			ItemCarouselPage page = new ItemCarouselPage();
			foreach (int itemId in Model.UserItems.Select(i => i.Id))
			{
				page.Children.Add(new ItemDetailPage()
				{
					ItemId = itemId
				});
			}

			page.CurrentPage = page.Children.FirstOrDefault(p => ((ItemDetailPage)p).ItemId == selectedItem.Id);
			Navigation.PushAsync(page);
		}
	}
}