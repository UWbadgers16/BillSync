using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace BillSync
{
	public partial class Transactions : PhoneApplicationPage
    {
        enum months { January = 1, February, March, April, May, June, July, August, September, November, December };
		// Constructor
		public Transactions()
        {
            DateTime dateSystem;
			InitializeComponent();
            //Items must be added in order by correct date, otherwise they will appear out of order.
            List<Transaction2> source = new List<Transaction2>();
            dateSystem = new DateTime(2031, 11, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2032, 10, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2022, 1, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2033, 6, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2034, 8, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2035, 11, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2036, 10, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2037, 1, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2038, 6, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2039, 8, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2040, 9, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2013, 1, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2015, 1, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2012, 1, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2011, 6, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2011, 8, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2011, 9, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2011, 1, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2011, 2, 18);
            source.Add(new Transaction2() { Name = "electric Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2011, 2, 18);
            source.Add(new Transaction2() { Name = "jouwda Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2011, 4, 18);
            source.Add(new Transaction2() { Name = "Groceries", Date = getDateString(dateSystem), Amount = "100.34" });
            dateSystem = new DateTime(2010, 1, 18);
            source.Add(new Transaction2() { Name = "Badger Game", Date = getDateString(dateSystem), Amount = "87.34" });
            dateSystem = new DateTime(2010, 2, 18);
            source.Add(new Transaction2() { Name = "Horse Masks", Date = getDateString(dateSystem), Amount = "95.41" });
            dateSystem = new DateTime(2010, 3, 18);
            source.Add(new Transaction2() { Name = "McDonalds", Date = getDateString(dateSystem), Amount = "32.46" });
            dateSystem = new DateTime(2010, 4, 18);
            source.Add(new Transaction2() { Name = "Michigan State Ticket", Date = getDateString(dateSystem), Amount = "45.00" });
            dateSystem = new DateTime(2010, 5, 18);
            source.Add(new Transaction2() { Name = "Groceries", Date = getDateString(dateSystem), Amount = "97.02" });
            dateSystem = new DateTime(2010, 6, 18);
            source.Add(new Transaction2() { Name = "Ice Cream", Date = getDateString(dateSystem), Amount = "15.42" });
            dateSystem = new DateTime(2010, 7, 18);
            source.Add(new Transaction2() { Name = "Brewer Game", Date = getDateString(dateSystem), Amount = "19.99" });
            dateSystem = new DateTime(2010, 8, 18);
            source.Add(new Transaction2() { Name = "Summerfest", Date = getDateString(dateSystem), Amount = "45.65" });
            dateSystem = new DateTime(2010, 9, 18);
            source.Add(new Transaction2() { Name = "Ice Cream", Date = getDateString(dateSystem), Amount = "15.42" });
            dateSystem = new DateTime(2010, 10, 18);
            source.Add(new Transaction2() { Name = "Brewer Game", Date = getDateString(dateSystem), Amount = "19.99" });
            dateSystem = new DateTime(2010, 11, 18);
            source.Add(new Transaction2() { Name = "Summerfest", Date = getDateString(dateSystem), Amount = "45.65" });
            dateSystem = new DateTime(2010, 12, 18);
            source.Add(new Transaction2() { Name = "Freak Fest", Date = getDateString(dateSystem), Amount = "12.34" });
            dateSystem = new DateTime(2011, 1, 18);
            source.Add(new Transaction2() { Name = "Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });


			var transByDate = from trans in source
								   group trans by trans.Date into c
								   //orderby c.Key
								   select new Group<Transaction2>(c.Key, c);

            this.transListGroup.ItemsSource = transByDate;
		
		}

        public String getDateString(DateTime date)
        {

            int theMonth = date.Month;
            String theYear = date.Year.ToString();
            String dateString;
            switch (theMonth)
            {
                case 1:
                    dateString = "January";
                    break;
                case 2:
                    dateString = "February";
                    break;
                case 3:
                    dateString = "March";
                    break;
                case 4:
                    dateString = "April";
                    break;
                case 5:
                    dateString = "May";
                    break;
                case 6:
                    dateString = "June";
                    break;
                case 7:
                    dateString = "July";
                    break;
                case 8:
                    dateString = "August";
                    break;
                case 9:
                    dateString = "September";
                    break;
                case 10:
                    dateString = "October";
                    break;
                case 11:
                    dateString = "November";
                    break;
                default:
                    dateString = "December";
                    break;
            }
            return dateString + " " + theYear;
        }

		public class Group<T> : IEnumerable<T>
		{
			public Group(string name, IEnumerable<T> items)
			{
				this.Title = name;
                this.TileTitle = name.Substring(0, 3) + "\n" + "'" + name.Substring(name.Length - 2);
				this.Items = new List<T>(items);
			}

			public override bool Equals(object obj)
			{
				Group<T> that = obj as Group<T>;

				return (that != null) && (this.Title.Equals(that.Title));
			}

            public string TileTitle
            {
                get;
                set;
            }
			public string Title
			{
				get;
				set;
			}

			public IList<T> Items
			{
				get;
				set;
			}

			#region IEnumerable<T> Members

			public IEnumerator<T> GetEnumerator()
			{
				return this.Items.GetEnumerator();
			}

			#endregion

			#region IEnumerable Members

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.Items.GetEnumerator();
			}

			#endregion
		}
	}
}