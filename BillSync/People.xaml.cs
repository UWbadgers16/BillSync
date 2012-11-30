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
using Microsoft.Phone.UserData;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;
using System.Windows.Controls.Primitives;

namespace BillSync
{
    public partial class People : PhoneApplicationPage
    {
        int page = 0;
        List<NewMember> memb = new List<NewMember>();
        
        public People()
        {
            InitializeComponent();
            Contacts objContacts = new Contacts();
            objContacts.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(Contacts_SearchCompleted);
            objContacts.SearchAsync(string.Empty, FilterKind.None, null);
            
        }
        void Contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
           List<String> duplicates = new List<String>();
            try
            {
                List
                    <JumpList> source = new List<JumpList>();

                foreach (Member memb in Database_Functions.GetMembers())
                {
                    decimal trans = Database_Functions.GetMemberTotal(memb.ID);
                    
                    if (!duplicates.Contains(memb.Name))
                    {
                        duplicates.Add(memb.Name);
                        if (trans < 0)
                        {
                            source.Add(new JumpList()
                            {
                                Name = memb.Name,
                                SelectedComponentImage = "Images/delete.png"
                            });

                        }
                        else
                        {
                            source.Add(new JumpList()
                            {

                                Name = memb.Name,
                                SelectedComponentImage = "Images/add.png"
                            });
                        }

                    }
                    
                  }
               
                       

                /*  foreach (var result in e.Results)
                  {
 
                      source.Add(new JumpList() { Name = result.DisplayName });     
                 }*/
                /*
                foreach (var trans in Database_Functions.GetTransactions())
                {
                    if (trans.Amount < 0)
                    {

                        source.Add(new JumpList() { SelectedComponentImage = "Images/minus.png" });
                    }
                    else
                    {

                        source.Add(new JumpList() { SelectedComponentImage = "Images/add.png" });
                    }
                }
                */
                var groupBy = from jumplist in source
                              group jumplist by jumplist.GroupHeader into c
                              orderby c.Key
                              select new Group2<JumpList>(c.Key, c);



                this.outstandingListGroups.ItemsSource = groupBy;
                this.settledListGroups.ItemsSource = groupBy;
                this.allListGroups.ItemsSource = groupBy;
            }
            catch (System.Exception)
            {
                //That's okay, no results
            }

        }
      /*   
        private void ContactResultsData_Tap(object sender, GestureEventArgs e)
        {
            App.con = ((sender as ListBox).SelectedValue as Contact);

            NavigationService.Navigate(new Uri("/ContactDetails.xaml", UriKind.Relative));
        }
        */
      
        void tap_JumpListItem(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock temp = (TextBlock)sender;
            NavigationService.Navigate(new Uri("/ContactDetails.xaml?msg=" + temp.Text, UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

        //    string msg = NavigationContext.QueryString["msg"];
       //     string this_page = NavigationContext.QueryString["this_page"];

        //    if(!int.TryParse(msg, out page))
                panorama_people.DefaultItem = panorama_people.Items[page];
        }

        private void outstandingListGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void settledListGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void allListGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewMember.xaml?", UriKind.Relative));
        }
        private int findMember(string name, IList<Member> members)
        {
            foreach (Member m in members)
            {
                if (m.Name.Equals(name))
                    return m.ID;
            }

            return -1;
        }

        private void Member_Hold(object sender, GestureEventArgs e)
        {
            IList<Member> members = Database_Functions.GetMembers();
            TextBlock tapped = (TextBlock)sender;
            int index = findMember(tapped.Text, members);
         //   GlobalVars.member = memb[index];
            contextMenu_edit.IsOpen = true;
        }

        private void editMember_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewMember.xaml", UriKind.Relative));
        }

        
        
    }
    
}