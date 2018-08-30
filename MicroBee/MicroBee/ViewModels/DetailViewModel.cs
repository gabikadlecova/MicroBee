using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using MicroBee.Data.Models;
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

	    public DetailViewModel(MicroItem item, Image itemImage)
	    {
		    Item = item;
		    ItemImage = itemImage;
	    }

		public ICommand EditCommand => new Command(async () =>
		{

		});
	    public ICommand SubmitEditCommand => new Command(async () =>
	    {

	    });

		public ICommand AcceptJobCommand => new Command(async () =>
		{

		});
	   

	    public event PropertyChangedEventHandler PropertyChanged;

	    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	    {
		    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	    }
    }
}
