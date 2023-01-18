using Batch.Views;
using Batch.Database;
using Batch.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batch.ViewModels
{
	public class CardViewModel : INotifyPropertyChanged
	{
		private ObservableCollection<Card> cards, searchSubset, itemsCopy;
		private Deck deck;
		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<Card> Cards
		{
			get
			{
				return cards;
			}
			set
			{
				if (cards != value)
				{
					cards = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new PropertyChangedEventArgs("Cards"));
				}
			}
		}

		public CardViewModel(Deck deck)
		{
			Cards = new ObservableCollection<Card>();
			this.deck = deck;
			LoadFromDBAsync();
		}

		public void AddDeck(Card card)
		{
			if (card != null && searchSubset == null)
			{
				Cards.Add(card);
				HomePage.deckViewModel.SaveDatabase();
			}
		}

		public void RemoveDeck(Card card)
		{
			if (searchSubset == null)
			{
				Cards.Remove(card);
				HomePage.deckViewModel.SaveDatabase();
			}
		}

		public async void LoadFromDBAsync()
		{
			Cards.Clear();
			foreach (var card in deck.Cards)
			{
				Cards.Add(card);
			}
		}

		public async void UpdateBasedOnSearch(String searchString, int sort)
		{
			if (itemsCopy == null)
			{
				itemsCopy = Cards;
				searchSubset = new ObservableCollection<Card>(Cards);
			}
			else
				searchSubset = new ObservableCollection<Card>(itemsCopy);

			// first remove without searchString
			if (!String.IsNullOrEmpty(searchString))
			{
				for (var i = searchSubset.Count - 1; i >= 0; i--)
				{
					var deck = searchSubset[i];
					if (!deck.Side1.ToLower().Contains(searchString.ToLower()) &&
						!deck.Side2.ToLower().Contains(searchString.ToLower()))
						searchSubset.RemoveAt(i);
				}
			}

			// finish by sorting
			switch (sort)
			{
				case 0: // none
					Cards = new ObservableCollection<Card>(searchSubset);
					break;
				case 1: // side 1
					Cards = new ObservableCollection<Card>(searchSubset.OrderBy(card => card.Side1));
					break;
				case 2: // side 2
					Cards = new ObservableCollection<Card>(searchSubset.OrderBy(card => card.Side2));
					break;
			}
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
