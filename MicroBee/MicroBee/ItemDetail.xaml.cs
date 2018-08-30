using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroBee.Data.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MicroBee
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemDetail : ContentPage
	{
		public MicroItem Item { get; }
		public Image ItemImage { get; }

		public bool IsUserOwner => App.UserName == Item.OwnerName;

		public ItemDetail ()
		{
			InitializeComponent ();
		}
	}
}