using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Batch.Models
{
    public class Card
    {
        private string side1, side2;
		public event PropertyChangedEventHandler PropertyChanged;

		public Card(string side1, string side2)
        {
            this.side1 = side1;
            this.side2 = side2;
        }

		public string Side1
		{
			get
			{
				return side1;
			}
			set
			{
				if (side1 != value)
				{
					side1 = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new PropertyChangedEventArgs("Side1"));
				}
			}
		}

		public string Side2
		{
			get
			{
				return side2;
			}
			set
			{
				if (side2 != value)
				{
					side2 = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new PropertyChangedEventArgs("Side2"));
				}
			}
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
