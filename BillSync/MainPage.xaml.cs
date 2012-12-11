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
using System.Windows.Controls.Primitives;
using System.Threading;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Live;
using Microsoft.Live.Controls;
using System.Windows.Resources;
using Microsoft.Xna.Framework.Media;
using Microsoft.Phone.Tasks;

namespace BillSync
{
    public partial class MainPage : PhoneApplicationPage
    {
        Popup newGroupName = new Popup();
        string[] filenames;

        private const String ConnectionString = @"isostore:/BillDB.sdf";

        public MainPage()
        {
            InitializeComponent();
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                if (!context.DatabaseExists())
                {
                    // create database if it does not exist
                    context.CreateDatabase();
                    Database_Functions.test();
                }
                else
                { // debug
                    //MessageBox.Show("deleting db");
                    context.DeleteDatabase();
                    context.CreateDatabase();
                    Database_Functions.test();
                }
            }
            //deleteIsolatedStorage();
        }

        private void textBlock_newGroup_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.IsEnabled = false;
            Prompt newGroup = new Prompt(Prompt.Type.Group);
            newGroupName.Child = newGroup;
            newGroupName.VerticalOffset = 180;
            newGroupName.HorizontalOffset = 30;
            newGroupName.IsOpen = true;

            newGroup.button_create.Click += (s, args) =>
            {
                this.IsEnabled = true;
                newGroupName.IsOpen = false;
                NavigationService.Navigate(new Uri("/NewGroup.xaml?msg=" + newGroup.Title, UriKind.Relative));
            };
        }

        private void textBlock_notifications_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Notifications.xaml", UriKind.Relative));
        }

        private void textBlock_bills_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //progressBar.Visibility = Visibility.Visible;
            //GlobalVars.main = this;
            NavigationService.Navigate(new Uri("/ItemsList.xaml", UriKind.Relative));
        }

        private void textBlock_people_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/People.xaml", UriKind.Relative));
        }

        private void textBlock_submitPayment_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SubmitPayment.xaml", UriKind.Relative));
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (newGroupName.IsOpen)
            {
                this.IsEnabled = true;
                newGroupName.IsOpen = false;
                e.Cancel = true;
            }
            else
                base.OnBackKeyPress(e);
        }

        private void PanoramaItem_Loaded(object sender, RoutedEventArgs e)
        {
            IList<BitmapImage> isoImages = getImages();
            IList<Image> images = new List<Image>();
            images.Add(image6);
            images.Add(image7);
            images.Add(image8);
            images.Add(image9);
            images.Add(image10);
            images.Add(image11);
            WriteableBitmap wmp;

            for (int i = 0; i < isoImages.Count; i++)
            {
                if (isoImages[i] != null)
                {
                    wmp = new WriteableBitmap(isoImages[i]);
                    wmp = wmp.Rotate(90);
                    images[i].Source = wmp;
                }
                else
                {
                    images[i].Source = isoImages[i];
                }
            }
        }

        private IList<BitmapImage> getImages()
        {
            ImageBrush temp = new ImageBrush();
            IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();
            filenames = pickImages(iso.GetFileNames());
            IList<BitmapImage> images = new List<BitmapImage>();

            foreach (string file in filenames)
            {
                BitmapImage bimg = new BitmapImage();
                if (iso.FileExists(file))
                {
                    using (IsolatedStorageFileStream stream = iso.OpenFile(file, FileMode.Open, FileAccess.Read))
                    {
                        bimg.SetSource(stream);
                    }
                }
                else
                    bimg = null;

                images.Add(bimg);
            }

            return images;
        }

        private string[] pickImages(string[] files)
        {
            string[] filenames = new string[6];
            IList<string> thumbnails = findThumbnails(files);
            Random rand = new Random();
            IList<int> index = new List<int>();
            int temp;

            for (int i = 0; i < thumbnails.Count && i < filenames.Length; i++)
            {
                temp = rand.Next(thumbnails.Count);

                while (index.Contains(temp))
                    temp = rand.Next(thumbnails.Count);

                filenames[i] = thumbnails[temp];
                index.Add(temp);
            }

            return filenames;
        }

        private void image6_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int index = filenames[0].IndexOf("_");
            enlargePicture(filenames[0].Substring(0, index));
        }

        private void image7_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int index = filenames[1].IndexOf("_");
            enlargePicture(filenames[1].Substring(0, index));
        }

        private void image8_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int index = filenames[2].IndexOf("_");
            enlargePicture(filenames[2].Substring(0, index));
        }

        private void image9_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int index = filenames[3].IndexOf("_");
            enlargePicture(filenames[3].Substring(0, index));
        }

        private void image10_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int index = filenames[4].IndexOf("_");
            enlargePicture(filenames[4].Substring(0, index));
        }

        private void image11_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int index = filenames[5].IndexOf("_");
            enlargePicture(filenames[5].Substring(0, index));
        }

        private void enlargePicture(string name)
        {
            NavigationService.Navigate(new Uri("/FullPicture.xaml?msg=" + name, UriKind.Relative));
        }

        private void textBlock_syncing_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SkyDrive.xaml", UriKind.Relative));
        }

        private IList<string> findThumbnails(string[] files)
        {
            IList<string> thumbnails = new List<string>();

            foreach (string file in files)
            {
                if (file.Contains("_th.jpg"))
                    thumbnails.Add(file);
            }

            return thumbnails;
        }

        private void deleteIsolatedStorage()
        {
            IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();
            string[] filenames = iso.GetFileNames();
            foreach (string file in filenames)
            {
                iso.DeleteFile(file);
            }
        }
    }
}