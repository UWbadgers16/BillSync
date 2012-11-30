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
    public partial class NewMember : PhoneApplicationPage
    {
       
        public NewMember()
        {
            InitializeComponent();
        }
        private void button_addMembers_Click(object sender, EventArgs e)
        {
            Database_Functions.AddMember(0,  textBox_name.Text, textBox_email.Text, textBox_phone.Text);
            NavigationService.Navigate(new Uri("/People.xaml", UriKind.Relative));

        }
        private void textBox_name_Tap(object sender, EventArgs e)
        {
            
        }
        private void textBox_email_Tap(object sender, EventArgs e)
        {
            InputScope Keyboard = new InputScope();
            InputScopeName ScopeName = new InputScopeName();
            ScopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
            Keyboard.Names.Add(ScopeName);
            textBox_phone.InputScope = Keyboard;
        }
        private void textBox_phone_Tap(object sender, EventArgs e)
        {
            InputScope Keyboard = new InputScope();
            InputScopeName ScopeName = new InputScopeName();
            ScopeName.NameValue = InputScopeNameValue.Digits;
            Keyboard.Names.Add(ScopeName);
            textBox_phone.InputScope = Keyboard;

        }
    }
}