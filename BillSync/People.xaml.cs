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
        
        List<NewMember> mem = new List<NewMember>();
        IList<Member> members = Database_Functions.GetAllMembers();
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
               List<JumpList> settled = new List<JumpList>();
               List<JumpList> all = new List<JumpList>();
             
                   foreach (Member memb in Database_Functions.GetAllMembers())
                   {
                       decimal trans = 0;
                       string group_name = "";
                       int numofSameMembers = findNumMember(memb, members);
                       IList<Member> mem = findMember(numofSameMembers, memb, members);
                       for (int i = 0; i < numofSameMembers; i++)
                       {
                           trans += Database_Functions.GetMemberTotal(mem.ElementAt<Member>(i).ID);
                           if (numofSameMembers == 1)
                           {
                               group_name = Database_Functions.GetMemberGroupName(mem.ElementAt<Member>(i).ID);
                           }
                           else
                           {
                               group_name += "   " + Database_Functions.GetMemberGroupName(mem.ElementAt<Member>(i).ID);
                           }

                       }
                       if (!duplicates.Contains(memb.Name))
                       {
                           duplicates.Add(memb.Name);
                           if (trans < 0)
                           {
                               source.Add(new JumpList()
                               {
                                   Name = memb.Name,
                                   SelectedComponentImage = "Images/delete.png",
                                   GroupName = group_name
                               });
                               all.Add(new JumpList()
                               {
                                   Name = memb.Name,
                                   SelectedComponentImage = "Images/delete.png",
                                   GroupName = group_name
                               });

                           }
                           else if (trans > 0)
                           {
                               source.Add(new JumpList()
                               {

                                   Name = memb.Name,
                                   SelectedComponentImage = "Images/add.png",
                                   GroupName = group_name

                               });
                               all.Add(new JumpList()
                               {

                                   Name = memb.Name,
                                   SelectedComponentImage = "Images/add.png",
                                   GroupName = group_name
                               });
                           }
                           else
                           {
                               settled.Add(new JumpList()
                               {
                                   Name = memb.Name,
                                   GroupName = group_name
                               });
                               all.Add(new JumpList()
                               {
                                   Name = memb.Name,
                                   GroupName = group_name
                               });
                           }

                       }

                   }




                   var groupBy = from jumplist in source
                                 group jumplist by jumplist.GroupHeader into c
                                 orderby c.Key
                                 select new Group2<JumpList>(c.Key, c);


                   var groupSettled = from jumplist in settled
                                      group jumplist by jumplist.GroupHeader into c
                                      orderby c.Key
                                      select new Group2<JumpList>(c.Key, c);

                   var groupAll = from jumplist in all
                                  group jumplist by jumplist.GroupHeader into c
                                  orderby c.Key
                                  select new Group2<JumpList>(c.Key, c);

                   this.outstandingListGroups.ItemsSource = groupBy;
                   this.settledListGroups.ItemsSource = groupSettled;
                   this.allListGroups.ItemsSource = groupAll;

               
              
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
            GlobalVars.member = mem[findMember(temp.Text)];
            NavigationService.Navigate(new Uri("/ContactDetails.xaml?msg=" + temp.Text, UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

        //   string msg = NavigationContext.QueryString["msg"];
        //   string this_page = NavigationContext.QueryString["this_page"];

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
        private int findMember(string name)
        {
            IList<Member> members = new List<Member>();
            foreach (Member m in members)
            {
                if (m.Name.Equals(name))
                    return m.ID;
            }

            return -1;
        }

        private void Member_Hold(object sender, GestureEventArgs e)
        {
            IList<Member> members = Database_Functions.GetAllMembers();
            TextBlock tapped = (TextBlock)sender;
            int index = findMember(tapped.Text);
            GlobalVars.member = mem[index];
            contextMenu_edit.IsOpen = true;
            //NavigationService.Navigate(new Uri("/ContactDetails.xaml?msg=" + tapped.Text, UriKind.Relative));
        }

        private void Member_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            GlobalVars.member = mem[findMember(tb.Text)];
            NavigationService.Navigate(new Uri("/ItemDetails.xaml", UriKind.Relative));
        }
        private void editMember_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewMember.xaml?", UriKind.Relative));
        }
        private int findNumMember(Member mbr, IList<Member> members)
        {
            int count = 0;
            foreach (Member m in members)
            {
                if (m.Name.Equals(mbr.Name))
                    count++;
            }

            return count;
        }
        private IList<Member> findMember(int count, Member mbr, IList<Member> members)
        {
            IList<Member> newMember = new List<Member>(count);
            foreach (Member m in members)
            {
                if (m.Name.Equals(mbr.Name))
                    newMember.Add(m);
            }



            return newMember;
        }

       
        
    }
    
}