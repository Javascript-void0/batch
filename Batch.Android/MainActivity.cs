using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Views;
using Xamarin.Forms;
using Batch.Views;

namespace Batch.Droid
{
    [Activity(Label = "Batch", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            // transparent status bar : https://stackoverflow.com/a/59377978
            Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);
            Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            Window.SetStatusBarColor(Android.Graphics.Color.Transparent);

			Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

			// https://heartbeat.comet.ml/force-an-orientation-on-a-single-page-in-xamarin-forms-b9c0c5295367
			MessagingCenter.Subscribe<DeckReviewPage>(this, "AllowLandscape", sender =>
			{
				RequestedOrientation = ScreenOrientation.Landscape;
			});

			MessagingCenter.Subscribe<DeckReviewPage>(this, "PreventLandscape", sender =>
			{
				RequestedOrientation = ScreenOrientation.Portrait;
			});

			LoadApplication(new App());
		}
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}