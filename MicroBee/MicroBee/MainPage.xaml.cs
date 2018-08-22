﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroBee.Data;
using Xamarin.Forms;

namespace MicroBee
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		protected async override void OnAppearing()
		{
			var itemTask = App.Service.GetMicroItemsAsync();
			itemList.ItemsSource = await itemTask;
		}
	}
}
