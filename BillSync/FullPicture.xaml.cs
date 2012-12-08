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
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;

namespace BillSync
{
    public partial class FullPicture : PhoneApplicationPage
    {
        string fileName;
        public FullPicture()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string msg = NavigationContext.QueryString["msg"];
            fileName = msg;
            MessageBox.Show(msg);
            this.LayoutRoot.Background = getImageFromIsolatedStorage(msg + ".jpg");
        }
        private Brush getImageFromIsolatedStorage(string imageName)
        {
            ImageBrush temp = new ImageBrush();
            BitmapImage bimg = new BitmapImage();

            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (iso.FileExists(imageName))
                {
                    using (IsolatedStorageFileStream stream = iso.OpenFile(imageName, FileMode.Open, FileAccess.Read))
                    {
                        bimg.SetSource(stream);
                    }
                }
                else
                {
                    bimg = null;
                }
            }

            temp.ImageSource = bimg;
            return temp;
        }

        private void ApplicationBarDeleteButton_Click(object sender, EventArgs e)
        {
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (iso.FileExists(fileName + ".jpg") && (iso.FileExists(fileName + "_th.jpg")))
                {
                    iso.DeleteFile(fileName + ".jpg");
                    iso.DeleteFile(fileName + "_th.jpg");
                    MessageBox.Show("File sucessfully deleted");
                    NavigationService.GoBack();
                }
                else if (iso.FileExists(fileName + "_th.jpg"))
                {
                    iso.DeleteFile(fileName + "_th.jpg");
                    MessageBox.Show("File sucessfully deleted");
                    NavigationService.GoBack();
                }                
                else if (iso.FileExists(fileName + ".jpg"))
                {
                    iso.DeleteFile(fileName + ".jpg");
                    MessageBox.Show("File sucessfully deleted");
                    NavigationService.GoBack();
                }
            }

        }

    }

}