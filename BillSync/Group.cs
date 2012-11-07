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
     * Groups
     *  ID int pkey
     *  name string
     *  created datetime
     */

    [Table]
    public class Group
    {
        private EntitySet<Item> itemsRef;
        private EntitySet<Member> membersRef;

        public Group()
        {
            this.itemsRef = new EntitySet<Item>(this.OnItemAdded, this.OnItemRemoved);
            this.membersRef = new EntitySet<Member>(this.OnMemberAdded, this.OnMemberRemoved);
        }

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

        [Column]
        public DateTime Created
        {
            get;
            set;
        }

        [Association(Name = "FK_Group_Items", Storage = "itemsRef", ThisKey = "ID", OtherKey = "GroupID")]
        public EntitySet<Item> Items
        {
            get
            {
                return this.itemsRef;
            }
        }

        private void OnItemAdded(Item item)
        {
            item.Group = this;
        }

        private void OnItemRemoved(Item item)
        {
            item.Group = null;
        }

        [Association(Name = "FK_Group_Members", Storage = "membersRef", ThisKey = "ID", OtherKey = "GroupID")]
        public EntitySet<Member> Members
        {
            get
            {
                return this.membersRef;
            }
        }

        private void OnMemberAdded(Member member)
        {
            member.Group = this;
        }

        private void OnMemberRemoved(Member member)
        {
            member.Group = null;
        }
    }
}