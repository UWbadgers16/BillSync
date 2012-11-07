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
    public partial class Notifications : PhoneApplicationPage
    {
        enum months { January = 1, February, March, April, May, June, July, August, September, November, December };
        // Constructor
        public Notifications()
        {
            DateTime dateSystem;
            InitializeComponent();
            //Items must be added in order by correct date, otherwise they will appear out of order.
            List<Transaction2> source = new List<Transaction2>();
            dateSystem = new DateTime(2012, 11, 18);
            source.Add(new Transaction2() { Name = "John posted a new bill: Cable Bill", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2012, 11, 18);
            source.Add(new Transaction2() { Name = "Yue Weng posted a new bill: Groceries", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2012, 10, 18);
            source.Add(new Transaction2() { Name = "Georgii paid a bill: MG&E", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2012, 10, 18);
            source.Add(new Transaction2() { Name = "A bill due date is approaching: Halloween Tickets", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2012, 10, 18);
            source.Add(new Transaction2() { Name = "Georgii posted a new bill: The four horsemen of the Apocolypse", Date = getDateString(dateSystem), Amount = "9.47" });
            dateSystem = new DateTime(2012, 10, 18);
            source.Add(new Transaction2() { Name = "John paid the bill: Rent", Date = getDateString(dateSystem), Amount = "9.47" });


            var transByDate = from trans in source
                              group trans by trans.Date into c
                              //orderby c.Key
                              select new Group<Transaction2>(c.Key, c);

            this.notifListGroup.ItemsSource = transByDate;

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