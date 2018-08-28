using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using MicroBee.Annotations;

namespace MicroBee.ViewModels
{
    class ItemsViewModel : INotifyPropertyChanged
    {
	    public event PropertyChangedEventHandler PropertyChanged;

	    public ItemsViewModel()
	    {

	    }

	    [NotifyPropertyChangedInvocator]
	    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	    {
		    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	    }
    }
}
