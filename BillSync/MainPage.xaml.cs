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
using System.Windows.Controls.Primitives;
using System.Threading;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;

namespace BillSync
{
    public partial class MainPage : PhoneApplicationPage
    {
        Popup newGroupName = new Popup();

        private const String ConnectionString = @"isostore:/BillDB.sdf";

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                if (!context.DatabaseExists())
                {
                    // create database if it does not exist
                    context.CreateDatabase();
                    Database_Functions.test();
                }
                else
                { // debug
                    //MessageBox.Show("deleting db");
                    context.DeleteDatabase();
                    context.CreateDatabase();
                    Database_Functions.test();
                }
            }
        }

        //public void setProgressBar(Boolean on)
        //{
        //    if (on)
        //    {
        //        customIndeterminateProgressBar.IsIndeterminate = true;
        //        customIndeterminateProgressBar.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        customIndeterminateProgressBar.IsIndeterminate = false;
        //        customIndeterminateProgressBar.Visibility = Visibility.Collapsed;
        //    }
        //}

        //handle new item tap
        private void newBill_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.IsEnabled = false;
            Prompt newGroup = new Prompt(Prompt.Type.Group);
            newGroupName.Child = newGroup;
            newGroupName.VerticalOffset = 180;
            newGroupName.HorizontalOffset = 30;
            newGroupName.IsOpen = true;

            newGroup.button_create.Click += (s, args) =>
            {
                this.IsEnabled = true;
                newGroupName.IsOpen = false;
                NavigationService.Navigate(new Uri("/NewGroup.xaml?msg=" + newGroup.Title, UriKind.Relative));
            };
        }

        //handle recent tap
        private void transactions_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //setProgressBar(true);
            //GlobalVars.main = this;
            NavigationService.Navigate(new Uri("/ItemsList.xaml", UriKind.Relative));
        }

        //handle list tap
        private void people_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //setProgressBar(true);
            GlobalVars.main = this;
            NavigationService.Navigate(new Uri("/People.xaml", UriKind.Relative));
        }

        //handle notifications tap
        private void notifications_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Notifications.xaml", UriKind.Relative));
            //MessageBox.Show("NOTIFICATIONS");
        }

        private void ApplicationBarSearchButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("SEARCH");
        }

        private void ApplicationBarSettingsMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("SETTINGS");
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (newGroupName.IsOpen)
            {
                this.IsEnabled = true;
                newGroupName.IsOpen = false;
                e.Cancel = true;
            }
            else
                base.OnBackKeyPress(e);
        }
    }
}