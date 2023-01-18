using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Batch.Models
{
    public class Deck : INotifyPropertyChanged
    {
		private List<Card> cards = new List<Card>();
		private List<string> tags = new List<string>();
        private string name;
		private int size;
		private string created;
		public int id;
		// color
		public event PropertyChangedEventHandler PropertyChanged;

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (name != value)
				{
					name = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new PropertyChangedEventArgs("Name"));
				}
			}
		}

		public string Created
		{
			get
			{
				return created;
			}
			set
			{
				//Console.WriteLine(created);
				//if (created == null) // not set yet
				//{
				//	created = String.Format("{0:MM.dd.yy}", DateTime.Now);
				//	if (PropertyChanged != null)
				//		PropertyChanged(this, new PropertyChangedEventArgs("Created"));
				//}
				if (created != value)
				{
					created = value;
				}
			}
		}

		public int Size
		{
			get
			{
				return cards.Count;
			}
			set
			{
				if (size != value)
				{
					size = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new PropertyChangedEventArgs("Size"));
				}
			}
		}

		public List<string> Tags
		{
			get
			{
				//if (tags.Count == 0) return "None";
				//var str = "";
				//for (int i = 0; i < tags.Count; i++)
				//{
				//	if (i == tags.Count - 1)
				//		str += "" + tags[i];
				//	else
				//		str += "" + tags[i] + ", ";
				//}
				//return str;
				return tags;
			}
			set
			{
				if (tags != value)
				{
					tags = value;
					//if (PropertyChanged != null)
					//	PropertyChanged(this, new PropertyChangedEventArgs("Tags"));
				}
			}
		}

		public List<Card> Cards
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
					//if (PropertyChanged != null)
					//	PropertyChanged(this, new PropertyChangedEventArgs("Tags"));
				}
			}
		}

		public void AddTag(string tag, bool update)
		{
			if (!Tags.Contains(tag))
			{
				Tags.Add(tag);
				if (update)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("Tags"));
				}
			}
		}

		public void AddCard(Card card)
		{
			Cards.Add(card);
			//if (update)
			//{
			//	PropertyChanged(this, new PropertyChangedEventArgs("Tags"));
			//}
		}

		public int Id
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
			}
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

}
