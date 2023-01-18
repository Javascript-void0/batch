using Batch.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Batch.Models;
using Batch.Views;

namespace Batch
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeckFormPage : ContentPage
	{
		private int cardCount = 0;
		private Deck deck;
		public DeckFormPage()
		{
			InitializeComponent();
			cardCount = 1;
			deck = null;
			saveDeckButton.Source = ImageSource.FromResource("Batch.Images.device-floppy.png");

			if (Application.Current.Properties.ContainsKey("Option4"))
				if ((bool)Application.Current.Properties["Option4"])
					AbsoluteLayout.SetLayoutBounds(saveDeckButton, new Rectangle(0.05, 0.95, 80, 80));
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			// left-handed (option 4)
			AbsoluteLayout.SetLayoutBounds(saveDeckButton, new Rectangle(0.95, 0.95, 80, 80));
			if (Application.Current.Properties.ContainsKey("Option4"))
				if ((bool)Application.Current.Properties["Option4"])
					AbsoluteLayout.SetLayoutBounds(saveDeckButton, new Rectangle(0.05, 0.95, 80, 80));
		}

		public DeckFormPage(Deck deck)
		{
			InitializeComponent();
			cardCount = 1;
			this.deck = deck;
			titleDeckName.Text = "Editing: " + deck.Name;
			saveDeckButton.Source = ImageSource.FromResource("Batch.Images.device-floppy.png");

			deckName.Text = deck.Name;
			if (deck.Tags.Count > 0)
			{
				var conv = new ArrayListToStringConverter();
				deckTags.Text = (string) (conv.Convert(deck.Tags, null, null, null));
			}
			deleteDeckButton.IsVisible = true;

			for (var i = 0; i < deck.Cards.Count; i++)
			{
				if (i != 0)
					newCardClicked(null, null);
				var card = cards.Children[cards.Children.Count - 1] as Frame;
				var side1 = ((card.Content as StackLayout).Children[2] as StackLayout).Children[1] as Entry;
				var side2 = ((card.Content as StackLayout).Children[3] as StackLayout).Children[1] as Entry;
				side1.Text = (deck.Cards[i] as Card).Side1;
				side2.Text = (deck.Cards[i] as Card).Side2;
			}
		}

		protected override bool OnBackButtonPressed()
		{
			if (deckName.Text == null && deckTags.Text == null)
			{
				Navigation.PopAsync();
				return true;
			}
			Device.BeginInvokeOnMainThread(async () => {

				var result = await DisplayAlert("Warning", "Progress will not be saved", "Confirm", "Cancel");
				if (result) await Navigation.PopAsync();
			});
			return true;
		}

		private void deleteCardClicked(object sender, EventArgs e)
		{
			var button = sender as ImageButton;
			var cards = button.Parent.Parent.Parent.Parent as StackLayout;
			var card = button.Parent.Parent.Parent as Frame;
			cards.Children.Remove(card);
		}

		private async void DeleteDeckButtonClicked(object sender, EventArgs e)
		{
			var result = await DisplayAlert("Warning", "This action is permanent!", "Confirm", "Cancel");
			if (result)
			{
				DeckViewModel.RemoveDeck(deck);
				RecentViewModel.RemoveRecent(deck);
				RecentViewModel.SetRecentFiveDecks();
				FlyoutPage f = Application.Current.MainPage as FlyoutPage;
				ContentPage fl = f.Flyout as ContentPage;
				StackLayout s = fl.Content as StackLayout;
				ListView l = s.Children[1] as ListView;
				l.ItemsSource = RecentViewModel.recentFive;
				await Navigation.PopToRootAsync();
			}
		}

		private async void newCardClicked(object sender, EventArgs e)
		{
			var card = new Frame()
			{
				Margin = new Thickness(20, 0, 20, 20),
				HeightRequest = 200,
				CornerRadius = 10
			};

			var stack = new StackLayout();

			var top = new StackLayout() { Orientation = StackOrientation.Horizontal };
			var title = new Label()
			{
				Text = "Card " + (cardCount + 1),
				TextColor = (Color)Application.Current.Resources["BackgroundColor2"],
				FontSize = 20,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				FontFamily = "Jost"
			};
			top.Children.Add(title);
			var x = new ImageButton()
			{
				WidthRequest = 30,
				HeightRequest = 30,
				HorizontalOptions = LayoutOptions.End,
				Margin = new Thickness(10, 0),
				Padding = new Thickness(5),
				CornerRadius = 100,
				Source = ImageSource.FromResource("Batch.Images.x.png"),
				BackgroundColor = Color.White,
			};
			x.Clicked += deleteCardClicked;
			top.Children.Add(x);

			var separator = new BoxView() { Style = (Style)Application.Current.Resources["Separator"] };

			var side1 = new StackLayout() { Orientation = StackOrientation.Horizontal };
			side1.Children.Add(new Label() 
			{ 
				Text="Side1: ", 
				FontSize=20, 
				VerticalOptions=LayoutOptions.Center, 
				TextColor = (Color)Application.Current.Resources["BackgroundColor"], 
				FontAttributes=FontAttributes.Bold, 
				HorizontalOptions=LayoutOptions.Start,
				FontFamily = "JostBold"
			});
			side1.Children.Add(new Entry()
			{
				Placeholder = "Word",
				TextColor = (Color)Application.Current.Resources["BackgroundColor"],
				PlaceholderColor = Color.Gray,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				FontFamily = "Jost"
			});
			var side2 = new StackLayout() { Orientation = StackOrientation.Horizontal };
			side2.Children.Add(new Label()
			{
				Text = "Side2: ",
				FontSize = 20,
				VerticalOptions = LayoutOptions.Center,
				TextColor = (Color)Application.Current.Resources["BackgroundColor"],
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Start,
				FontFamily = "JostBold"
			});
			side2.Children.Add(new Entry()
			{
				Placeholder = "Definition",
				TextColor = (Color)Application.Current.Resources["BackgroundColor"],
				PlaceholderColor = Color.Gray,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				FontFamily = "Jost"
			});

			stack.Children.Add(top);
			stack.Children.Add(separator);
			stack.Children.Add(side1);
			stack.Children.Add(side2);
			card.Content = stack;

			cards.Children.Add(card);
			cardCount++;
			side1.Children[1].Focus(); // focus cursor on first entry of new card
			await scrollView.ScrollToAsync(bottom, ScrollToPosition.End, true);
		}

		private async void saveButtonClicked(object sender, EventArgs e)
		{
			Deck d;
			// edit existing deck or create new deck
			if (deck == null)
			{
				d = new Deck();
				d.Created = String.Format("{0:MM.dd.yy}", DateTime.Now);
				d.id = DeckViewModel.FindNextId();
			}
			else
				d = deck;
			
			// set name
			if (deckName.Text != null)
				d.Name = deckName.Text;
			else
				d.Name = "New Deck"; // default name

			// add tags
			d.Tags.Clear();
			if (deckTags.Text != null)
			{
				var tags = deckTags.Text;
				tags = tags.Replace(", ", ",");
				var tagsList = tags.Split(',');
				foreach (var tag in tagsList)
				{
					if (!string.IsNullOrEmpty(tag))
					{
						d.AddTag(tag, false);
					}
				}
			}

			// add cards
			d.Cards.Clear();
			foreach (Frame card in cards.Children)
			{
				var side1 = ((card.Content as StackLayout).Children[2] as StackLayout).Children[1] as Entry;
				var side2 = ((card.Content as StackLayout).Children[3] as StackLayout).Children[1] as Entry;
				if (side1.Text != null && side2.Text != null)
					d.AddCard(new Card(side1.Text, side2.Text));
			}

			await Navigation.PushAsync(new DeckDetailView(d)); // open deck detail page
			if (deck == null)
			{
				DeckViewModel.AddDeck(d);
				Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]); // close deck form page (this)
			}
			else
			{
				DeckViewModel.EditDeck(deck, d);
				Navigation.RemovePage(Navigation.NavigationStack[1]);
				Navigation.RemovePage(Navigation.NavigationStack[1]);
			}
		}

	}
}