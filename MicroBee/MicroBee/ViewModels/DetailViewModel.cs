using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using MicroBee.Data;
using MicroBee.Data.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace MicroBee.ViewModels
{
	public class DetailViewModel : INotifyPropertyChanged
	{
		public MicroItem Item { get; }
		public Image ItemImage { get; }
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

		public bool IsEditing { get; private set; }

		public bool IsDetail
		{
			get => !IsEditing;
			set => IsEditing = !value;
		}

		public bool IsImageChanged { get; set; }
		public ICommand EditCommand => new Command(() => { IsEditing = true; });
		public ICommand SubmitEditCommand => new Command(async () =>
		{

			//await App.ItemService.UpdateMicroItemAsync(Item,);

			IsImageChanged = false;
			IsEditing = false;
		});

		public ICommand AcceptJobCommand => new Command(async () =>
		{

		});

		public ICommand ChooseImageCommand => new Command(async () =>
		{

			var file = await CrossMedia.Current.PickPhotoAsync();

			IsImageChanged = true;
		});

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
