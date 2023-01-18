using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Batch.Models;
using Batch.Views;
using Batch.ViewModels;
using Xamarin.Forms.Xaml;
using System.Text.Json;

namespace Batch
{
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
		{
            InitializeComponent();

			flyoutPage.listView.ItemTapped += OnItemTapped;
			flyoutPage.settings.ItemTapped += OnItemTapped;
			flyoutPage.recentDecks.ItemTapped += OnRecentTapped;

			var recentViewModel = new RecentViewModel();
			flyoutPage.recentDecks.ItemsSource = RecentViewModel.recentFive;
		}

		void OnItemTapped(object sender, ItemTappedEventArgs e)
		{
			var item = e.Item as FlyoutPageItem;
			var newPage = (Page)Activator.CreateInstance(item.TargetType);
			Console.WriteLine(newPage.Title);
			var curr = Detail.Navigation.NavigationStack.LastOrDefault();
			// home goes back to root
			if (newPage.Title == "Home")
			{
				navigationPage.PopToRootAsync();
			}
			// other pages pushed on stack, pressing again doesn't create new instance
			else if (newPage.Title != curr.Title || newPage.Title == null)
			{
				navigationPage.PushAsync(newPage);
				if (curr.Title != "Home") // no stack flyout pages
					navigationPage.Navigation.RemovePage(navigationPage.Navigation.NavigationStack[navigationPage.Navigation.NavigationStack.Count - 2]);
			}
			IsPresented = false;
		}

		void OnRecentTapped(object sender, ItemTappedEventArgs e)
		{
			if (sender == null || e == null)
				return;
			Deck deck = (Deck)e.Item;

			var curr = Detail.Navigation.NavigationStack.LastOrDefault();
			var newPage = new DeckDetailView(deck);

			navigationPage.PushAsync(newPage);

			if (curr.Title != "Home") // no stack flyout pages
				navigationPage.Navigation.RemovePage(navigationPage.Navigation.NavigationStack[navigationPage.Navigation.NavigationStack.Count - 2]);

			IsPresented = false;
		}
	}
}
