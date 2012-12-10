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
    public partial class SubmitPayment : PhoneApplicationPage
    {
        Member mem;

        public SubmitPayment()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            IList<Member> allMembers = Database_Functions.GetAllMembers();
            IList<string> memberNames = new List<string>();
            IList<Member> members = new List<Member>();
            foreach (Member m in allMembers)
            {
                if (!memberNames.Contains(m.Name))
                {
                    memberNames.Add(m.Name);
                    members.Add(m);
                }
            }
            listPicker_members.ItemsSource = members;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("Are you sure you'd like to submit payment?", "Submit payment?", MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.OK)
            {
                Item i = (Item)listPicker_items.SelectedItem;
                decimal amount = 0;
                decimal.TryParse(textBox_payment.Text, out amount);
                Database_Functions.AddTransaction(i.ID, mem.ID, amount);
                NavigationService.GoBack();
            }
        }

        private void listPicker_members_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mem = (Member)listPicker_members.SelectedItem;
            IList<Item> items = new List<Item>();
            IList<Member> allMembers = Database_Functions.GetAllMembers();
            for (int i = 0; i < allMembers.Count; i++)
            {
                if (allMembers[i].Name.Equals(mem.Name))
                {
                    foreach (Item item in Database_Functions.GetOwedItems(allMembers[i].ID))
                    {
                        items.Add(item);
                    }
                }
            }

            listPicker_items.ItemsSource = items;
        }
    }
}