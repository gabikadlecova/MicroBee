using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MicroBee
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemMasterDetailPageMaster : ContentPage
	{
		public ListView ListView => this.listView;
		public ItemMasterDetailPageMaster()
		{
			InitializeComponent();

			BindingContext = new ItemMasterDetailPageMasterViewModel();
		}

		class ItemMasterDetailPageMasterViewModel : INotifyPropertyChanged
		{
			public ObservableCollection<ItemMasterDetailPageMenuItem> MenuItems { get; set; }

			public ItemMasterDetailPageMasterViewModel()
			{
				MenuItems = new ObservableCollection<ItemMasterDetailPageMenuItem>()
				{
					new ItemMasterDetailPageMenuItem { Title = "Job list", TargetType = typeof(ItemsPage)},
					new ItemMasterDetailPageMenuItem { Title = "Created jobs", TargetType = typeof(UserItemsPage)},
					new ItemMasterDetailPageMenuItem { Title = "Accepted jobs", TargetType = typeof(AcceptedItemsPage)},
					new ItemMasterDetailPageMenuItem { Title = "User profile", TargetType = typeof(ProfilePage)}

				};
			}

			#region INotifyPropertyChanged Implementation
			public event PropertyChangedEventHandler PropertyChanged;
			void OnPropertyChanged([CallerMemberName] string propertyName = "")
			{
				if (PropertyChanged == null)
					return;

				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
			#endregion
		}
	}
}