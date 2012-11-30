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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace BillSync
{
    public partial class ContactDetails : PhoneApplicationPage
    {
        public ContactDetails()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string msg = NavigationContext.QueryString["msg"];
            IList<Member> members = Database_Functions.GetMembers();
            Member memb = findMember(msg, members);
            member_name.Text = memb.Name;
            phone_number.Text = memb.Phone;
            email_address.Text = memb.Email;
            decimal trans = Database_Functions.GetMemberTotal(memb.ID);
            money_owed.Text = "$ " + trans.ToString();
        }

        private Member findMember(string name, IList<Member> members)
        {
            foreach (Member m in members)
            {
                if (m.Name.Equals(name))
                    return m;
            }

            return null;
        }

    }
}