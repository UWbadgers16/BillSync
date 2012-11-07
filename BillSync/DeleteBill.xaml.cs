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
    public partial class DeleteItem : PhoneApplicationPage
    {
        List<Item> items = new List<Item>();

        public DeleteItem()
        {
            InitializeComponent();
            items = (List<Item>)GlobalVars.globalData;
            GlobalVars.globalData = null;
            var deleteItems = from i in items
                              group i by i.Title into c
                              orderby c.Key
                              select new DeleteGroup<Item>(c.Key, c);

            this.deleteListGroup.ItemsSource = deleteItems;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            items = (List<Item>)GlobalVars.globalData;
            GlobalVars.globalData = null;
        }

        public class DeleteGroup<T> : IEnumerable<T>
        {
            public DeleteGroup(string name, IEnumerable<T> items)
            {
                this.Title = name;
                this.TileTitle = name;
                this.Items = new List<T>(items);
            }

            public override bool Equals(object obj)
            {
                DeleteGroup<T> that = obj as DeleteGroup<T>;

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
    }
}