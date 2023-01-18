using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Batch.ViewModels
{
	public class ArrayListToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = "";
			List<string> arr = (List<string>)value;
			if (arr.Count == 0) return "None";
			for (int i = 0; i < arr.Count; i++)
			{
				result += "" + arr[i];
				if (i < arr.Count - 1)
					result += ", ";
			}
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
