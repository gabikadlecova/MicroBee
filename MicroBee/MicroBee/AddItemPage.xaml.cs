using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroBee.Data.Models;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MicroBee
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddItemPage : ContentPage
	{
		public event EventHandler AddSucceeded;
		public AddItemPage()
		{
			InitializeComponent();

			Model.Item = new MicroItem();
		}

		protected override async void OnAppearing()
		{
			Model.Categories = new ObservableCollection<ItemCategory>(await App.ItemService.GetCategoriesAsync());
		}

		private async void SubmitButton_OnClicked(object sender, EventArgs e)
		{
			if (!App.IsUserAuthenticated)
			{
				throw new InvalidOperationException("User must be logged in to add an item.");
			}

			Model.Item.OwnerName = App.UserName;
			await App.ItemService.AddMicroItemAsync(Model.Item, Model.ImageData);
			AddSucceeded?.Invoke(this, EventArgs.Empty);
		}

		private async void ChooseImage_OnClicked(object sender, EventArgs e)
		{
			var file = await CrossMedia.Current.PickPhotoAsync();
			if (file == null)
			{
				return;
			}

			using (MemoryStream stream = new MemoryStream())
			{
				file.GetStream().CopyTo(stream);
				Model.ImageData = stream.ToArray();
			}
		}
	}
}