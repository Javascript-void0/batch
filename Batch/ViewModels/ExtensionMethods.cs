﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Batch.ViewModels
{
	// https://stackoverflow.com/a/43532125
	public static class ExtensionMethods
	{
		public static string GetHexString(this Xamarin.Forms.Color color)
		{
			var red = (int)(color.R * 255);
			var green = (int)(color.G * 255);
			var blue = (int)(color.B * 255);
			var alpha = (int)(color.A * 255);
			var hex = $"#{alpha:X2}{red:X2}{green:X2}{blue:X2}";

			return hex;
		}
	}
}
