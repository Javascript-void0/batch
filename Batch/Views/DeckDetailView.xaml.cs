using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Batch.Models;
using Batch.ViewModels;
using Batch.Database;

namespace Batch.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeckDetailView : ContentPage
	{
		public Deck deck;
		private CardViewModel cardViewModel { get; set; }
		private int sort;
		public DeckDetailView(Deck deck)
		{
			InitializeComponent();
			this.deck = deck;
			SetTitle();

			cardViewModel = new CardViewModel(deck);
			cardsList.BindingContext = cardViewModel;
			sort = 0;
		}

		private void SetTitle()
		{
			titleDeckName.Text = deck.Name;
			var cardCount = deck.Cards.Count;
			if (cardCount == 1)
				numCards.Text = "(1) Card";
			else
				numCards.Text = "(" + cardCount + ") Cards";
		}

		private async void RenameClicked(object sender, EventArgs e)
		{
			var newName = await DisplayPromptAsync("Rename", "New Name: ", initialValue: deck.Name);
			if (newName != null && newName != "")
			{
				deck.Name = newName;
				SetTitle();
				HomePage.deckViewModel.SaveDatabase();
			}
		}

		private async void EditClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new DeckFormPage(deck));
		}

		private async void TagsClicked(object sender, EventArgs e)
		{
			ArrayListToStringConverter conv = new ArrayListToStringConverter();
			string newTags;
			if (deck.Tags != null)
				newTags = await DisplayPromptAsync("Tags", "Edit Tags: ", initialValue: (string)conv.Convert(deck.Tags, null, null, null));
			else
				newTags = await DisplayPromptAsync("Tags", "Edit Tags: ");
			deck.Tags.Clear();
			if (newTags != null && newTags != "")
			{
				newTags = newTags.Replace(", ", ",");
				var tagsList = newTags.Split(',');
				foreach (var tag in tagsList)
				{
					if (!string.IsNullOrEmpty(tag))
					{
						deck.AddTag(tag, true);
					}
				}
				HomePage.deckViewModel.SaveDatabase();
			}
		}

		private async void ReviewClicked(object sender, EventArgs e)
		{
			bool shuffle = true;
			if (!(bool)Application.Current.Properties["Option1"]) // if always shuffle == true, no action sheet
			{
				var order = await DisplayActionSheet("Deck Order: ", "Cancel", null, "Default", "Shuffle");
				if (order == "Default")
					shuffle = false;
				else if (order == "Shuffle")
					shuffle = true;
				else
					return;
			}

			var start = await DisplayActionSheet("Starting Side: ", "Cancel", null, "Side 1", "Side 2");
			bool flip;
			if (start == "Side 1")
				flip = false;
			else if (start == "Side 2")
				flip = true;
			else
				return;

			await Navigation.PushAsync(new DeckReviewPage(deck, shuffle, flip));
		}

		private void CardSearchChanged(object sender, TextChangedEventArgs e)
		{
			cardViewModel.UpdateBasedOnSearch(e.NewTextValue, sort);
		}

		private async void SortClicked(object sender, EventArgs e)
		{
			var result = await DisplayActionSheet("Sort By: ", "Cancel", null, "None", "Side 1", "Side 2");
			if (result == "None")
				sort = 0;
			else if (result == "Side 1")
				sort = 1;
			else if (result == "Side 2")
				sort = 2;
			cardViewModel.UpdateBasedOnSearch(searchBar.Text, sort);
		}
	}
}