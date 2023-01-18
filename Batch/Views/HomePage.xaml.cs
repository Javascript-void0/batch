using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Batch.Views;
using System.Collections;
using Batch.Models;
using Batch.ViewModels;
using System.Text.RegularExpressions;
using static Xamarin.Essentials.AppleSignInAuthenticator;

namespace Batch.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public static DeckViewModel deckViewModel { get; set; }
		private int sort;

		public HomePage()
		{
			InitializeComponent();
			
			deckViewModel = new DeckViewModel();
			var recentViewModel = new RecentViewModel();
			RecentViewModel.SetRecentFiveDecks();
			
			BindingContext = deckViewModel;
			
			if (Application.Current.Properties.ContainsKey("Sort"))
				sort = (int)Application.Current.Properties["Sort"];
			else
				sort = 1;
			deckViewModel.UpdateBasedOnSearch(searchBar.Text, sort);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			// left-handed (option 4)
			AbsoluteLayout.SetLayoutBounds(newDeckButton, new Rectangle(0.95, 0.95, 80, 80));
			if (Application.Current.Properties.ContainsKey("Option4"))
				if ((bool)Application.Current.Properties["Option4"])
					AbsoluteLayout.SetLayoutBounds(newDeckButton, new Rectangle(0.05, 0.95, 80, 80));

			// update count in top-right
			numDecks.Text = "(" + deckViewModel.Items.Count + ") Decks";
			if (deckViewModel.Items.Count == 1)
				numDecks.Text = "(1) Deck";
			if (deckViewModel.Items.Count == 0)
				noDecks.IsVisible = true;
			else
				noDecks.IsVisible = false;

			// update recent deck order
			deckViewModel.UpdateBasedOnSearch(searchBar.Text, sort);
		}

		private void New_Deck_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new DeckFormPage());
		}

		private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var deck = e.Item as Deck;
			decksList.SelectedItem = null;
			Navigation.PushAsync(new DeckDetailView(deck));
		}

		private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			deckViewModel.UpdateBasedOnSearch(e.NewTextValue, sort);
		}

		private async void dotsClicked(object sender, EventArgs e)
		{
			var result = await DisplayActionSheet("Sort By: ", "Cancel", null, "Alphabetical", "Created", "Recent");
			if (result == "Alphabetical")
				sort = 0;
			else if (result == "Created")
				sort = 1;
			else if (result == "Recent")
				sort = 2;
			Application.Current.Properties["Sort"] = sort;
			deckViewModel.UpdateBasedOnSearch(searchBar.Text, sort);
		}

		private async void shuffleClicked(object sender, EventArgs e)
		{
			var item = ((ImageButton)sender).BindingContext as Deck;
			await Navigation.PushAsync(new DeckReviewPage(item, true, false));
		}
	}
}