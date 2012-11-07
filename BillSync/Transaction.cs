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
     * transactions
     *  ID int pkey
     *  item_id int fkey
     *  member_id int fkey
     *  amount money
     */
    [Table]
    public class Transaction
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID
        {
            get;
            set;
        }

        [Column(CanBeNull = false)]
        public decimal Amount
        {
            get;
            set;
        }

        /*****************************
            ITEM FOREIGN KEY
         * ***************************/

        [Column(Storage = "itemID", DbType = "Int")]
        public int? ItemID
        {
            get
            {
                return this.itemID;
            }
            set
            {
                this.itemID = value;
            }
        }
  
        private Nullable<int> itemID;
        private EntityRef<Item> transactionsIRef = new EntityRef<Item>();

        [Association(Name = "FK_Item_Transactions", Storage = "transactionsIRef", ThisKey = "ItemID", OtherKey = "ID", IsForeignKey = true)]
        public Item Item
        {
            get
            {
                return this.transactionsIRef.Entity;
            }
            set
            {
                Item previousValue = this.transactionsIRef.Entity;
                if (((previousValue != value) || (this.transactionsIRef.HasLoadedOrAssignedValue == false)))
                {
                    if ((previousValue != null))
                    {
                        this.transactionsIRef.Entity = null;
                        previousValue.Transactions.Remove(this);
                    }
                    this.transactionsIRef.Entity = value;
                    if ((value != null))
                    {
                        value.Transactions.Add(this);
                        this.itemID = value.ID;
                    }
                    else
                    {
                        this.itemID = default(Nullable<int>);
                    }
                }
            }
        }

        /*****************************
            MEMBER FOREIGN KEY
         * ***************************/
        [Column(Storage = "memberID", DbType = "Int")]
        public int? MemberID
        {
            get
            {
                return this.memberID;
            }
            set
            {
                this.memberID = value;
            }
        }

        private Nullable<int> memberID;
        private EntityRef<Member> transactionsMRef = new EntityRef<Member>();

        [Association(Name = "FK_Member_Transactions", Storage = "transactionsMRef", ThisKey = "MemberID", OtherKey = "ID", IsForeignKey = true)]
        public Member Member
        {
            get
            {
                return this.transactionsMRef.Entity;
            }
            set
            {
                Member previousValue = this.transactionsMRef.Entity;
                if (((previousValue != value) || (this.transactionsMRef.HasLoadedOrAssignedValue == false)))
                {
                    if ((previousValue != null))
                    {
                        this.transactionsMRef.Entity = null;
                        previousValue.Transactions.Remove(this);
                    }
                    this.transactionsMRef.Entity = value;
                    if ((value != null))
                    {
                        value.Transactions.Add(this);
                        this.memberID = value.ID;
                    }
                    else
                    {
                        this.memberID = default(Nullable<int>);
                    }
                }
            }
        }
    }
}
