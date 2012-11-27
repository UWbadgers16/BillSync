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
    public partial class NewItem : PhoneApplicationPage
    {
        List<TextBlock> textBlocks = new List<TextBlock>();
        List<TextBox> textBoxes = new List<TextBox>();
        Boolean specify_amount = false;
        //List<Member> source = new List<Member>();
        Boolean isEditing = false;
        NewGroup group;

        public NewItem()
        {
            InitializeComponent();
            //source.Add(new Member() { Name = "John" });
            //source.Add(new Member() { Name = "Eric" });
            //source.Add(new Member() { Name = "Yue Weng" });
            //source.Add(new Member() { Name = "Georgii" });
            //this.listPicker.ItemsSource = source;
            group = GlobalVars.group;
            GlobalVars.group = null;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (GlobalVars.item != null)
            {
                NewItem load = GlobalVars.item;
                this.item_name.Text = load.item_name.Text;
                this.textBox_description.Text = load.textBox_description.Text;
                this.textBox_total.Text = load.textBox_total.Text;
                this.listPicker = load.listPicker;
                this.checkBox_splitEven = load.checkBox_splitEven;
                this.datePicker_date = load.datePicker_date;
                textBlocks.Clear();
                textBoxes.Clear();
                loadSpecifics(load);
                isEditing = true;
                this.listPicker.ItemsSource = load.listPicker.ItemsSource;
                GlobalVars.item = null;
            }
            else
            {
                string msg = NavigationContext.QueryString["msg"];
                item_name.Text = msg;
                this.listPicker.ItemsSource = group.Members;
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Member temp;
            for (int i = 0; i < listPicker.Items.Count; i++)
            {
                TextBlock newBlock = new TextBlock();
                newBlock.FontSize = 24;
                newBlock.Margin = new Thickness(0, 10, 0, 0);
                newBlock.Visibility = Visibility.Collapsed;
                temp = (Member)listPicker.Items[i];
                newBlock.Text = temp.Name;
                textBlocks.Add(newBlock);
                TextBox newBox = new TextBox();
                newBox.Height = 71;
                newBox.Width = 460;
                newBox.Text = "";
                newBox.Margin = new Thickness(-18, -5, 0, 0);
                newBox.Visibility = Visibility.Collapsed;
                InputScope asdf = new InputScope();
                newBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
                textBoxes.Add(newBox);
                stackPanel_main.Children.Add(newBlock);
                stackPanel_main.Children.Add(newBox);
            }
        }

        //private void button_addContributors_Click(object sender, RoutedEventArgs e)
        //{
        //    //NavigationService.Navigate(new Uri("/People.xaml?msg=" + "2" + "&this_page=" + pivot_bill.SelectedIndex.ToString(), UriKind.Relative));
        //    NavigationService.Navigate(new Uri("/People.xaml?msg=" + "2", UriKind.Relative));
        //}

        private void button_specifyAmount_Click(object sender, RoutedEventArgs e)
        {
            int index = listPicker.SelectedIndex;
            if (!specify_amount)
            {
                textBlocks[index].Visibility = Visibility.Visible;
                textBoxes[index].Visibility = Visibility.Visible;
                specify_amount = true;
            }
            else
            {
                textBlocks[index].Visibility = Visibility.Collapsed;
                textBoxes[index].Visibility = Visibility.Collapsed;
                specify_amount = false;
            }
        }

        private void listPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (specify_amount)
            {
                collapseAll();
                textBlocks[listPicker.SelectedIndex].Visibility = Visibility.Visible;
                textBoxes[listPicker.SelectedIndex].Visibility = Visibility.Visible;
            }
        }

        private void collapseAll()
        {
            for (int i = 0; i < listPicker.Items.Count; i++)
            {
                textBlocks[i].Visibility = Visibility.Collapsed;
                textBoxes[i].Visibility = Visibility.Collapsed;
            }
        }

        private void ApplicationBarSaveButton_Click(object sender, EventArgs e)
        {
            saveItem();
        }

        private void loadSpecifics(NewItem load)
        {
            Member temp;
            for (int i = 0; i < listPicker.Items.Count; i++)
            {
                TextBlock newBlock = new TextBlock();
                newBlock.FontSize = 24;
                newBlock.Margin = new Thickness(0, 10, 0, 0);
                newBlock.Visibility = Visibility.Collapsed;
                temp = (Member)listPicker.Items[i];
                newBlock.Text = temp.Name;
                newBlock.Text = load.textBlocks[i].Text;
                textBlocks.Add(newBlock);
                TextBox newBox = new TextBox();
                newBox.Height = 71;
                newBox.Width = 460;
                newBox.Text = "";
                newBox.Margin = new Thickness(-18, -5, 0, 0);
                newBox.Visibility = Visibility.Collapsed;
                newBox.Text = load.textBoxes[i].Text;
                textBoxes.Add(newBox);
                stackPanel_main.Children.Add(newBlock);
                stackPanel_main.Children.Add(newBox);
            }
        } 

        private void checkBox_splitEven_Checked(object sender, RoutedEventArgs e)
        {
            button_specifyAmount.IsEnabled = false;
            int index = listPicker.SelectedIndex;
            textBlocks[index].Visibility = Visibility.Collapsed;
            textBoxes[index].Visibility = Visibility.Collapsed;
            specify_amount = false;
        }

        private void checkBox_splitEven_Unchecked(object sender, RoutedEventArgs e)
        {
            button_specifyAmount.IsEnabled = true;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("Would you like to save this item?", "Save item?", MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.OK)
                saveItem();
            else
                base.OnBackKeyPress(e);
        }

        private void saveItem()
        {
            if (isEditing)
            {
                GlobalVars.editItem = this;
                isEditing = false;
            }
            else
            {
                int item_id = Database_Functions.AddItem(group.Group_ID, item_name.Text, textBox_description.Text, datePicker_date.Value.Value);
                GlobalVars.item = this;
            }
            //NavigationService.Navigate(new Uri("/NewGroup.xaml?msg=" + "save", UriKind.Relative));
            NavigationService.GoBack();
        }
    }
}