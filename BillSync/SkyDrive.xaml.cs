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
using Microsoft.Phone.Tasks;
using System.Text.RegularExpressions;

namespace BillSync
{
    public partial class SkyDrive : PhoneApplicationPage
    {
        private LiveConnectClient client;
        private LiveConnectSession session;
        private string cid;
        private string resid;

        public SkyDrive()
        {
            InitializeComponent();
        }

        private void skydrive_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e.Session != null && e.Status == LiveConnectSessionStatus.Connected)
            {
                session = e.Session;
                client = new LiveConnectClient(e.Session);
                //MessageBox.Show("Signed in.");
                client.GetCompleted    += new EventHandler<LiveOperationCompletedEventArgs>(signin_GetCompleted);
                client.UploadCompleted += new EventHandler<LiveOperationCompletedEventArgs>(uploadClient_UploadCompleted);
                client.GetAsync("me", null);
            }
            else
            {
                //MessageBox.Show("Not signed in:" + e.Status.ToString());
                client = null;
            } 
        }
 
        void signin_GetCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                //MessageBox.Show("Hello, signed-in user!");
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
                client.UploadAsync("me/SkyDrive/public_documents", fileName, fileStream, OverwriteOption.Overwrite);
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
        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LiveConnectClient liveClient = new LiveConnectClient(session);
                client.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(clientDataFetch_GetCompleted);
                client.GetAsync("/me/skydrive/public_documents/files");
            }
            catch (LiveConnectException exception)
            {
                MessageBox.Show("Error getting shared read link: " + exception.Message);
            }
        }
        void clientDataFetch_GetCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                List<object> data = (List<object>)e.Result["data"];
                string file_id = "";
                foreach (IDictionary<string, object> content in data)
                {
                    if ((string)content["name"] == "BillDB.sdf")
                        file_id = (string)content["id"];
                }
                if (file_id == "")
                    MessageBox.Show("No database file found.");
                else
                {
                    MessageBox.Show("DB id: " + file_id + ";");
                    try
                    {
                        LiveConnectClient liveClient = new LiveConnectClient(session);
                        liveClient.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(shareLink_completed);
                        liveClient.GetAsync(file_id + "/shared_edit_link");
                    }
                    catch (LiveConnectException exception)
                    {
                        MessageBox.Show("Error getting shared edit link: " + exception.Message);
                    }
                }
            }
        }
        void shareLink_completed(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Regex regex = new Regex(@"cid=([^&]*)&resid=([^&]*)", RegexOptions.Compiled);
                MatchCollection matches = regex.Matches((string)e.Result["link"]);
                cid = matches[0].Groups[0].Value;
                resid = matches[0].Groups[1].Value;
                MessageBox.Show("cid: " + cid + "; resid: " + resid + ";");
                EmailComposeTask email = new EmailComposeTask();
                email.Body = "Click here to get the share link: " + (string)e.Result["link"];
                email.Subject = "BillSync Share Link";
                email.To = "georgii@saveliev.su";
                email.Show();
            }
        }

        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (session == null)
            {
                MessageBox.Show("You must sign in first.");
            }
            else
            {
                LiveConnectClient client = new LiveConnectClient(session);
                client.DownloadCompleted += new EventHandler<LiveDownloadCompletedEventArgs>(OnDownloadCompleted);
                client.DownloadAsync("file.a6b2a7e8f2515e5e.A6B2A7E8F2515E5E!131/picture?type=thumbnail");
            }
        }

        void OnDownloadCompleted(object sender, LiveDownloadCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                //imageFrame.Visibility = Visibility.Visible;
                //BitmapImage imgSource = new BitmapImage();
                //imgSource.SetSource(e.Result);
                // imageFrame is a user-defined Image control.
                //imageFrame.Source = imgSource;
                e.Result.Close();
            }
            else
            {
                MessageBox.Show("Error downloading image: " + e.Error.ToString());
            }
        }
    }
}