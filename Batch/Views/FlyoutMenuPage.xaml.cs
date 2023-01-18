using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Batch.ViewModels;
using Batch.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Batch.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FlyoutMenuPage : ContentPage
	{
		public FlyoutMenuPage()
		{
			InitializeComponent();
		}
	}
}