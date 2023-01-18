using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using Batch.Models;
using Batch.Views;
using Xamarin.Forms;

namespace Batch.ViewModels
{
	public class RecentViewModel
	{
		public static List<Deck> recentDecks, recentFive;

		public RecentViewModel()
		{
			if (Application.Current.Properties.ContainsKey("Recent"))
			{
				var jsonString = (string)Application.Current.Properties["Recent"];
				recentDecks = JsonSerializer.Deserialize<List<Deck>>(jsonString);
			}
			else
			{
				recentDecks = new List<Deck>();
			}
		}

		public static void RecentAccessed(Deck deck)
		{
			// remove existing
			RemoveRecent(deck);

			// move isntance to front
			recentDecks.Insert(0, deck);
			Application.Current.Properties["Recent"] = JsonSerializer.Serialize(recentDecks);
			Console.WriteLine(recentDecks);

		}


		public static void SetRecentFiveDecks()
		{
			if (recentDecks.Count > 5)
				recentFive = new List<Deck>(recentDecks.GetRange(0, 5));
			else
				recentFive = new List<Deck>(recentDecks);
		}

		public static void RemoveRecent(Deck deck)
		{
			for (int i = recentDecks.Count - 1; i >= 0; i--)
			{
				if (recentDecks[i].id == deck.id)
				{
					recentDecks.RemoveAt(i);
					Application.Current.Properties["Recent"] = JsonSerializer.Serialize(recentDecks);
				}
			}
		}

		// TODO: edit deck (rename)
	}
}
