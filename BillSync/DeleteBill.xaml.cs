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
    public partial class DeleteBill : PhoneApplicationPage
    {
        List<NewItem> items = new List<NewItem>();

        public DeleteBill()
        {
            InitializeComponent();
            items = GlobalVars.items;
            GlobalVars.items = null;
            updateList();
        }

        public class BillGroup<T> : IEnumerable<T>
        {
            public BillGroup(string name, IEnumerable<T> items)
            {
                this.Title = name.Substring(0, 1);
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

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock tapped = (TextBlock)sender;
            string tappedText = tapped.Text;
            for(int i = 0; i < items.Count; i++)
            {
                if (items[i].item_name.Text.Equals(tappedText))
                {
                    MessageBoxResult m = MessageBox.Show("You'd like to delete " + tappedText + "?", "Delete?", MessageBoxButton.OKCancel);
                    if(m == MessageBoxResult.OK)
                        items.RemoveAt(i);
                }
            }

            updateList();
        }

        private void updateList()
        {
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

        private void ApplicationBarDeleteButton_Click(object sender, EventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("You'd like to delete these items?", "Delete?", MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.OK)
            {
                GlobalVars.items = items;
                //NavigationService.Navigate(new Uri("/NewGroup.xaml?msg=" + "delete", UriKind.Relative));
                NavigationService.GoBack();
            }
        }
    }
}