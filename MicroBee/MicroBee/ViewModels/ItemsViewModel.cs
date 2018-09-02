using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using MicroBee.Data.Models;
using Xamarin.Forms;

namespace MicroBee.ViewModels
{
	class ItemsViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public InfiniteItemCollection Items { get; }
		
		public ItemCategory SelectedCategory
		{
			get => Items.Category;
			set
			{
				Items.Category = value;
				OnPropertyChanged();
			}
		}

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

		public ItemsViewModel()
		{
			Items = new InfiniteItemCollection(App.ItemService);
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private bool _isRefreshing = false;
		public bool IsRefreshing
		{
			get => _isRefreshing;
			set
			{
				_isRefreshing = value;
				OnPropertyChanged();
			}
		}
		public ICommand RefreshCommand => new Command(async () =>
		{
			IsRefreshing = true;

			Items.Reset();

			IsRefreshing = false;
		});
	}
}
