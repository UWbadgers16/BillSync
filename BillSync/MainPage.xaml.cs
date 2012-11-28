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
                    test();
                }
                else
                { // debug
                    MessageBox.Show("deleting db");
                    context.DeleteDatabase();
                    context.CreateDatabase();
                    test();
                }
            }

            //test(); //debug
        }

        // database test
        public void test()
        {
            int group1 = Database_Functions.AddGroup("Apartment");
            int group2 = Database_Functions.AddGroup("House");
            int group3 = Database_Functions.AddGroup("Trip");
            int item1 = Database_Functions.AddItem(group1, "Groceries", "orange juice and bread", DateTime.Parse("2013-01-01 7:34:42Z"));
            int item2 = Database_Functions.AddItem(group2, "Internet", "asfdfsd", DateTime.Parse("2013-01-05 7:34:42Z"));
            int item3 = Database_Functions.AddItem(group3, "Power", "asdfasdfasdfs", DateTime.Parse("2013-01-10 7:34:42Z"));
            int item4 = Database_Functions.AddItem(group3, "Cable", "qwerty", DateTime.Parse("2013-01-15 7:34:42Z"));
            int item5 = Database_Functions.AddItem(group1, "Horse masks", "OG horse, black horse, unicorn, zebra", DateTime.Parse("2013-02-01 7:34:42Z"));
            int item6 = Database_Functions.AddItem(group1, "Booze", "halloween party", DateTime.Parse("2013-04-01 7:34:42Z"));
            int item7 = Database_Functions.AddItem(group1, "N64", "", DateTime.Parse("2013-06-01 7:34:42Z"));
            int item8 = Database_Functions.AddItem(group1, "Super Smash Bros 64", "", DateTime.Parse("2013-10-01 7:34:42Z"));

            Database_Functions.ChangeDate(item1, DateTime.Parse("2012-01-01 7:34:42Z"));
            Database_Functions.ChangeDate(item2, DateTime.Parse("2012-01-05 7:34:42Z"));
            Database_Functions.ChangeDate(item3, DateTime.Parse("2012-01-10 7:34:42Z"));
            Database_Functions.ChangeDate(item4, DateTime.Parse("2012-01-15 7:34:42Z"));
            Database_Functions.ChangeDate(item5, DateTime.Parse("2012-02-01 7:34:42Z"));
            Database_Functions.ChangeDate(item6, DateTime.Parse("2012-04-01 7:34:42Z"));
            Database_Functions.ChangeDate(item7, DateTime.Parse("2012-06-01 7:34:42Z"));
            Database_Functions.ChangeDate(item8, DateTime.Parse("2012-10-01 7:34:42Z"));

            int member1 = Database_Functions.AddMember(group1, "Georgii Saveliev");
            int member2 = Database_Functions.AddMember(group2, "Eric Dargelies");
            int member3 = Database_Functions.AddMember(group3, "Yue Weng Mak");
            int member4 = Database_Functions.AddMember(group1, "John Cabaj");
            int member5 = Database_Functions.AddMember(group1, "Eric Dargelies");
            int member6 = Database_Functions.AddMember(group1, "Yue Weng Mak");
            int transaction1 = Database_Functions.AddTransaction(item1, member1, 14.50m); //group1
            int transaction2 = Database_Functions.AddTransaction(item2, member2, 20.33m); //group2
            int transaction3 = Database_Functions.AddTransaction(item4, member3, 20.33m); //group3
            int transaction4 = Database_Functions.AddTransaction(item3, member3, 65.88m); //group3

            // group1 - items 1, 5-8
            Database_Functions.AddTransaction(item2, member2, -13.54m); //group2
            Database_Functions.AddTransaction(item1, member4, -36.54m); //group1
            Database_Functions.AddTransaction(item5, member5, 5.00m);   //group1
            Database_Functions.AddTransaction(item6, member6, 10.04m);  //group1
            Database_Functions.AddTransaction(item7, member6, -20.54m); //group1
            Database_Functions.AddTransaction(item8, member6, -16.54m); //group1
            Database_Functions.AddTransaction(item5, member1, -66.54m); //group1
            Database_Functions.AddTransaction(item6, member4, -77.54m); //group1
            Database_Functions.AddTransaction(item7, member5, -88.54m); //group1
            Database_Functions.AddTransaction(item8, member1, -16.54m); //group1
            Database_Functions.AddTransaction(item8, member4, -16.54m); //group1
            Database_Functions.AddTransaction(item8, member5, -16.54m); //group1
            Database_Functions.AddTransaction(item8, member4, 1.54m);   //group1
            Database_Functions.AddTransaction(item8, member6, 3.54m);   //group1

            //Database_Functions.PrintGroups();
            //Database_Functions.PrintItems();
            //MessageBox.Show("deleting");
            //Database_Functions.removeItem(item1);
            //Database_Functions.removeItem(item2);
            //Database_Functions.removeItem(item3);
            //Database_Functions.PrintItems();
            //Database_Functions.PrintMembers();
            //Database_Functions.PrintTransactions();

            Database_Functions.sendLedgerEmail(group3);
        }

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
                this.IsEnabled = true;
                newGroupName.IsOpen = false;
                e.Cancel = true;
            }
            else
                base.OnBackKeyPress(e);
        }
    }
}