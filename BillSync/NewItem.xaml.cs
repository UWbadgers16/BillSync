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
        IList<TextBlock> textBlocks = new List<TextBlock>();
        IList<TextBox> textBoxes = new List<TextBox>();
        IList<ToggleSwitch> toggleSwitches = new List<ToggleSwitch>();
        Boolean specify_amount = false;
        //List<Member> source = new List<Member>();
        Boolean isEditing = false;
        NewGroup group;
        int item_id = -1;
        IList<Member> members;
        //int group_id = -1;

        //public int Group_ID
        //{
        //    get { return group_id; }
        //    set { group_id = value; }
        //}

        public NewGroup Group
        {
            get { return group; }
            set { group = value; }
        }

        public int Item_ID
        {
            get { return item_id; }
            set { item_id = value; }
        }

        public IList<TextBlock> TextBlocks
        {
            get { return textBlocks; }
            set { textBlocks = value; }
        }

        public IList<TextBox> TextBoxes
        {
            get { return textBoxes; }
            set { textBoxes = value; }
        }

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
            members = GlobalVars.members;
            GlobalVars.members = null;

            if (GlobalVars.item != null)
            {
                NewItem load = GlobalVars.item;
                this.item_id = load.item_id;
                this.group = load.group;
                this.item_name.Text = load.item_name.Text;
                this.textBox_description.Text = load.textBox_description.Text;
                this.textBox_total.Text = load.textBox_total.Text;
                this.checkBox_splitEven.IsChecked = load.checkBox_splitEven.IsChecked;
                this.datePicker_date = load.datePicker_date;
                textBlocks.Clear();
                textBoxes.Clear();
                //this.listPicker.ItemsSource = load.listPicker.ItemsSource;
                this.listPicker.ItemsSource = group.Members;
                loadSpecifics(load);
                isEditing = true;
                GlobalVars.item = null;
            }
            //else if (GlobalVars.selectedMembers != null && GlobalVars.selectMode != null)
            //{
            //    if (GlobalVars.selectMode.Equals("payers"))
            //        listPicker_payers.ItemsSource = GlobalVars.selectedMembers;
            //    //else if(GlobalVars.selectMode.Equals("owers"))
            //    GlobalVars.selectedMembers = null;
            //    GlobalVars.selectMode = null;
            //}
            else
            {
                try
                {
                    string msg = NavigationContext.QueryString["msg"];
                    this.item_name.Text = msg;
                    this.listPicker.ItemsSource = group.Members;
                }
                catch (KeyNotFoundException ex)
                {
                    //do nothing
                }
            }
            this.textBox_itemName.Text = this.item_name.Text;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            loadAmounts();
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
                toggleSwitches[index].Visibility = Visibility.Visible;
                textBoxes[index].Visibility = Visibility.Visible;
                specify_amount = true;
            }
            else
            {
                textBlocks[index].Visibility = Visibility.Collapsed;
                toggleSwitches[index].Visibility = Visibility.Collapsed;
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
                toggleSwitches[listPicker.SelectedIndex].Visibility = Visibility.Visible;
                textBoxes[listPicker.SelectedIndex].Visibility = Visibility.Visible;
            }
        }

        public void changeSpecifyAmount(Boolean open)
        {
            specify_amount = open;
            if (specify_amount)
                collapseAll();
        }

        private void collapseAll()
        {
            for (int i = 0; i < listPicker.Items.Count; i++)
            {
                textBlocks[i].Visibility = Visibility.Collapsed;
                toggleSwitches[i].Visibility = Visibility.Collapsed;
                textBoxes[i].Visibility = Visibility.Collapsed;
            }
        }

        private void ApplicationBarSaveButton_Click(object sender, EventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("Would you like to save this item?", "Save item?", MessageBoxButton.OKCancel);

            if (m == MessageBoxResult.OK)
            {
                saveItem();
            }
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
                //newBlock.Text = load.textBlocks[i].Text;
                textBlocks.Add(newBlock);
                ToggleSwitch toggle = new ToggleSwitch();
                toggle.Content = "owe";
                toggle.Checked += new EventHandler<RoutedEventArgs>(toggle_Checked);
                toggle.Unchecked += new EventHandler<RoutedEventArgs>(toggle_Unchecked);
                toggle.IsChecked = load.toggleSwitches[i].IsChecked;
                toggle.Visibility = Visibility.Collapsed;
                toggleSwitches.Add(toggle);
                TextBox newBox = new TextBox();
                newBox.Height = 71;
                newBox.Width = 460;
                newBox.Text = "";
                newBox.Margin = new Thickness(-18, -5, 0, 0);
                newBox.Visibility = Visibility.Collapsed;
                newBox.Text = load.textBoxes[i].Text;
                InputScope asdf = new InputScope();
                newBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
                textBoxes.Add(newBox);
                stackPanel_main.Children.Add(newBlock);
                stackPanel_main.Children.Add(toggle);
                stackPanel_main.Children.Add(newBox);
            }
        } 

        private void checkBox_splitEven_Checked(object sender, RoutedEventArgs e)
        {
            button_specifyAmount.IsEnabled = false;
            int index = listPicker.SelectedIndex;
            if (index != -1)
            {
                textBlocks[index].Visibility = Visibility.Collapsed;
                toggleSwitches[index].Visibility = Visibility.Collapsed;
                textBoxes[index].Visibility = Visibility.Collapsed;
                specify_amount = false;
            }
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
            //addMissingMembers();
            IList<int> addMembers = new List<int>();
            IList<Member> activeMembers = Database_Functions.GetActiveMembers(group.Group_ID);
            IList<string> activeNames = new List<string>();
            foreach (Member m in activeMembers)
            {
                activeNames.Add(m.Name);
            }
            foreach (Member m in activeMembers)
            {
                activeNames.Add(m.Name);
            }
            if (checkFields())
            {
                this.item_name.Text = textBox_itemName.Text;

                if (isEditing)
                {
                    GlobalVars.editItem = this;
                    isEditing = false;
                    Database_Functions.EditItem(item_id, textBox_itemName.Text, textBox_description.Text, datePicker_date.Value.Value);
                    foreach (Member m in members)
                    {
                        if (!activeNames.Contains(m.Name))
                            addMembers.Add(Database_Functions.AddMember(group.Group_ID, m.Name, m.Email, m.Phone));
                    }

                    addNewTransactions(addMembers);
                }
                else
                {
                    item_id = Database_Functions.AddItem(group.Group_ID, textBox_itemName.Text, textBox_description.Text, datePicker_date.Value.Value);
                    GlobalVars.item = this;
                    foreach (Member m in members)
                    {
                        addMembers.Add(Database_Functions.AddMember(group.Group_ID, m.Name, m.Email, m.Phone));
                    }

                    addTransactions(addMembers);
                }

                NavigationService.GoBack();
            }
            else
                MessageBox.Show("Please fill in all required fields", "Invalid fields", MessageBoxButton.OK);
        }

        public void loadAmounts()
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
                ToggleSwitch toggle = new ToggleSwitch();
                toggle.Content = "owe";
                toggle.Checked += new EventHandler<RoutedEventArgs>(toggle_Checked);
                toggle.Unchecked += new EventHandler<RoutedEventArgs>(toggle_Unchecked);
                toggle.Visibility = Visibility.Collapsed;
                toggleSwitches.Add(toggle);
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
                stackPanel_main.Children.Add(toggle);
                stackPanel_main.Children.Add(newBox);
            }
        }

        void toggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggle = (ToggleSwitch)sender;
            toggle.Content = "owes";
        }

        void toggle_Checked(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggle = (ToggleSwitch)sender;
            toggle.Content = "pays";
        }

        private void addTransactions(IList<int> members)
        {
            Database_Functions.DeleteTransactions(item_id);
            decimal total;
            if(textBox_total.Text.Contains("$"))
                total = Decimal.Parse(textBox_total.Text.Substring(1));
            else
                total = Decimal.Parse(textBox_total.Text);
            decimal split = total / members.Count;
            decimal amount = split * -1;
            for (int i = 0; i < members.Count; i++)
            {
                if (checkBox_splitEven.IsChecked == true)
                    Database_Functions.AddTransaction(item_id, members[i], split * -1);
                else
                {
                    Decimal temp;
                    Decimal.TryParse(textBoxes[i].Text, out temp);
                    Database_Functions.AddTransaction(item_id, members[i], temp * -1);
                }
            }
        }

        private void addNewTransactions(IList<int> members)
        {
            decimal total;
            if (textBox_total.Text.Contains("$"))
                total = Decimal.Parse(textBox_total.Text.Substring(1));
            else
                total = Decimal.Parse(textBox_total.Text);
            decimal split = total / members.Count;
            decimal amount = split * -1;
            for (int i = 0; i < members.Count; i++)
            {
                if (checkBox_splitEven.IsChecked == true)
                    Database_Functions.AddTransaction(item_id, members[i], split * -1);
                else
                {
                    Decimal temp;
                    Decimal.TryParse(textBoxes[i].Text, out temp);
                    Database_Functions.AddTransaction(item_id, members[i], temp * -1);
                }
            }
        }

        private Boolean checkFields()
        {
            Boolean allEmpty = true;
            if (textBox_description.Text == "")
                return false;
            if (textBox_total.Text == "")
                return false;
            if (listPicker.Items.Count == 0)
                return false;
            for (int i = 0; i < textBoxes.Count && allEmpty; i++)
            {
                if (textBoxes[i].Text != "")
                    allEmpty = false;
            }
            if (checkBox_splitEven.IsChecked == false && allEmpty == true)
                return false;

            return true;
        }

        private void addMissingMembers()
        {
            for (int i = 0; i < listPicker.Items.Count; i++)
            {
                Member m = (Member)listPicker.Items[i];
                if (!findMember(m.Name, members))
                    Database_Functions.AddMember(group.Group_ID, m.Name, m.Email, m.Phone);
            }
        }

        private Boolean findMember(string name, IList<Member> members)
        {
            foreach (Member m in members)
            {
                if (m.Name.Equals(name))
                    return true;
            }

            return false;
        }

        private void button_takePicture_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Camera.xaml?msg=" + item_id, UriKind.Relative));
        }

        //private void button_addPayer_Click(object sender, RoutedEventArgs e)
        //{
        //    GlobalVars.members = members;
        //    NavigationService.Navigate(new Uri("/SelectMembers.xaml?msg=" + "payers", UriKind.Relative));
        //}

        //private void button_listPayers_Click(object sender, RoutedEventArgs e)
        //{
        //    if (button_addPayer.Visibility == Visibility.Visible)
        //    {
        //        button_addPayer.Visibility = Visibility.Collapsed;
        //        listPicker_payers.Visibility = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        button_addPayer.Visibility = Visibility.Visible;
        //        listPicker_payers.Visibility = Visibility.Visible;
        //    }
        //}
    }
}