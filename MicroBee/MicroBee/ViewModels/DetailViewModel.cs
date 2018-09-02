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
		public MicroItem Item
		{
			get => _item;
			set
			{
				_item = value;
				OnPropertyChanged();
			}
		}

		public ImageSource ItemImage => ImageData == null ? null : ImageSource.FromStream(() => new MemoryStream(ImageData));
		private IList<ItemCategory> _categories;
		private MicroItem _item;
		private byte[] _imageData;

		public IList<ItemCategory> Categories
		{
			get => _categories;
			set
			{
				_categories = value;
				OnPropertyChanged();
			}
		}

		public bool IsUserOwner => Item != null && App.UserName == Item.OwnerName;
		public bool CanAccept => Item != null && !IsUserOwner && Item.WorkerName == null;



		public bool IsImageChanged { get; set; }

		public byte[] ImageData
		{
			get => _imageData;
			set
			{
				_imageData = value;
				OnPropertyChanged(nameof(ItemImage));
			}
		}

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
