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
using Microsoft.Phone.Tasks;

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
            int group1 = Database_Functions.AddGroup("Apartment");
            int group2 = Database_Functions.AddGroup("House");
            int group3 = Database_Functions.AddGroup("Trip");
            int item1 = Database_Functions.AddItem(group1, "Groceries", "orange juice and bread");
            int item2 = Database_Functions.AddItem(group2, "Internet", "asfdfsd");
            int item3 = Database_Functions.AddItem(group3, "Power", "asdfasdfasdfs");
            int item4 = Database_Functions.AddItem(group3, "Cable", "qwerty");
            int item5 = Database_Functions.AddItem(group1, "Horse masks", "OG horse, black horse, unicorn, zebra");
            int item6 = Database_Functions.AddItem(group1, "Booze", "halloween party");
            int item7 = Database_Functions.AddItem(group1, "N64", "");
            int item8 = Database_Functions.AddItem(group1, "Super Smash Bros 64", "");
            int member1 = Database_Functions.AddMember(group1, "Georgii Saveliev");
            int member2 = Database_Functions.AddMember(group2, "Eric Dargelies");
            int member3 = Database_Functions.AddMember(group3, "Yue Weng Mak");
            int member4 = Database_Functions.AddMember(group1, "John Cabaj");
            int member5 = Database_Functions.AddMember(group1, "Eric Dargelies");
            int member6 = Database_Functions.AddMember(group1, "Yue Weng Mak");
            int transaction1 = Database_Functions.AddTransaction(item1, member1, 14.50m);
            int transaction2 = Database_Functions.AddTransaction(item2, member1, 20.33m);
            int transaction3 = Database_Functions.AddTransaction(item2, member2, 45.66m);
            int transaction4 = Database_Functions.AddTransaction(item3, member3, 65.88m);
            int transaction5 = Database_Functions.AddTransaction(item4, member4, 99.99m);

            // group1 - items 1, 5-8
            Database_Functions.AddTransaction(item1, member4, -36.54m);
            Database_Functions.AddTransaction(item5, member5, 5.00m);
            Database_Functions.AddTransaction(item6, member6, 10.04m);
            Database_Functions.AddTransaction(item7, member6, -20.54m);
            Database_Functions.AddTransaction(item8, member6, -55.54m);
            Database_Functions.AddTransaction(item5, member1, -66.54m);
            Database_Functions.AddTransaction(item6, member4, -77.54m);
            Database_Functions.AddTransaction(item7, member5, -88.54m);
            Database_Functions.AddTransaction(item8, member1, -6.54m);
            Database_Functions.AddTransaction(item8, member4, -16.54m);
            Database_Functions.AddTransaction(item8, member5, -26.54m);

            /*Database_Functions.PrintGroups();
            Database_Functions.PrintItems();
            Database_Functions.PrintMembers();
            Database_Functions.PrintTransactions();*/

            //sendLedgerEmail(1);
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

        private void sendLedgerEmail(int group_id)
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Ledger: \n\n==========================\n");
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Group> gquery = from c in context.Groups where c.ID == group_id select c;
                IList<Group> group = gquery.ToList();
                messageBuilder.AppendLine("Group Name: " + group.FirstOrDefault().Name + "\n==========================\n");

                IQueryable<Member> mquery = from c in context.Members where c.Group.ID == group_id select c;
                IList<Member> members = mquery.ToList();
                string membernames = "";
                foreach (Member member in members)
                {
                    membernames = membernames + member.Name + "\t";
                }

                messageBuilder.AppendLine("Members:\n" + membernames + "\n==========================\n` ");

                IQueryable<Item> iquery = from c in context.Items where c.Group.ID == group_id select c;
                IList<Item> items = iquery.ToList();
                foreach (Item item in items)
                {
                    messageBuilder.AppendLine("\nItem: " + item.Title + " (" + item.ID.ToString() + ")");
                    messageBuilder.AppendLine("Transactions:\n");
                    IQueryable<Transaction> tquery = from c in context.Transactions where c.ItemID == item.ID select c;
                    IList<Transaction> transactions = tquery.ToList();
                    foreach (Transaction transaction in transactions)
                    {
                        messageBuilder.AppendLine("\t" + transaction.Member.Name + ": " + transaction.Amount);
                    }
                }
            }

            EmailComposeTask email = new EmailComposeTask();
            email.Body = messageBuilder.ToString();
            email.Subject = "BillSync ledger for group ID " + group_id.ToString();
            email.To = "georgii@saveliev.su";
            email.Show();
        }
    }
}