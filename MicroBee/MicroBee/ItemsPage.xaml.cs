using System.Linq;
using MicroBee.ViewModels;
using Xamarin.Forms;

namespace MicroBee
{
	public partial class ItemsPage : ContentPage
	{
		public ItemsPage()
		{
			InitializeComponent();

			itemListView.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
			{
				var selectedItem = (InfiniteItemElement)((ListView)sender).SelectedItem;
				if (selectedItem == null)
				{
					return;
				}

				ItemCarouselPage carouselPage = new ItemCarouselPage();
				foreach (var element in Model.Items)
				{
					carouselPage.Children.Add(new ItemDetailPage()
					{
						ItemId = element.Item.Id,
						IsAcceptEnabled = true
					});
				}

				carouselPage.CurrentPage =
					carouselPage.Children.FirstOrDefault(p => ((ItemDetailPage)p).ItemId == selectedItem.Item.Id);

				await Navigation.PushAsync(carouselPage);

				itemListView.SelectedItem = null;
			};

			Initialize();
		}

		private async void Initialize()
		{
			Model.Categories = await App.ItemService.GetCategoriesAsync();

			await Model.Items.LoadMoreAsync();
		}

	}
}
