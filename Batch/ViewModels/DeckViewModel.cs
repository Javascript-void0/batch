using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Batch.Database;
using Batch.Models;
using Batch.Views;

namespace Batch.ViewModels
{
	public class DeckViewModel : INotifyPropertyChanged
	{
		private static ObservableCollection<Deck> items, searchSubset, itemsCopy;
		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<Deck> Items
		{
			get
			{
				return items;
			}
			set
			{
				if (items != value)
				{
					items = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new PropertyChangedEventArgs("Items"));
				}
			}
		}

		public static int FindNextId()
		{
			// finds next open deck id
			var existingDecks = items;
			int i = 1;
			while (existingDecks.Any(x => x.Id == i)) // while id already exists
			{
				i++;
			}
			return i;
		}

		public DeckViewModel()
		{
			Items = new ObservableCollection<Deck>();
			LoadFromDBAsync();
		}

		public static void AddDeck(Deck deck)
		{
			items.Add(deck);
			if (itemsCopy != null && itemsCopy.Count != 0)
				itemsCopy.Add(deck);
			TDatabase.SaveDatabase(items.ToList());
		}

		public static void EditDeck(Deck deck, Deck newDeck)
		{
			for (var i = 0; i < items.Count; i++)
			{
				if (items[i].Equals(deck))
				{
					items[i] = newDeck;
					TDatabase.SaveDatabase(items.ToList());
					break;
				}
			}
		}

		public static void RemoveDeck(Deck deck)
		{
			//if (searchSubset == null)
			//{
			items.Remove(deck);
			if (itemsCopy != null && itemsCopy.Count != 0)
				itemsCopy.Remove(deck);
			TDatabase.SaveDatabase(items.ToList());
			//}
		}

		public async void SaveDatabase()
		{
			TDatabase.SaveDatabase(Items.ToList());
		}

		public async void LoadFromDBAsync()
		{
			Items.Clear();
			foreach (var deck in TDatabase.LoadDatabase())
			{
				Items.Add(deck);
			}

			// check deck id's
			foreach (var item in Items)
			{
				if (item.id <= 0)
				{
					item.id = FindNextId();
					TDatabase.SaveDatabase(Items.ToList());
				}
			}
		}

		public async void UpdateBasedOnSearch(String searchString, int sort)
		{
			if (itemsCopy == null)
			{
				itemsCopy = Items;
				searchSubset = new ObservableCollection<Deck>(Items);
			}
			else
				searchSubset = new ObservableCollection<Deck>(itemsCopy);

			if (String.IsNullOrEmpty(searchString) && sort == 1) // default
			{
				Items = itemsCopy;
				itemsCopy = null;
				searchSubset = new ObservableCollection<Deck>(Items);
			}

			// first remove without searchString
			if (!String.IsNullOrEmpty(searchString))
			{
				for (var i = searchSubset.Count - 1; i >= 0; i--)
				{
					var deck = searchSubset[i];

					// doesn't match any tags
					bool hasTag = false;
					foreach (var tag in deck.Tags)
					{
						if ((tag).ToLower().Contains(searchString.ToLower()))
						{
							hasTag = true;
						}
					}

					// doesnt match name or any tags
					if (!deck.Name.ToLower().Contains(searchString.ToLower()) && !hasTag)
						searchSubset.RemoveAt(i);
				}
			}


			// finish by sorting // TODO: Fix Sorting
			switch (sort)
			{
				case 0: // alphabetical
					Items = new ObservableCollection<Deck>(searchSubset.OrderBy(deck => deck.Name));
					break;
				case 1: // created
					Items = new ObservableCollection<Deck>(searchSubset.OrderBy(deck => DateTime.Parse(deck.Created)));
					break;
				case 2: // recent
					if (searchSubset.Count <= 1)
					{
						Items = searchSubset;
						break;
					}
					var recent = RecentViewModel.recentDecks;
					for (var i = recent.Count - 1; i >= 0; i--)
					{
						var deck = searchSubset.Where(x => x.id == recent[i].id).SingleOrDefault();
						if (deck != null)
						{
							searchSubset.Remove(deck);
							searchSubset.Insert(0, deck); // move to front
						}
					}
					Items = new ObservableCollection<Deck>(searchSubset);
					break;
			}
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}