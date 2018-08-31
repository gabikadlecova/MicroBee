using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroBee.Data.Models;
using MicroBee.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MicroBee
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemDetail : CarouselPage
	{
		public ObservableCollection<DetailViewModel> Pages { get; set; }
		public ItemDetail ()
		{
			InitializeComponent ();
		}

		protected override void OnAppearing()
		{
			BindingContext = this;
		}
	}
}