using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroBee.Data;

using MicroBee.Data.Models;
using Xamarin.Forms;

namespace MicroBee
{
	public partial class MainPage : ContentPage
	{

		public MainPage()
		{
			InitializeComponent();
		}

		protected override async void OnAppearing()
		{
			await Model.Items.LoadMoreAsync();
			Model.Categories = await App.ItemService.GetCategoriesAsync();
		}
	}
}
