//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using Microsoft.Phone.Controls;

//namespace BillSync
//{
//    public partial class ContactDetails : PhoneApplicationPage
//    {
//        IList<Member> members = Database_Functions.GetAllMembers();
//        IList<Group> group = Database_Functions.GetGroups();
//        public ContactDetails()
//        {
//            InitializeComponent();
//        }
//        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
//        {
//            decimal trans = 0;
//            IList<int> temp_id = new List<int>();
//            string msg = NavigationContext.QueryString["msg"];

//            int numofSameMembers = findNumMember(msg, members);
//            IList<Member> memb = findMember(numofSameMembers, msg, members);
          
//            for (int i = 0; i < numofSameMembers; i++)
//            {
//                member_name.Text = memb.ElementAt<Member>(i).Name;
//                phone_number.Text = memb.ElementAt<Member>(i).Phone;
//                email_address.Text = memb.ElementAt<Member>(i).Email;
//                trans += Database_Functions.GetMemberTotal(memb.ElementAt<Member>(i).ID);
//            }
           

//            IList<Member> members = Database_Functions.GetAllMembers();
//            Member memb = findMember(msg, members);
//            member_name.Text = memb.Name;
//            phone_number.Text = memb.Phone;
//            email_address.Text = memb.Email;
//            decimal trans = Database_Functions.GetMemberTotal(memb.ID);

//            money_owed.Text = "$ " + trans.ToString();
//        }

//        private IList<Member> findMember(int count, string name, IList<Member> members)
//        {
//            IList<Member> newMember = new List<Member>(count);
//             foreach (Member m in members)
//              {
//                  if (m.Name.Equals(name))
//                      newMember.Add(m);
//               }
           
            

//            return newMember;
//        }

//        private int findNumMember(string name, IList<Member> members)
//        {
//            int count = 0;
//            foreach (Member m in members)
//            {
//                if (m.Name.Equals(name))
//                    count++;
//            }

//            return count;
//        }
       
//    }
//}