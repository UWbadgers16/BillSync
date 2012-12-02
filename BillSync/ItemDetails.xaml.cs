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
    public partial class ItemDetails : PhoneApplicationPage
    {
        NewItem newItem = null;
        
        public ItemDetails()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            newItem = GlobalVars.item;
            GlobalVars.item = null;
            Item item = Database_Functions.GetItem(newItem.Item_ID);
            this.item_name.Text = item.Title;
            this.textBlock_description.Text = item.Description;
            textBlock_total.Text = Database_Functions.GetItemCost(item.ID).ToString("c");
            IList<Transaction> transactions = Database_Functions.GetItemTransactions(item.ID);
            IList<TextBlock> names = new List<TextBlock>();
            IList<TextBlock> amounts = new List<TextBlock>();
            IList<string> names_only = new List<string>();
            IList<Decimal> amounts_only = new List<Decimal>();
            int index = -1;

            foreach (Transaction t in transactions)
            {
                TextBlock name = new TextBlock();
                name.FontSize = 20;
                name.Margin = new Thickness(9, 64, 0, 0);
                name.Text = Database_Functions.GetMember((int)t.MemberID).Name;
                TextBlock amount = new TextBlock();
                amount.FontSize = 28;
                amount.Margin = new Thickness(9, 0, 0, 0);
                amounts_only.Add(t.Amount);
                if (t.Amount < 0)
                    amount.Text = "-" + (t.Amount * -1).ToString("c");
                else
                    amount.Text = t.Amount.ToString("c");
                if (!names_only.Contains(name.Text))
                {
                    names_only.Add(name.Text);
                    names.Add(name);
                    ContentPanel.Children.Add(name);
                    amounts.Add(amount);
                    ContentPanel.Children.Add(amount);
                }
                else
                {
                    index = names_only.IndexOf(name.Text);
                    Decimal temp = amounts_only[index] + t.Amount;
                    if (temp < 0)
                        amounts[index].Text = "-" + (temp * -1).ToString("c");
                    else
                        amounts[index].Text = temp.ToString("c");
                }
            }
        }
    }
}