using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using MicroBee.Data.Models;

namespace MicroBee.ViewModels
{
    class UserProfileViewModel : INotifyPropertyChanged
    {
	    private UserProfile _profile;

	    public UserProfile Profile
	    {
		    get => _profile;
		    set
		    {
			    _profile = value;
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
