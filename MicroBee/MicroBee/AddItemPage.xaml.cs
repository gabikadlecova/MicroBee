using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		}

		protected override async void OnAppearing()
		{

		}

		private async void SubmitButton_OnClicked(object sender, EventArgs e)
		{
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