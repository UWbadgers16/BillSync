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
        NewItem item = null;
        public ItemDetails()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            item = GlobalVars.item;
            GlobalVars.item = null;
            item_name.Text = item.item_name.Text;
            textBlock_description.Text = item.textBox_description.Text;
            textBlock_total.Text = string.Format(item.textBox_total.Text, "c");

            Member temp;
            for (int i = 0; i < item.listPicker.Items.Count; i++)
            {
                TextBlock name = new TextBlock();
                name.FontSize = 20;
                name.Margin = new Thickness(9, 64, 0, 0);
                temp = (Member)item.listPicker.Items[i];
                name.Text = temp.Name;
                TextBlock amount = new TextBlock();
                amount.FontSize = 28;
                amount.Margin = new Thickness(9, 0, 0, 0);
                amount.Text = String.Format(item.TextBoxes[i].Text, "c");
                if (amount.Text == "")
                    amount.Text = "$0";
                else if (!amount.Text.Contains("$"))
                    amount.Text = "$" + amount.Text;
                ContentPanel.Children.Add(name);
                ContentPanel.Children.Add(amount);
            }
        }
    }
}