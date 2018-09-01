using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicroBee.Data;
using MicroBee.Data.Models;
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace MicroBee.ViewModels
{
	class InfiniteItemCollection : ObservableCollection<InfiniteItemElement>, IInfiniteScrollLoader, IInfiniteScrollLoading
	{
		private readonly IMicroItemService _service;
		private int _currentPage;

		private int _pageSize;
		public int PageSize
		{
			get => _pageSize;
			set
			{
				_pageSize = value;
				Reset();
			}
		}

		private ItemCategory _category;

		public ItemCategory Category
		{
			get => _category;
			set
			{
				_category = value;
				Reset();
			}
		}

		public InfiniteItemCollection(IMicroItemService service)
		{
			_service = service;
			_pageSize = 10;
			CanLoadMore = true;
		}

		public async void Reset()
		{
			_currentPage = 0;
			ClearItems();
			CanLoadMore = true;
			await LoadMoreAsync();
		}

		public async Task LoadMoreAsync()
		{
			IsLoadingMore = true;
			LoadingMore?.Invoke(this, new LoadingMoreEventArgs(true));

			List<MicroItem> nextItems = null;
			if (Category != null)
			{
				nextItems = await _service.GetMicroItemsAsync(_currentPage, PageSize, Category.Name);
			}
			else
			{
				nextItems = await _service.GetMicroItemsAsync(_currentPage, PageSize);
			}

			foreach (var item in nextItems)
			{
				ImageSource imageData = null;
				if (item.ImageId != null)
				{
					var imageBytes = await _service.GetImageAsync(item.ImageId.Value);
					
					imageData = ImageSource.FromStream(() => new MemoryStream(imageBytes));
					
				}

				Add(new InfiniteItemElement() { Item = item, ItemImage = imageData });
			}

			_currentPage++;
			if (nextItems.Count < PageSize)
			{
				CanLoadMore = false;
			}

			IsLoadingMore = false;
			LoadingMore?.Invoke(this, new LoadingMoreEventArgs(false));
		}

		public bool CanLoadMore { get; private set; }
		public bool IsLoadingMore { get; private set; }
		public event EventHandler<LoadingMoreEventArgs> LoadingMore;
	}
	class InfiniteItemElement
	{
		public MicroItem Item { get; set; }
		public ImageSource ItemImage { get; set; }
	}
}
