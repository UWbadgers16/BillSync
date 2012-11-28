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
using Microsoft.Phone.Tasks;

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

        public static void EditGroup(int group_id, String group_name)
        {
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                Group group = (from c in context.Groups where c.ID == group_id select c).Single();

                group.Name = group_name;

                context.SubmitChanges();
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

        public static void removeItem(int item_id)
        {
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Transaction> query = from c in context.Transactions where c.ItemID == item_id select c;
                context.Transactions.DeleteAllOnSubmit(query);

                Item item = (from c in context.Items where c.ID == item_id select c).Single();
                context.Items.DeleteOnSubmit(item);
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
                if (!is_split_even && transaction.Amount < 0)
                { // if the transaction is for amount owned, hold onto the cost
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
                IQueryable<Member> query = from c in context.Members where c.Group.ID == group_id && c.Active == true select c;
                memberList = query.ToList();
            }
            return memberList;
        }

        public static int AddMember(int group_id, String member_name, String email, string phone_number)
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
                member.Email = email;
                member.Phone = phone_number;
                member.Group = groupList.FirstOrDefault();

                context.Members.InsertOnSubmit(member);
                context.SubmitChanges();

                return member.ID;
            }
        }

        public static void EditMember(int member_id, String member_name, String email, string phone_number)
        {
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                Member member = (from c in context.Members where c.ID == member_id select c).Single();

                member.Name = member_name;
                member.Email = email;
                member.Phone = phone_number;

                context.SubmitChanges();
            }
        }

        public static void setMemberActivity(int member_id, bool active)
        {
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                Member member = (from c in context.Members where c.ID == member_id select c).Single();

                member.Active = active;

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

        /*******************************
         * Misc Functions
         ******************************* */

        public static void sendLedgerEmail(int group_id)
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Ledger for ");
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                IQueryable<Group> gquery = from c in context.Groups where c.ID == group_id select c;
                IList<Group> group = gquery.ToList();
                messageBuilder.AppendLine("Group Name: " + group.FirstOrDefault().Name + "\n==========================\n");

                IQueryable<Member> mquery = from c in context.Members where c.Group.ID == group_id select c;
                IList<Member> members = mquery.ToList();
                string membernames = "";
                foreach (Member member in members)
                {
                    membernames = membernames + member.Name + ", ";
                }

                messageBuilder.AppendLine("Members:\n" + membernames + "\n==========================\n");

                IQueryable<Item> iquery = from c in context.Items where c.Group.ID == group_id select c;
                IList<Item> items = iquery.ToList();
                foreach (Item item in items)
                {
                    messageBuilder.AppendLine("\nItem: " + item.Title + " (" + item.ID.ToString() + ")");
                    messageBuilder.AppendLine("Transactions:\n");
                    IQueryable<Transaction> tquery = from c in context.Transactions where c.ItemID == item.ID orderby c.Member.Name select c;
                    IList<Transaction> transactions = tquery.ToList();
                    foreach (Transaction transaction in transactions)
                    {
                        messageBuilder.AppendLine("\t" + transaction.Member.Name + ": " + transaction.Amount);
                    }
                }
            }

            EmailComposeTask email = new EmailComposeTask();
            email.Body = messageBuilder.ToString();
            email.Subject = "BillSync ledger for group ID " + group_id.ToString();
            email.To = "georgii@saveliev.su";
            email.Show();
        }

        // database test
        public static void test()
        {
            int group1 = AddGroup("Apartment");
            int group2 = AddGroup("House");
            int group3 = AddGroup("Trip");
            int item1 = AddItem(group1, "Groceries", "orange juice and bread", DateTime.Parse("2013-01-01 7:34:42Z"));
            int item2 = AddItem(group2, "Internet", "asfdfsd", DateTime.Parse("2013-01-05 7:34:42Z"));
            int item3 = AddItem(group3, "Power", "asdfasdfasdfs", DateTime.Parse("2013-01-10 7:34:42Z"));
            int item4 = AddItem(group3, "Cable", "qwerty", DateTime.Parse("2013-01-15 7:34:42Z"));
            int item5 = AddItem(group1, "Horse masks", "OG horse, black horse, unicorn, zebra", DateTime.Parse("2013-02-01 7:34:42Z"));
            int item6 = AddItem(group1, "Booze", "halloween party", DateTime.Parse("2013-04-01 7:34:42Z"));
            int item7 = AddItem(group1, "N64", "", DateTime.Parse("2013-06-01 7:34:42Z"));
            int item8 = AddItem(group1, "Super Smash Bros 64", "", DateTime.Parse("2013-10-01 7:34:42Z"));

            ChangeDate(item1, DateTime.Parse("2012-01-01 7:34:42Z"));
            ChangeDate(item2, DateTime.Parse("2012-01-05 7:34:42Z"));
            ChangeDate(item3, DateTime.Parse("2012-01-10 7:34:42Z"));
            ChangeDate(item4, DateTime.Parse("2012-01-15 7:34:42Z"));
            ChangeDate(item5, DateTime.Parse("2012-02-01 7:34:42Z"));
            ChangeDate(item6, DateTime.Parse("2012-04-01 7:34:42Z"));
            ChangeDate(item7, DateTime.Parse("2012-06-01 7:34:42Z"));
            ChangeDate(item8, DateTime.Parse("2012-10-01 7:34:42Z"));

            int member1 = AddMember(group1, "Georgii Saveliev", "gsaveliev@wisc.edu", "608-698-3167");
            int member2 = AddMember(group2, "Eric Dargelies", "dargelies@wisc.edu", "920-733-9441");
            int member3 = AddMember(group3, "Yue Weng Mak", "ymak@wisc.edu", "608-770-6358");
            int member4 = AddMember(group1, "John Cabaj", "cabaj@wisc.edu", "920-208-9224");
            int member5 = AddMember(group1, "Eric Dargelies", "dargelies@wisc.edu", "920-733-9441");
            int member6 = AddMember(group1, "Yue Weng Mak", "ymak@wisc.edu", "608-770-6358");
            int transaction1 = AddTransaction(item1, member1, 14.50m); //group1
            int transaction2 = AddTransaction(item2, member2, 20.33m); //group2
            int transaction3 = AddTransaction(item4, member3, 20.33m); //group3
            int transaction4 = AddTransaction(item3, member3, 65.88m); //group3

            // group1 - items 1, 5-8
            AddTransaction(item2, member2, -13.54m); //group2
            AddTransaction(item1, member4, -36.54m); //group1
            AddTransaction(item5, member5, 5.00m);   //group1
            AddTransaction(item6, member6, 10.04m);  //group1
            AddTransaction(item7, member6, -20.54m); //group1
            AddTransaction(item8, member6, -16.54m); //group1
            AddTransaction(item5, member1, -66.54m); //group1
            AddTransaction(item6, member4, -77.54m); //group1
            AddTransaction(item7, member5, -88.54m); //group1
            AddTransaction(item8, member1, -16.54m); //group1
            AddTransaction(item8, member4, -16.54m); //group1
            AddTransaction(item8, member5, -16.54m); //group1
            AddTransaction(item8, member4, 1.54m);   //group1
            AddTransaction(item8, member6, 3.54m);   //group1

            //PrintGroups();
            //PrintItems();
            //MessageBox.Show("deleting");
            //removeItem(item1);
            //removeItem(item2);
            //removeItem(item3);
            //PrintItems();
            //PrintMembers();
            //PrintTransactions();

            //sendLedgerEmail(group1);
        }
    }
}
