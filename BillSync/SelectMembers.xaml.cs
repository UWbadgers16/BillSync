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
    public partial class SelectMembers : PhoneApplicationPage
    {
        string msg;

        public SelectMembers()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            msg = NavigationContext.QueryString["msg"];
            foreach (Member m in GlobalVars.members)
            {
                memberSelectList.Items.Add(new MultiselectItem() { Content = m.Name, FontSize = 28 });
            }
            GlobalVars.members = null;
        }

        private void ApplicationBarSelectButton_Click(object sender, EventArgs e)
        {
            IList<Member> members = Database_Functions.GetAllMembers();
            IList<Member> selectedMembers = new List<Member>();
            for (int i = 0; i < memberSelectList.SelectedItems.Count; i++)
            {
                foreach (Member m in members)
                {
                    if (m.Name.Equals((string)memberSelectList.SelectedItems[i]))
                        selectedMembers.Add(m);
                }
            }
            GlobalVars.selectedMembers = selectedMembers;
            GlobalVars.selectMode = msg;
            NavigationService.GoBack();
        }

    }
}