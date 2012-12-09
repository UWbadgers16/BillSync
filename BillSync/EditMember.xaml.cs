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
    public partial class EditMember : PhoneApplicationPage
    {

        Member edit_memb = new Member();
        public EditMember()
        {
            InitializeComponent();
            
            edit_memb = GlobalVars.member;
            member_name.Text = edit_memb.Name;
            textBox_name.Text = edit_memb.Name;
            textBox_email.Text = edit_memb.Email;
            textBox_phone.Text = edit_memb.Phone;
        }

        private void button_editMembers_Click(object sender, EventArgs e)
        {
            Database_Functions.EditMember(edit_memb.ID, textBox_name.Text, textBox_email.Text, textBox_phone.Text);
            NavigationService.Navigate(new Uri("/People.xaml", UriKind.Relative));

        }
        private void textBox_name_Tap(object sender, EventArgs e)
        {
            InputScope Keyboard = new InputScope();
            InputScopeName ScopeName = new InputScopeName();
            ScopeName.NameValue = InputScopeNameValue.Text;
            Keyboard.Names.Add(ScopeName);
            textBox_phone.InputScope = Keyboard;
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