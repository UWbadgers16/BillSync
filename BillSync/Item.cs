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
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace BillSync
{
    /*
     * Items
     *  ID int pkey
     *  group_id int fkey
     *  title string
     *  description string
     *  created datetime
     */

    [Table]
    public class Item
    {
        private Nullable<int> groupID;
        private EntityRef<Group> groupRef = new EntityRef<Group>();

        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID
        {
            get;
            set;
        }

        [Column(CanBeNull = false)]
        public string Title
        {
            get;
            set;
        }

        [Column]
        public string Description
        {
            get;
            set;
        }

        [Column]
        public DateTime Created
        {
            get;
            set;
        }

        [Column(Storage = "groupID", DbType = "Int")]
        public int? GroupID
        {
            get
            {
                return this.groupID;
            }
            set
            {
                this.groupID = value;
            }
        }

        [Association(Name = "FK_Group_Items", Storage = "groupRef", ThisKey = "GroupID", OtherKey = "ID", IsForeignKey = true)]
        public Group Group
        {
            get
            {
                return this.groupRef.Entity;
            }
            set
            {
                Group previousValue = this.groupRef.Entity;
                if (((previousValue != value) || (this.groupRef.HasLoadedOrAssignedValue == false)))
                {
                    if ((previousValue != null))
                    {
                        this.groupRef.Entity = null;
                        previousValue.Items.Remove(this);
                    }
                    this.groupRef.Entity = value;
                    if ((value != null))
                    {
                        value.Items.Add(this);
                        this.groupID = value.ID;
                    }
                    else
                    {
                        this.groupID = default(Nullable<int>);
                    }
                }
            }
        }

        private EntitySet<Transaction> transactionsIRef;

        public Item()
        {
            this.transactionsIRef = new EntitySet<Transaction>(this.OnTransactionAdded, this.OnTransactionRemoved);
        }

        [Association(Name = "FK_Item_Transactions", Storage = "transactionsIRef", ThisKey = "ID", OtherKey = "ItemID")]
        public EntitySet<Transaction> Transactions
        {
            get
            {
                return this.transactionsIRef;
            }
        }

        private void OnTransactionAdded(Transaction transaction)
        {
            transaction.Item = this;
        }

        private void OnTransactionRemoved(Transaction transaction)
        {
            transaction.Item = null;
        }
    }
}
