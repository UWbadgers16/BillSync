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
        List<NewItem> items = new List<NewItem>();
        List<ItemWrapper> source = new List<ItemWrapper>();
        Boolean first_load = true;
        
        public NewGroup()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (GlobalVars.globalData != null)
            {
                try
                {
                    items.Add((NewItem)GlobalVars.globalData);
                }
                catch (InvalidCastException ex)
                {
                    items = (List<NewItem>)GlobalVars.globalData;
                }
                GlobalVars.globalData = null;
                List<ItemWrapper> source = new List<ItemWrapper>();
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
            else
            {
                string msg = NavigationContext.QueryString["msg"];
                group_name.Text = msg;
            }
        }

        private void ApplicationBarAddButton_Click(object sender, EventArgs e)
        {
            addItem();
        }

        private void ApplicationBarDeleteButton_Click(object sender, EventArgs e)
        {
            GlobalVars.globalData = items;
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

                first_load = false;
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
                this.IsEnabled = true;
                newItemName.IsOpen = false;
                newItem.Title = newItemPrompt.Title;
                NavigationService.Navigate(new Uri("/NewItem.xaml?msg=" + newItem.Title, UriKind.Relative));
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
                this.IsEnabled = true;
                newItemName.IsOpen = false;
                e.Cancel = true;
            }
            else
                base.OnBackKeyPress(e);
        }

        private void ApplicationBarSaveButton_Click(object sender, EventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("You'd like to save this group?", "Save?", MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.OK)
            {
                int newGroup = Database_Functions.AddGroup(group_name.Text);

                foreach (NewItem i in items)
                {
                    Database_Functions.AddItem(newGroup, i.item_name.Text, i.textBox_description.Text);

                }
            }
        }
    }
}