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
                }
                else
                { // debug
                    MessageBox.Show("deleting db");
                    context.DeleteDatabase();
                    context.CreateDatabase();
                }
            }
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                if (!context.DatabaseExists())
                {
                    // create database if it does not exist
                    context.CreateDatabase();
                }
                else
                { // debug
                    MessageBox.Show("deleting db");
                    context.DeleteDatabase();
                    context.CreateDatabase();
                }
            }

            test(); //debug
        }

        // database test
        public void test()
        {
            int test1 = Database_Functions.AddGroup("apartment");
            int test2 = Database_Functions.AddGroup("house");
            int test3 = Database_Functions.AddGroup("trip");
            Database_Functions.AddItem(test1, "groceries", "orange juice and bread");
            Database_Functions.AddItem(test2, "internet", "asfdfsd");
            Database_Functions.AddItem(test3, "powerbill", "asdfasdfasdfs");
            Database_Functions.AddItem(test3, "cable", "qwerty");
            Database_Functions.AddMember(test1, "Georgii Saveliev");
            Database_Functions.AddMember(test2, "Eric Dargelies");
            Database_Functions.AddMember(test3, "Yue Weng Mak");
            Database_Functions.AddMember(test1, "John Cabaj");
            Database_Functions.AddTransaction(1, 1, 14.50m);
            Database_Functions.AddTransaction(2, 1, 20.33m);
            Database_Functions.AddTransaction(2, 2, 45.66m);
            Database_Functions.AddTransaction(3, 3, 65.88m);
            Database_Functions.AddTransaction(9, 5, 99.99m);

            Database_Functions.PrintGroups();
            Database_Functions.PrintItems();
            Database_Functions.PrintMembers();
            Database_Functions.PrintTransactions();
        }

        //handle new item tap
        private void newBill_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Prompt newGroup = new Prompt(Prompt.Type.Group);
            newGroupName.Child = newGroup;
            newGroupName.VerticalOffset = 180;
            newGroupName.HorizontalOffset = 30;
            newGroupName.IsOpen = true;

            newGroup.button_create.Click += (s, args) =>
            {
                newGroupName.IsOpen = false;
                NavigationService.Navigate(new Uri("/GroupPage.xaml?msg=" + newGroup.Title, UriKind.Relative));
            };
        }

        //handle recent tap
        private void transactions_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ItemsList.xaml", UriKind.Relative));
        }

        //handle list tap
        private void people_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
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
                newGroupName.IsOpen = false;
                e.Cancel = true;
            }
            else
                base.OnBackKeyPress(e);
        }
    }
}