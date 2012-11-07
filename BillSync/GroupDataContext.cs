using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;

namespace BillSync
{
    public class GroupDataContext : DataContext
    {
        public GroupDataContext(string connectionString)
            : base(connectionString)
        {
        }

        public Table<Group> Groups
        {
            get
            {
                return this.GetTable<Group>();
            }
        }

        public Table<Item> Items
        {
            get
            {
                return this.GetTable<Item>();
            }
        }

        public Table<Member> Members
        {
            get
            {
                return this.GetTable<Member>();
            }
        }

        public Table<Transaction> Transactions
        {
            get
            {
                return this.GetTable<Transaction>();
            }
        }
    }
}
