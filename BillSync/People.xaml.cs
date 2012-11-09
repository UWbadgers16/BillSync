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

namespace BillSync
{
    public partial class People : PhoneApplicationPage
    {
        
        public People()
        {
            InitializeComponent();
            Contacts objContacts = new Contacts();
            objContacts.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(Contacts_SearchCompleted);
            objContacts.SearchAsync(string.Empty, FilterKind.None, null);
            
        }
         void Contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            try
            {
                List
                    <JumpList> source = new List<JumpList>();


                foreach (var result in e.Results)
                {
         
                   source.Add(new JumpList() { Name = result.DisplayName });      
                }

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
         
        private void ContactResultsData_Tap(object sender, GestureEventArgs e)
        {
            App.con = ((sender as ListBox).SelectedValue as Contact);

            NavigationService.Navigate(new Uri("/ContactDetails.xaml", UriKind.Relative));
        }

      
        void tap_JumpListItem(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
            NavigationService.Navigate(new Uri("/ContactDetails.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string msg = "";

            if (NavigationContext.QueryString.TryGetValue("msg", out msg))
                panorama_people.DefaultItem = panorama_people.Items[Convert.ToInt32(msg)];
        }

    }
    
}