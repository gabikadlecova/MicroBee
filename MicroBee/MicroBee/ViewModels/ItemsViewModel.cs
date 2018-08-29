using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using MicroBee.Data.Models;
using Xamarin.Forms;

namespace MicroBee.ViewModels
{
    class ItemsViewModel : INotifyPropertyChanged
    {
	    public event PropertyChangedEventHandler PropertyChanged;
		
		public InfiniteItemCollection Items { get; }

	    public ItemsViewModel()
	    {
		    Items = new InfiniteItemCollection(App.Service);
	    }
		
	    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	    {
		    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	    }
    }
}
