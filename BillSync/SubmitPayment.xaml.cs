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
        string member_name = null;
        Member mem = null;

        public SubmitPayment()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            member_name = GlobalVars.member_name;
            GlobalVars.member_name = null;
            textBlock_name.Text = member_name;

            mem = findMember(member_name);
            IList<Item> items = Database_Functions.GetOwedItems(mem.ID);
            listPicker_items.ItemsSource = items;
        }

        private Member findMember(string member_name)
        {
            Member temp = new Member();
            IList<Member> members = Database_Functions.GetAllMembers();

            foreach (Member m in members)
            {
                if (m.Name.Equals(member_name))
                    temp = m;
            }

            return temp;
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
            }
        }
    }
}