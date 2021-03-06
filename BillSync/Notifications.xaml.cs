﻿using System;
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
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;

namespace BillSync
{
    public partial class Notifications : PhoneApplicationPage
    {
        // Constructor
        public Notifications()
        {
            InitializeComponent();
            progressBar.Visibility = Visibility.Visible;
        }
        /*
            //Items must be added in order by correct date, otherwise they will appear out of order.
            List<ItemWrapper> source = new List<ItemWrapper>();
            IList<Item> bills = Database_Functions.GetItemsSortByDueDate();
            foreach (Item bill in bills)
            {                 
                source.Add(new ItemWrapper() { 
                    Name = bill.Title, 
                    Date = getDateString(bill.Due), 
                    GroupName = Database_Functions.GetGroupName(bill.ID),
                    GroupID = (int)bill.GroupID, 
                    DueDate = getDueDateString(bill.Due)});
            }

            var transByDate = from trans in source
                              group trans by trans.Date into c
                              //orderby c.Key
                              select new Group<ItemWrapper>(c.Key, c);
            
            this.notifListGroup.ItemsSource = transByDate;

        }
        */
        public String getDueDateString(DateTime date){
            int theMonth = date.Month;
            String theYear = date.Year.ToString();
            String theDay = date.Day.ToString();
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
            return "Due: " + dateString + " " + theDay + ", " + theYear;
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
        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
            Button temp = (Button)sender;
            if (temp.Background != null)
            {
                String pictureID = (String)temp.Tag;
                NavigationService.Navigate(new Uri("/FullPicture.xaml?msg=" + pictureID, UriKind.Relative));
            }
            //open larger picture
        }
        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //setProgressBar(true);
            TextBlock temp = (TextBlock)sender;
            int group_id = Convert.ToInt32(temp.Tag);
            //MessageBox.Show(group_id.ToString());
            GlobalVars.group_id = group_id;
            //GlobalVars.itemsList = this;
            NavigationService.Navigate(new Uri("/NewGroup.xaml", UriKind.Relative));
        }
        private Brush getImageFromIsolatedStorage(string imageName)
        {
            ImageBrush temp = new ImageBrush();
            BitmapImage bimg = new BitmapImage();

            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (iso.FileExists(imageName))
                {
                    using (IsolatedStorageFileStream stream = iso.OpenFile(imageName, FileMode.Open, FileAccess.Read))
                    {
                        bimg.SetSource(stream);
                    }
                }
                else
                {
                    bimg = null;
                }
            }
            if (bimg != null)
            {
                temp.ImageSource = bimg;
                
                RotateTransform aRotateTransform = new RotateTransform();
                aRotateTransform.CenterX = 0.5; 
                aRotateTransform.CenterY = 0.5;
                aRotateTransform.Angle = 90;
                temp.RelativeTransform = aRotateTransform;
            }
            else
            {
                temp = null;
            }
            return temp;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Items must be added in order by correct date, otherwise they will appear out of order.
            List<ItemWrapper> source = new List<ItemWrapper>();
            IList<Item> bills = Database_Functions.GetItemsSortByDueDate();
            foreach (Item bill in bills)
            {
                if ((Database_Functions.GetItemCost(bill.ID) != 0) && (Database_Functions.GetItemCost(bill.ID) != 0))
                {
                    source.Add(new ItemWrapper()
                    {
                        itemID = bill.ID.ToString(),
                        thumbnail = getImageFromIsolatedStorage(bill.ID + "_th.jpg"),
                        Name = bill.Title,
                        Date = getDateString(bill.Created),
                        GroupName = Database_Functions.GetGroupName(bill.ID),
                        GroupID = (int)bill.GroupID,
                        DueDate = getDueDateString(bill.Due),
                        Amount = stringCost(Database_Functions.GetItemCost(bill.ID))
                    });
                }
            }

            var transByDate = from trans in source
                              group trans by trans.Date into c
                              //orderby c.Key
                              select new Group<ItemWrapper>(c.Key, c);

            this.notifListGroup.ItemsSource = transByDate;
            progressBar.Visibility = Visibility.Collapsed;
        }

        private string stringCost(decimal p)
        {
            decimal y = Math.Round(p, 2);
            return "$" + y.ToString();
        }
    }
}