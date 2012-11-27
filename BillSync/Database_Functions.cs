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
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Text;

namespace BillSync
{
    public class Database_Functions
    {
        private const String ConnectionString = @"isostore:/BillDB.sdf";

        /*******************************
         * Groups
         ******************************* */

        public static IList<Group> GetGroups()
        {
            IList<Group> groupList = null;
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Group> query = from c in context.Groups select c;
                groupList = query.ToList();
            }
            return groupList;
        }

        public static int AddGroup(String group_name)
        {
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                Group group = new Group();

                group.Name = group_name;
                group.Created = DateTime.Now;

                context.Groups.InsertOnSubmit(group);
                context.SubmitChanges();

                return group.ID;
            }
        }

        public static void PrintGroups()
        {
            IList<Group> groups = GetGroups();

            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Groups: ");
            foreach (Group group in groups)
            {
                messageBuilder.AppendLine(group.ID.ToString() + " - " + group.Name);
            }
            MessageBox.Show(messageBuilder.ToString());
        }

        /*******************************
         * Items
         ******************************* */

        public static IList<Item> GetItems()
        {
            IList<Item> itemList = null;
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Item> query = from c in context.Items orderby c.Created select c;
                itemList = query.ToList();
            }
            return itemList;
        }

        public static IList<Item> GetItems(int group_id)
        {
            IList<Item> itemList = null;
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Item> query = from c in context.Items orderby c.Created where c.GroupID == group_id select c;
                itemList = query.ToList();
            }
            return itemList;
        }

        public static IList<Item> GetItemsSortByDueDate()
        {
            IList<Item> itemList = null;
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Item> query = from c in context.Items orderby c.Due select c;
                itemList = query.ToList();
            }
            return itemList;
        }

        public static int AddItem(int group_id, String item_name, String item_desc, DateTime due)
        {
            // Get associated Group for group_id
            IList<Group> groupList = null;
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Group> query = from c in context.Groups where c.ID == group_id select c;
                groupList = query.ToList();

                Item item = new Item();
                item.Title = item_name;
                item.Description = item_desc;
                item.Created = DateTime.Now;
                item.Due = due;
                item.Group = groupList.FirstOrDefault();

                context.Items.InsertOnSubmit(item);
                context.SubmitChanges();

                return item.ID;
            }
        }

        public static void EditItem(int item_id, String item_name, String item_desc, DateTime due)
        {
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                Item item = (from c in context.Items where c.ID == item_id select c).Single();

                item.Title = item_name;
                item.Description = item_desc;
                item.Due = due;

                context.SubmitChanges();
            }
        }

        public static string GetGroupName(int item_id)
        {
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                return (from c in context.Items where c.ID == item_id select c).Single().Group.Name;
            }
        }

        public static IList<Transaction> GetItemTransactions(int item_id)
        {
            IList<Transaction> transactionList = null;
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Transaction> query = from c in context.Transactions where c.ItemID == item_id select c;
                transactionList = query.ToList();
            }
            return transactionList;
        }

        public static bool IsSplitEvenly(int item_id)
        {
            IList<Transaction> transactionList = null;
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Transaction> query = from c in context.Transactions where c.ItemID == item_id select c;
                transactionList = query.ToList();
            }

            decimal cost = 0;
            bool is_split_even = false;
            int num_owers = 0;
            foreach (Transaction transaction in transactionList)
            {
                if (!is_split_even && transaction.Amount < 0){ // if the transaction is for amount owned, hold onto the cost
                    cost = transaction.Amount;
                    is_split_even = true;
                    num_owers = 1;
                }
                else if (cost == transaction.Amount)
                    num_owers++;
            }
            if (num_owers == 1)
                is_split_even = false;
            return is_split_even;
        }

        public static decimal GetItemCost(int item_id)
        {
            IList<Transaction> transactionList = null;
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Transaction> query = from c in context.Transactions where c.ItemID == item_id select c;
                transactionList = query.ToList();
            }
            decimal cost = 0;
            foreach (Transaction transaction in transactionList)
            {
                if (transaction.Amount > 0)
                    cost += transaction.Amount;
            }
            return cost;
        }

        public static void ChangeDate(int item_id, DateTime datetime)
        {
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                Item item = (from c in context.Items where c.ID == item_id select c).Single();

                item.Created = datetime;

                context.SubmitChanges();
            }
        }

        public static void PrintItems()
        {
            IList<Item> items = GetItems();

            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Items: ");
            foreach (Item item in items)
            {
                messageBuilder.AppendLine(item.GroupID + ": " + item.Title + " (" + item.Description + ")");
            }
            MessageBox.Show(messageBuilder.ToString());
        }

        public static void PrintItem(String group_name)
        {
            IList<Item> itemList = null;
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Item> query = from c in context.Items where c.Group.Name == group_name select c;
                itemList = query.ToList();
            }

            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Items: ");
            foreach (Item item in itemList)
            {
                messageBuilder.AppendLine(item.Title + " - " + item.Description);
            }
            MessageBox.Show(messageBuilder.ToString());
        }

        /*******************************
         * Members
         ******************************* */

        public static IList<Member> GetMembers()
        {
            IList<Member> memberList = null;
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Member> query = from c in context.Members select c;
                memberList = query.ToList();
            }
            return memberList;
        }

        public static IList<Member> GetMembers(int group_id)
        {
            IList<Member> memberList = null;
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Member> query = from c in context.Members where c.Group.ID == group_id select c;
                memberList = query.ToList();
            }
            return memberList;
        }

        public static int AddMember(int group_id, String member_name)
        {
            // Get associated Group for group_id
            IList<Group> groupList = null;
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Group> query = from c in context.Groups where c.ID == group_id select c;
                groupList = query.ToList();

                Member member = new Member();
                member.Name = member_name;
                member.Active = true;
                member.Group = groupList.FirstOrDefault();

                context.Members.InsertOnSubmit(member);
                context.SubmitChanges();

                return member.ID;
            }
        }

        public static void EditMember(int member_id, String member_name)
        {
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                Member member = (from c in context.Members where c.ID == member_id select c).Single();

                member.Name = member_name;

                context.SubmitChanges();
            }
        }

        public static void PrintMembers()
        {
            IList<Member> members = GetMembers();

            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Members: ");
            foreach (Member member in members)
            {
                messageBuilder.AppendLine(member.GroupID + ": " + member.Name + "; active: " + member.Active);
            }
            MessageBox.Show(messageBuilder.ToString());
        }

        /*******************************
         * Transactions
         ******************************* */

        public static IList<Transaction> GetTransactions()
        {
            IList<Transaction> transactionList = null;
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Transaction> query = from c in context.Transactions select c;
                transactionList = query.ToList();
            }
            return transactionList;
        }

        public static int AddTransaction(int item_id, int member_id, decimal amount)
        {
            // Get associated Item for item_id and Member for member_id
            IList<Item> itemList = null;
            IList<Member> memberList = null;
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Item> query1 = from c in context.Items where c.ID == item_id select c;
                itemList = query1.ToList();
                IQueryable<Member> query2 = from c in context.Members where c.ID == member_id select c;
                memberList = query2.ToList();

                Transaction transaction = new Transaction();
                transaction.Amount = amount;
                transaction.Member = memberList.FirstOrDefault();
                transaction.Item = itemList.FirstOrDefault();

                context.Transactions.InsertOnSubmit(transaction);
                context.SubmitChanges();

                return transaction.ID;
            }
        }

        public static void PrintTransactions()
        {
            IList<Transaction> transactions = GetTransactions();

            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Transactions: ");
            foreach (Transaction transaction in transactions)
            {
                messageBuilder.AppendLine(transaction.ID.ToString() + " (" + transaction.ItemID + "," + transaction.MemberID + "): " + transaction.Amount);
            }
            MessageBox.Show(messageBuilder.ToString());
        }
    }
}
