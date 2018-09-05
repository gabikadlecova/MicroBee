using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MicroBee.ViewModels.Converters
{
	class CountZeroConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType != typeof(int))
			{
				throw new InvalidOperationException();
			}

			int intVal = (int)value;
			return intVal == 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new InvalidOperationException("Converter is only one-way.");
		}
	}
}
