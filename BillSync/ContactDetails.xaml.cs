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
            base.OnNavigatedTo(e);

            //Set the data context for this page to the selected contact
            this.DataContext = App.con;

            try
            {
                //Try to get a picture of the contact
                BitmapImage img = new BitmapImage();
                img.SetSource(App.con.GetPicture());
                Picture.Source = img;
            }
            catch (Exception)
            {
                //can't get a picture of the contact
            }
        }

    }
}