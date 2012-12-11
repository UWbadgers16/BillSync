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
    public partial class Debug : PhoneApplicationPage
    {
        public Debug()
        {
            InitializeComponent();
        }
        private void deleteButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Deleting database.");
            deleteDB();
        }
        private void deleteDB(){
            String ConnectionString = @"isostore:/BillDB.sdf";
            using (GroupDataContext context = new GroupDataContext(ConnectionString))
            {
                if (!context.DatabaseExists())
                {
                    context.DeleteDatabase();
                }
            }
            MessageBox.Show("Successfully deleted database.");
        }
        private void populateButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Populating database.");
            deleteDB();
        }
        private void populateDB(){
            Database_Functions.test();
            MessageBox.Show("Successfully populated database.");
        }
    }
}