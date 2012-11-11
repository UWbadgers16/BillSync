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

namespace BillSync
{
    public partial class NewGroup : PhoneApplicationPage
    {
        Popup newItemName = new Popup();
        List<Item> items = new List<Item>();
        List<Member> source = new List<Member>();
        Boolean specify_amount = false;

        public NewGroup()
        {
            InitializeComponent();
            textBlock_specifyName.Visibility = Visibility.Collapsed;
            textBox_specifyAmount.Visibility = Visibility.Collapsed;
            source.Add(new Member() { Name = "John" });
            source.Add(new Member() { Name = "Eric" });
            source.Add(new Member() { Name = "Yue Weng" });
            source.Add(new Member() { Name = "Georgii" });
            this.listPicker.ItemsSource = source;
        }

        public List<Item> Items
        {
            get { return items; }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string msg = "";

            if (NavigationContext.QueryString.TryGetValue("msg", out msg))
                pivot_bill.Title = msg;
        }

        private void ApplicationBarAddButton_Click(object sender, EventArgs e)
        {
            PivotItem newPivot = new PivotItem();
            Prompt newItemPrompt = new Prompt(Prompt.Type.Bill);
            Item newItem = new Item();
            newItemName.Child = newItemPrompt;
            newItemName.VerticalOffset = 180;
            newItemName.HorizontalOffset = 30;
            newItemName.IsOpen = true;

            newItemPrompt.button_create.Click += (s, args) =>
            {
                newItemName.IsOpen = false;
                newItem.Title = newItemPrompt.Title;
                items.Add(newItem);
                newPivot.Header = newItem.Title;
                pivot_bill.Items.Add(newPivot);
            };
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (newItemName.IsOpen)
            {
                newItemName.IsOpen = false;
                e.Cancel = true;
            }
            else
                base.OnBackKeyPress(e);
        }

        private void ApplicationBarDeleteButton_Click(object sender, EventArgs e)
        {
            GlobalVars.globalData = items;
            NavigationService.Navigate(new Uri("/DeleteBill.xaml", UriKind.Relative));
        }

        private void button_addContributor_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/People.xaml?msg=" + "2", UriKind.Relative));
        }

        private void button_specifyAmount_Click(object sender, RoutedEventArgs e)
        {
            if (!specify_amount)
            {
                Member temp = (Member)listPicker.Items[listPicker.SelectedIndex];
                textBlock_specifyName.Text = temp.Name;
                textBlock_specifyName.Visibility = Visibility.Visible;
                textBox_specifyAmount.Visibility = Visibility.Visible;
                specify_amount = true;
            }
            else
            {
                textBlock_specifyName.Visibility = Visibility.Collapsed;
                textBox_specifyAmount.Visibility = Visibility.Collapsed;
                specify_amount = false;
            }
            
        }

        private void listPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (specify_amount)
            {
                Member temp = (Member)listPicker.Items[listPicker.SelectedIndex];
                textBlock_specifyName.Text = temp.Name;
            }
        }

        private void pivot_bill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}