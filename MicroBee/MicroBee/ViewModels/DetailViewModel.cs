using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MicroBee.Data.Models;
using Plugin.Media;
using Xamarin.Forms;

namespace MicroBee.ViewModels
{
	public class DetailViewModel : INotifyPropertyChanged
	{
		private bool isEditing;

		public MicroItem Item { get; set; }
		public ImageSource ItemImage => ImageData == null? null : ImageSource.FromStream(() => new MemoryStream(ImageData));
		private IList<ItemCategory> _categories;
		public IList<ItemCategory> Categories
		{
			get => _categories;
			set
			{
				_categories = value;
				OnPropertyChanged();
			}
		}

		public bool IsUserOwner => App.UserName == Item.OwnerName;
		public bool CanAccept => !IsUserOwner && Item.WorkerName == null;

		public bool IsEditing
		{
			get => isEditing;
			private set
			{
				isEditing = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsDetail));
			}
		}

		public bool IsDetail
		{
			get => !isEditing;
			private set => IsEditing = !value;
		}

		public bool IsImageChanged { get; private set; }
		public byte[] ImageData { get; set; }
		public ICommand EditCommand => new Command(execute: () => { IsEditing = true; });
		public ICommand SubmitEditCommand => new Command(execute: async () =>
		{
			if (IsImageChanged)
			{
				await App.ItemService.UpdateMicroItemAsync(Item, ImageData);
			}
			else
			{
				await App.ItemService.UpdateMicroItemAsync(Item, null);
			}

			Item = await App.ItemService.GetMicroItemAsync(Item.Id);

			if (Item.ImageId != null)
			{
				ImageData = await App.ItemService.GetImageAsync(Item.ImageId.Value);
				OnPropertyChanged(nameof(ItemImage));
			}

			IsImageChanged = false;
			IsEditing = false;
		});

		public ICommand AcceptJobCommand => new Command(execute: async () =>
		{
			if (App.IsUserAuthenticated)
			{
				await App.ItemService.SetAsWorkerAsync(Item.Id);
			}
		});

		public ICommand ChooseImageCommand => new Command(async () =>
		{
			var file = await CrossMedia.Current.PickPhotoAsync();
			if (file == null)
			{
				return;
			}

			using (MemoryStream stream = new MemoryStream())
			{
				file.GetStream().CopyTo(stream);
				ImageData = stream.ToArray();
			}
			OnPropertyChanged(nameof(ItemImage));

			IsImageChanged = true;
		});

		public void UpdateUser()
		{
			OnPropertyChanged(nameof(CanAccept));
			OnPropertyChanged(nameof(IsUserOwner));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		
	}
}
