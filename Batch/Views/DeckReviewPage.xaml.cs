using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Batch.Models;
using Batch.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TouchTracking;

namespace Batch.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeckReviewPage : ContentPage
	{
		private List<Card> cards;

		public DeckReviewPage(Deck deck, bool shuffle, bool flip)
		{
			InitializeComponent();
			cards = new List<Card>(deck.Cards);
			if (shuffle)
				Shuffle();
			if (flip)
				cardCarousel.ItemTemplate = (DataTemplate)Resources["Flip"];
			cardCarousel.ItemsSource = cards;

			// Add deck to recent list
			RecentViewModel.RecentAccessed(deck);
			// Update recents in flyout(manually)._.
			RecentViewModel.SetRecentFiveDecks();
			FlyoutPage f = Application.Current.MainPage as FlyoutPage;
			ContentPage fl = f.Flyout as ContentPage;
			StackLayout s = fl.Content as StackLayout;
			ListView l = s.Children[1] as ListView;
			l.ItemsSource = RecentViewModel.recentFive;

			// TODO: Add functionality/ui
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			// landscape mode (option 3)
			if (Application.Current.Properties.ContainsKey("Option3"))
				if ((bool)Application.Current.Properties["Option3"])
				{
					MessagingCenter.Send(this, "AllowLandscape");

					// left-handed (option 4)
					if (Application.Current.Properties.ContainsKey("Option4"))
						if ((bool)Application.Current.Properties["Option4"])
							grid.FlowDirection = FlowDirection.RightToLeft;
				}
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			MessagingCenter.Send(this, "PreventLandscape");
		}

		private void FlipCard(object sender, EventArgs e)
		{
			//var side1 = (cardCarousel.CurrentItem as Card).Side1;
			//var side2 = (cardCarousel.CurrentItem as Card).Side2;
			//(cardCarousel.CurrentItem as Card).Side1 = side2;
			//(cardCarousel.CurrentItem as Card).Side2 = side1;

			//cardCarousel.ItemsSource = cards;

			//StackLayout s1 = ((StackLayout)((Frame)sender).Content).Children[0] as StackLayout;
			//StackLayout s2 = ((StackLayout)((Frame)sender).Content).Children[1] as StackLayout;

			// if () // TODO: help

			//Console.WriteLine(sender);
			//var frame = sender as Frame;
			//var deck = frame.BindingContext as Deck;
			//Console.WriteLine(deck.Name);

			Console.WriteLine(((TappedEventArgs)e).Parameter);
			Console.WriteLine(sender);
			Console.WriteLine(((StackLayout)((Frame)sender).Content).Children);
			StackLayout s1 = ((StackLayout)((Frame)sender).Content).Children[0] as StackLayout;
			StackLayout s2 = ((StackLayout)((Frame)sender).Content).Children[1] as StackLayout;
			s1.IsVisible = !s1.IsVisible;
			s2.IsVisible = !s2.IsVisible;
		}

		private void CardChanged(object sender, CurrentItemChangedEventArgs e)
		{
			// https://stackoverflow.com/a/62703839
			inProgressPaths.Clear();
			completedPaths.Clear();
			canvasView.InvalidateSurface();
		}

		private void Shuffle()
		{
			Random r = new Random();
			for (int i = cards.Count - 1; i > 0; i--)
			{
				int j = r.Next(i + 1);
				Card temp = cards[i];
				cards[i] = cards[j];
				cards[j] = temp;
			}
		}

		// canvas

		Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();
		List<SKPath> completedPaths = new List<SKPath>();

		SKPaint paint = new SKPaint
		{
			Style = SKPaintStyle.Stroke,
			Color = SKColor.Parse(ExtensionMethods.GetHexString((Color)Application.Current.Resources["BackgroundColor2"])),
			StrokeWidth = (float)((double)Application.Current.Properties["PenSize"]),
			StrokeCap = SKStrokeCap.Round,
			StrokeJoin = SKStrokeJoin.Round
		};

		private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
		{
			switch (args.Type)
			{
				case TouchActionType.Pressed:
					if (!inProgressPaths.ContainsKey(args.Id))
					{
						SKPath path = new SKPath();
						path.MoveTo(ConvertToPixel(args.Location));
						inProgressPaths.Add(args.Id, path);
						canvasView.InvalidateSurface();
					}
					break;

				case TouchActionType.Moved:
					if (inProgressPaths.ContainsKey(args.Id))
					{
						SKPath path = inProgressPaths[args.Id];
						path.LineTo(ConvertToPixel(args.Location));
						canvasView.InvalidateSurface();
					}
					break;

				case TouchActionType.Released:
					if (inProgressPaths.ContainsKey(args.Id))
					{
						completedPaths.Add(inProgressPaths[args.Id]);
						inProgressPaths.Remove(args.Id);
						canvasView.InvalidateSurface();
					}
					break;

				case TouchActionType.Cancelled:
					if (inProgressPaths.ContainsKey(args.Id))
					{
						inProgressPaths.Remove(args.Id);
						canvasView.InvalidateSurface();
					}
					break;
			}
		}
		private SKPoint ConvertToPixel(TouchTrackingPoint pt)
		{
			return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
							   (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
		}

		void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
		{
			SKCanvas canvas = args.Surface.Canvas;
			canvas.Clear();

			foreach (SKPath path in completedPaths)
			{
				canvas.DrawPath(path, paint);
			}

			foreach (SKPath path in inProgressPaths.Values)
			{
				canvas.DrawPath(path, paint);
			}
		}
	}
}