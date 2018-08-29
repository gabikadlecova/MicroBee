using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
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
		public int PageSize { get; set; }

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

			PageSize = 10;
			CanLoadMore = true;
		}

		public void Reset()
		{
			_currentPage = 0;
			Items.Clear();
		}

		public async Task LoadMoreAsync()
		{
			IsLoadingMore = true;
			LoadingMore?.Invoke(this, new LoadingMoreEventArgs(true));

			var nextItems = await _service.GetMicroItemsAsync(_currentPage, PageSize);
			foreach (var item in nextItems)
			{
				Image image = null;
				if (item.ImageId != null)
				{
					byte[] imageData = await _service.GetImageAsync(item.ImageId.Value);
					image = new Image { Source = ImageSource.FromStream(() => new MemoryStream(imageData)) };
				}

				Add(new InfiniteItemElement() { Item = item, ItemImage = image });
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
	struct InfiniteItemElement
	{
		public MicroItem Item { get; set; }
		public Image ItemImage { get; set; }
	}
}
