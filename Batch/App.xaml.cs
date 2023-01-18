using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Batch.Views;
using Batch.Database;
using Xamarin.Forms.PlatformConfiguration;

namespace Batch
{
    public partial class App : Application
    {
        private const String Option1Key = "Option1";
        private const String Option2Key = "Option2";
        private const String Option3Key = "Option3";
        private const String Option4Key = "Option4";
        private const String PenSizeKey = "PenSize";
		private const String DeckSortKey = "DeckSort";

		public App()
		{
			InitializeComponent();
			//MainPage = new NavigationPage(new MainPage()) { BarBackgroundColor = Color.Transparent };
			MainPage = new MainPage();
		}

		protected override void OnStart()
		{
			var settings = new SettingsPage();
			settings.LoadSettings();
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}

		public bool Option1
        {
            get
            {
                if (Properties.ContainsKey(Option1Key))
                    return (bool)Properties[Option1Key];
                return false;
            }
            set
            {
                Properties[Option1Key] = value;
            }
        }

		public bool Option2
		{
			get
			{
				if (Properties.ContainsKey(Option2Key))
					return (bool)Properties[Option2Key];
				return false;
			}
			set
			{
				Properties[Option2Key] = value;
			}
		}

		public bool Option3
		{
			get
			{
				if (Properties.ContainsKey(Option3Key))
					return (bool)Properties[Option3Key];
				return false;
			}
			set
			{
				Properties[Option3Key] = value;
			}
		}

		public bool Option4
		{
			get
			{
				if (Properties.ContainsKey(Option4Key))
					return (bool)Properties[Option4Key];
				return false;
			}
			set
			{
				Properties[Option4Key] = value;
			}
		}

		public double PenSize
		{
			get
			{
				if (Properties.ContainsKey(PenSizeKey))
					return (double)Properties[PenSizeKey];
				return 10.0;
			}
			set
			{
				Properties[PenSizeKey] = value;
			}
		}
	}
}
