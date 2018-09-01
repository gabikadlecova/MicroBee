using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroBee.Data;

using MicroBee.Data.Models;
using MicroBee.ViewModels;
using Xamarin.Forms;

namespace MicroBee
{
	public partial class MainPage : ContentPage
	{

		public MainPage()
		{
			InitializeComponent();
			itemListView.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
			{
				ItemDetailPage detailPage = new ItemDetailPage()
				{
					Pages = new ObservableCollection<DetailViewModel>(Model.Items.Select(it => new DetailViewModel()
					{
						ItemImage = it.ItemImage,
						Categories = Model.Categories,
						Item = it.Item
					}))
				};
				var selectedItem = (InfiniteItemElement)((ListView) sender).SelectedItem;
				detailPage.Selected =
					detailPage.Pages.FirstOrDefault(m => m.Item.Id == selectedItem.Item.Id);

				await Navigation.PushAsync(detailPage);
			};
		}

		protected override async void OnAppearing()
		{
			await Model.Items.LoadMoreAsync();
			Model.Categories = await App.ItemService.GetCategoriesAsync();
		}
		
	}
}
