using System;
using System.Linq;
using MicroBee.ViewModels;
using Xamarin.Forms;

namespace MicroBee
{
	/// <summary>
	/// Represents a job list page
	/// </summary>
	public partial class ItemsPage : ContentPage
	{
		public ItemsPage()
		{
			InitializeComponent();

			// item selected handler
			itemListView.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
			{
				//the method is called if we set the item to null as well!
				var selectedItem = (InfiniteItemElement)((ListView)sender).SelectedItem;
				if (selectedItem == null)
				{
					return;
				}

				//carousel of detail pages
				ItemCarouselPage carouselPage = new ItemCarouselPage();
				foreach (var element in Model.Items)
				{
					carouselPage.Children.Add(new ItemDetailPage()
					{
						ItemId = element.Item.Id,
						IsAcceptEnabled = true
					});
				}

				//current page based on the selected item
				carouselPage.CurrentPage =
					carouselPage.Children.FirstOrDefault(p => ((ItemDetailPage)p).ItemId == selectedItem.Item.Id);

				await Navigation.PushAsync(carouselPage);

				itemListView.SelectedItem = null;
			};

			// model initialization
			Initialize();
		}

		private async void Initialize()
		{
			Model.Categories = await App.ItemService.GetCategoriesAsync();

			await Model.Items.LoadMoreAsync();
		}


		private void Entry_OnCompleted(object sender, EventArgs e)
		{
			// applies a search filter to the model
			var entry = (Entry) sender;
			Model.Items.TitleSearch = entry.Text;
		}
	}
}
