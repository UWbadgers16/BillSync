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
using Microsoft.Live;
using Microsoft.Live.Controls;
using System.IO.IsolatedStorage;
using System.IO;

namespace BillSync
{
    public partial class SkyDrive : PhoneApplicationPage
    {
        private LiveConnectClient client;

        public SkyDrive()
        {
            InitializeComponent();
        }

        private void skydrive_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e.Session != null && e.Status == LiveConnectSessionStatus.Connected)
            {
                client = new LiveConnectClient(e.Session);
                MessageBox.Show("Signed in.");
                client.GetCompleted    += new EventHandler<LiveOperationCompletedEventArgs>(signin_GetCompleted);
                client.UploadCompleted += new EventHandler<LiveOperationCompletedEventArgs>(uploadClient_UploadCompleted);
                client.GetAsync("me", null);
            }
            else
            {
                MessageBox.Show("Not signed in:" + e.Status.ToString());
                client = null;
            } 
        }
 
        void signin_GetCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MessageBox.Show("Hello, signed-in user!");
            }
            else
            {
                MessageBox.Show("Error calling API: " + e.Error.ToString());
            }
        }
        //Client ID: 000000004C0DDE16
        //Client secret: vXYcVOVbrznKv928f2mtRV0meB2PE1t1 

        private void uploadButton_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                MessageBox.Show("Uploading to SkyDrive");
                IsolatedStorageFileStream fileStream = null;
                string fileName = "BillDB.sdf";

                try
                {
                    using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        fileStream = store.OpenFile(fileName, FileMode.Open, FileAccess.Read);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                client.UploadAsync("me/SkyDrive", fileName, fileStream, OverwriteOption.Overwrite);
            }
            else
            {
                MessageBox.Show("You must be signed for SkyDrive");
            }
        }
 
        void uploadClient_UploadCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MessageBox.Show("File uploaded with success!");
            }
            else
            {
                MessageBox.Show("File uploading failed!");
            }
        }
    }
}