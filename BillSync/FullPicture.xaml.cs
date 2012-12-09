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
        double initialAngle;
        double initialScale;
        double initialTranslateX;
        double initialTranslateY;

        public FullPicture()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string msg = NavigationContext.QueryString["msg"];
            fileName = msg;
            RotateTransform aRotateTransform = new RotateTransform();
            aRotateTransform.Angle = 90;
            transform.Rotation = 90;
            this.thePicture.Source = getImageSourceFromIsolatedStorage(msg + ".jpg");
        }
        private void GestureListener_DragStarted(object sender, DragStartedGestureEventArgs e)
        {
            initialTranslateX = transform.TranslateX;
            initialTranslateY = transform.TranslateY;
        }
        private void GestureListener_DragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            transform.TranslateX += e.HorizontalChange;
            transform.TranslateY += e.VerticalChange;
        }
        private void OnPinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            initialAngle = transform.Rotation;
            initialScale = transform.ScaleX;
        }

        private void OnPinchDelta(object sender, PinchGestureEventArgs e)
        {
            transform.Rotation = initialAngle + e.TotalAngleDelta;
            transform.ScaleX = initialScale * e.DistanceRatio;
            transform.ScaleY = initialScale * e.DistanceRatio;
        }
        private ImageSource getImageSourceFromIsolatedStorage(string imageName)
        {
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
            return bimg;
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
                else
                {
                    MessageBox.Show("Error: File was not found on isolated storage");
                    NavigationService.GoBack();
                }
            }

        }
    }

}