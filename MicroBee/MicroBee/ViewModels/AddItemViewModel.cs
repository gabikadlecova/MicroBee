using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using MicroBee.Data.Models;
using Xamarin.Forms;

namespace MicroBee.ViewModels
{
	class AddItemViewModel : INotifyPropertyChanged
	{
		private ObservableCollection<ItemCategory> _categories;
		private byte[] _imageData;
		private MicroItem _item;

		public ImageSource ImageSource => ImageData == null ? null : ImageSource.FromStream(() => new MemoryStream(ImageData));

		public MicroItem Item
		{
			get => _item;
			set
			{
				_item = value;
				OnPropertyChanged();
			}
		}

		public byte[] ImageData
		{
			get => _imageData;
			set
			{
				_imageData = value;
				OnPropertyChanged(nameof(ImageSource));
			}
		}

		public ObservableCollection<ItemCategory> Categories
		{
			get => _categories;
			set
			{
				_categories = value;
				OnPropertyChanged();
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
