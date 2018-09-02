using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MicroBee
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemDetailPage : ContentPage
	{
		public int ItemId { get; set; }
		public ItemDetailPage ()
		{
			InitializeComponent ();
		}

		protected override async void OnAppearing()
		{
			var item = await App.ItemService.GetMicroItemAsync(ItemId);
			Model.Item = item;

			if (item.ImageId != null)
			{
				byte[] imageData = await App.ItemService.GetImageAsync(item.ImageId.Value);
				Model.ImageData = imageData;
			}

			Model.UpdateUser();
		}
	}
}