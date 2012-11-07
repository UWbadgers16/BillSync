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
     * members
     *  ID int pkey
     *  group_id int fkey
     *  name string
     *  active boolean
     */
    [Table]
    public class Member
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
        public string Name
        {
            get;
            set;
        }

        [Column(CanBeNull = false)]
        public bool Active
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

        [Association(Name = "FK_Group_Members", Storage = "groupRef", ThisKey = "GroupID", OtherKey = "ID", IsForeignKey = true)]
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
                        previousValue.Members.Remove(this);
                    }
                    this.groupRef.Entity = value;
                    if ((value != null))
                    {
                        value.Members.Add(this);
                        this.groupID = value.ID;
                    }
                    else
                    {
                        this.groupID = default(Nullable<int>);
                    }
                }
            }
        }
        
        private EntitySet<Transaction> transactionsMRef;

        public Member()
        {
            this.transactionsMRef = new EntitySet<Transaction>(this.OnTransactionAdded, this.OnTransactionRemoved);
        }

        [Association(Name = "FK_Member_Transactions", Storage = "transactionsMRef", ThisKey = "ID", OtherKey = "MemberID")]
        public EntitySet<Transaction> Transactions
        {
            get
            {
                return this.transactionsMRef;
            }
        }

        private void OnTransactionAdded(Transaction transaction)
        {
            transaction.Member = this;
        }

        private void OnTransactionRemoved(Transaction transaction)
        {
            transaction.Member = null;
        }
    }
}
