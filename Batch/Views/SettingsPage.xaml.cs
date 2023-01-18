using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Batch.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		private int theme = 5;
		public SettingsPage()
		{
			InitializeComponent();
			LoadSettings();

			BindingContext = Application.Current;
		}

		protected override void OnDisappearing()
		{
			LoadSettings();
		}

		public void LoadSettings()
		{
			if (Application.Current.Properties.ContainsKey("Option1"))
				option1.IsChecked = (bool)Application.Current.Properties["Option1"];
			if (Application.Current.Properties.ContainsKey("Option2"))
				option2.IsChecked = (bool)Application.Current.Properties["Option2"];
			if (Application.Current.Properties.ContainsKey("Option3"))
				option3.IsChecked = (bool)Application.Current.Properties["Option3"];
			if (Application.Current.Properties.ContainsKey("Option4"))
				option4.IsChecked = (bool)Application.Current.Properties["Option4"];
			if (Application.Current.Properties.ContainsKey("PenSize"))
				penSizeSlider.Value = (double)Application.Current.Properties["PenSize"];
			if (Application.Current.Properties.ContainsKey("Theme"))
			{
				theme = (int)Application.Current.Properties["Theme"];
				SetTheme(theme);
			}

			saveButton.Text = "Save";
		}

		private void SaveSettings(object sender, EventArgs e)
		{
			saveButton.Text = "Save";
			Application.Current.Properties["Option1"] = option1.IsChecked;
			Application.Current.Properties["Option2"] = option2.IsChecked;
			Application.Current.Properties["Option3"] = option3.IsChecked;
			Application.Current.Properties["Option4"] = option4.IsChecked;
			Application.Current.Properties["PenSize"] = penSizeSlider.Value;
			Application.Current.Properties["Theme"] = theme;
		}

		private void DefaultSettings(object sender, EventArgs e)
		{
			saveButton.Text = "Save";
			option1.IsChecked = false;
			option2.IsChecked = false;
			option3.IsChecked = false;
			option4.IsChecked = false;
			penSizeSlider.Value = 10;
			SetTheme(5);
		}

		private void ThemeChanged(object sender, EventArgs e)
		{
			// https://stackoverflow.com/a/53281133
			Button b = (Button) sender;
			theme = Int32.Parse(b.BindingContext as string);
			SetTheme(theme);
			saveButton.Text = "Save*";
		}

		private void SetTheme(int theme)
		{
			Application.Current.Resources["BackgroundColor"] = Application.Current.Resources["Theme" + theme];
			Application.Current.Resources["BackgroundColor2"] = Application.Current.Resources["Theme" + theme + "2"];
			Application.Current.Resources["TextColor"] = Application.Current.Resources["Theme" + theme + "Text"];
			Application.Current.Resources["ShuffleIcon"] = Application.Current.Resources["Theme" + theme + "Shuffle"];
			Application.Current.Resources["EditIcon"] = Application.Current.Resources["Theme" + theme + "Edit"];
			Application.Current.Resources["SortIcon"] = Application.Current.Resources["Theme" + theme + "Sort"];
		}

		private void OptionChanged(object sender, ValueChangedEventArgs e)
		{
			saveButton.Text = "Save*";
		}
	}
}