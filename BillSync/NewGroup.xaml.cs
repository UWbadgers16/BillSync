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
        IList<NewItem> items = new List<NewItem>();
        IList<ItemWrapper> source = new List<ItemWrapper>();
        Boolean first_load = true;
        int group_id;
        IList<Member> contributors = new List<Member>();
        Boolean isEditing = false;

        public IList<Member> Members
        {
            get { return contributors; }
        }

        public int Group_ID
        {
            get { return group_id; }
        }

        public NewGroup()
        {
            InitializeComponent();
            contributors.Add(new Member() { Name = "John" });
            contributors.Add(new Member() { Name = "Eric" });
            contributors.Add(new Member() { Name = "Yue Weng" });
            contributors.Add(new Member() { Name = "Georgii" });
            this.listPicker_contributors.ItemsSource = contributors;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //this.billListGroup.IsEnabled = true;

            if (GlobalVars.item != null || GlobalVars.items != null || GlobalVars.editItem != null || GlobalVars.group_id != -1)
            {
                if (GlobalVars.item != null)
                {
                    items.Add(GlobalVars.item);
                    GlobalVars.item = null;
                }
                else if (GlobalVars.items != null)
                {
                    items = GlobalVars.items;
                    GlobalVars.items = null;
                }
                else if (GlobalVars.editItem != null)
                {
                    int index = findById(GlobalVars.editItem.Item_ID);
                    items[index] = GlobalVars.editItem;
                    GlobalVars.editItem = null;
                }
                else if (GlobalVars.group_id != -1)
                {
                    isEditing = true;
                    first_load = false;
                    int temp_group_id = GlobalVars.group_id;
                    GlobalVars.group_id = -1;
                    this.group_id = temp_group_id;
                    group_name.Text = Database_Functions.GetGroupName(temp_group_id);
                    IList<Item> temp_items = Database_Functions.GetItems(temp_group_id);
                    items = populateGroup(temp_items, temp_group_id);
                }
                //List<ItemWrapper> source = new List<ItemWrapper>();
                //foreach (NewItem item in items)
                //{
                //    source.Add(new ItemWrapper() { ItemPage = item, Name = item.item_name.Text });
                //}

                //var itemSource = from i in source
                //                 group i by i.Name.Substring(0, 1) into c
                //                 orderby c.Key
                //                 select new BillGroup<ItemWrapper>(c.Key, c);

                //this.billListGroup.ItemsSource = itemSource;
                populateList(items);
            }
            else
            {
                try
                {
                    string msg = NavigationContext.QueryString["msg"];
                    group_name.Text = msg;
                    group_id = Database_Functions.AddGroup(group_name.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    //do nothing
                }
            }
        }

        private void ApplicationBarAddButton_Click(object sender, EventArgs e)
        {
            addItem();
        }

        private void ApplicationBarDeleteButton_Click(object sender, EventArgs e)
        {
            GlobalVars.items = items;
            NavigationService.Navigate(new Uri("/DeleteBill.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (first_load && items.Count == 0)
            {
                MessageBoxResult m = MessageBox.Show("Add a new item", "Add Item", MessageBoxButton.OKCancel);
                if (m == MessageBoxResult.OK)
                    addItem();
                else if (m == MessageBoxResult.Cancel)
                    NavigationService.GoBack();
            }
        }

        private void addItem()
        {
            this.IsEnabled = false;
            Prompt newItemPrompt = new Prompt(Prompt.Type.Item);
            NewItem newItem = new NewItem();
            newItemName.Child = newItemPrompt;
            newItemName.VerticalOffset = 180;
            newItemName.HorizontalOffset = 30;
            newItemName.IsOpen = true;

            newItemPrompt.button_create.Click += (s, args) =>
            {
                if (findItem(newItemPrompt.Title) != -1)
                {
                    MessageBox.Show("Item name already exists. Please enter in new name", "Item name exists", MessageBoxButton.OK);
                    newItemPrompt.textBox_name.Text = "";
                }
                else
                {
                    GlobalVars.group = this;
                    this.IsEnabled = true;
                    newItemName.IsOpen = false;
                    newItem.Title = newItemPrompt.Title;
                    NavigationService.Navigate(new Uri("/NewItem.xaml?msg=" + newItem.Title, UriKind.Relative));
                    first_load = false;
                }
            };
        }

        public class BillGroup<T> : IEnumerable<T>
        {
            public BillGroup(string name, IEnumerable<T> items)
            {
                this.Title = name.Substring(0,1);
                this.TileTitle = name.Substring(0, 1);
                this.Items = new List<T>(items);
            }

            public override bool Equals(object obj)
            {
                BillGroup<T> that = obj as BillGroup<T>;

                return (that != null) && (this.Title.Equals(that.Title));
            }

            public string TileTitle
            {
                get;
                set;
            }
            public string Title
            {
                get;
                set;
            }

            public IList<T> Items
            {
                get;
                set;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            #region IEnumerable<T> Members

            public IEnumerator<T> GetEnumerator()
            {
                return this.Items.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.Items.GetEnumerator();
            }

            #endregion
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (newItemName.IsOpen)
            {
                if (!first_load)
                {
                    this.IsEnabled = true;
                    e.Cancel = true;
                }

                newItemName.IsOpen = false;
            }
            else if (contextMenu_edit_delete.IsOpen || contextMenu_deactivate.IsOpen)
            {
                contextMenu_edit_delete.IsOpen = false;
                contextMenu_deactivate.IsOpen = false;
                e.Cancel = true;
                this.billListGroup.IsEnabled = true;
            }
            else
            {
                MessageBoxResult m = MessageBox.Show("Would you like to save this group?", "Save group?", MessageBoxButton.OKCancel);
                if (m == MessageBoxResult.OK)
                    saveGroup();
                else
                    base.OnBackKeyPress(e);

                //base.OnBackKeyPress(e);
            }
        }

        //protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        //{
        //    MessageBoxResult m = MessageBox.Show("Would you like to save this item?", "Save item?", MessageBoxButton.OKCancel);

        //    if (m == MessageBoxResult.OK)
        //        saveItem();
        //    else
        //        base.OnBackKeyPress(e);
        //}

        private void ApplicationBarSaveButton_Click(object sender, EventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("Would you like to save this group?", "Save?", MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.OK)
            {
                saveGroup();
            }
        }

        private void Item_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //view item
        }

        private int findItem(string name)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item_name.Text.Equals(name))
                {
                    return i;
                }
            }

            return -1;
        }

        private void button_addContributors_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/People.xaml?msg=" + "2" + "&this_page=" + pivot_bill.SelectedIndex.ToString(), UriKind.Relative));
        }

        private void button_deactivateContributors_Click(object sender, RoutedEventArgs e)
        {
            Member selected = (Member)listPicker_contributors.SelectedItem;
            Database_Functions.setMemberActivity(selected.ID, false);
            listPicker_contributors.ItemsSource = Database_Functions.GetMembers(group_id);
        }

        private IList<NewItem> populateGroup(IList<Item> items, int group_id)
        {
            IList<NewItem> newItems = new List<NewItem>();
            IList<Member> memb = Database_Functions.GetMembers(group_id);
            //contributors = memberIListToList(memb);
            contributors = memb;
            this.listPicker_contributors.ItemsSource = memb;

            foreach (Item i in items)
            {
                NewItem newItem = new NewItem();
                newItem.item_name.Text = i.Title;
                newItem.textBox_description.Text = i.Description;
                newItem.textBox_total.Text = Database_Functions.GetItemCost(i.ID).ToString();
                newItem.datePicker_date.Value = i.Due;
                newItem.checkBox_splitEven.IsChecked = Database_Functions.IsSplitEvenly(i.ID);
                newItem.listPicker.ItemsSource = memb;
                newItem.Item_ID = i.ID;
                //newItem.Group_ID = group_id;
                newItem.Group = this;
                newItem.loadAmounts();
                newItems.Add(newItem);
            }

            return newItems;
        }

        private void populateList(IList<NewItem> items)
        {
            IList<ItemWrapper> source = new List<ItemWrapper>();
            foreach (NewItem item in items)
            {
                source.Add(new ItemWrapper() { ItemPage = item, Name = item.item_name.Text });
            }

            var itemSource = from i in source
                             group i by i.Name.Substring(0, 1) into c
                             orderby c.Key
                             select new BillGroup<ItemWrapper>(c.Key, c);

            this.billListGroup.ItemsSource = itemSource;
        }

        private void Item_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.billListGroup.IsEnabled = false;
            TextBlock tapped = (TextBlock)sender;
            int index = findItem(tapped.Text);
            GlobalVars.item = items[index];
            contextMenu_edit_delete.IsOpen = true;
        }

        private void editItem_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewItem.xaml", UriKind.Relative));
            this.billListGroup.IsEnabled = true;
        }

        private void deleteItem_Click(object sender, RoutedEventArgs e)
        {
            NewItem temp = GlobalVars.item;
            GlobalVars.item = null;
            Boolean found = false;

            for (int i = 0; i < items.Count && !found; i++)
            {
                if (items[i].item_name.Text.Equals(temp.item_name.Text))
                {
                    MessageBoxResult m = MessageBox.Show("Would you like to delete " + items[i].item_name.Text + "?", "Delete?", MessageBoxButton.OKCancel);
                    if (m == MessageBoxResult.OK)
                        items.RemoveAt(i);
                    found = true;
                    populateList(items);
                    //delete from database
                }
            }

            this.billListGroup.IsEnabled = true;
        }

        private void saveGroup()
        {
            int newGroup = Database_Functions.AddGroup(group_name.Text);
            this.group_name.Text = textBox_groupName.Text;

            if (isEditing)
            {
                Database_Functions.EditGroup(group_id, textBox_groupName.Text);
                isEditing = false;
            }

            NavigationService.GoBack();
        }

        private void listPicker_contributors_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            string name = tb.Text;
            contextMenu_deactivate.IsOpen = true;
        }

        private void deactivateMember_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("deactivated");
        }

        private int findById(int item_id)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Item_ID == item_id)
                {
                    return i;
                }
            }

            return -1;
        }

        //private List<Member> memberIListToList(IList<Member> memb)
        //{
        //    List<Member> members = new List<Member>();

        //    foreach (Member m in memb)
        //    {
        //        members.Add(m);
        //    }

        //    return members;
        //}

    }
}