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
    public partial class SubmitPayment : PhoneApplicationPage
    {
        string member_name = null;

        public SubmitPayment()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            member_name = GlobalVars.member_name;
            GlobalVars.member_name = null;
            textBlock_name.Text = member_name;

            Member m = findMember(member_name);

        }

        private Member findMember(string member_name)
        {
            Member temp = new Member();
            IList<Member> members = Database_Functions.GetAllMembers();

            foreach (Member m in members)
            {
                if (m.Name.Equals(member_name))
                    temp = m;
            }

            return temp;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {

        }
    }
}