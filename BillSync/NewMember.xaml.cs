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
        Group new_group = new Group();
        bool newMem;
        IList<Group> grp = Database_Functions.GetGroups();
        public NewMember()
        {
            InitializeComponent();
            //this.listPicker_group.ItemsSource = grp;
        }
        private void button_addMembers_Click(object sender, EventArgs e)
        {
            //// Check whether if the member that is added exist in the database
            //newMem = checkMembers(textBox_name.Text);
            //if (newMem)
            //{
            //    MessageBox.Show("Member name already exists. Please enter in new name", "Member name exists", MessageBoxButton.OK);
            //    textBox_name.Text = "";
            //}
            //else
            //{
            //    //new_group = (Group)listPicker_group.SelectedItem;
            //    Database_Functions.AddMember(new_group.ID, textBox_name.Text, textBox_email.Text, textBox_phone.Text);
            //    NavigationService.Navigate(new Uri("/People.xaml", UriKind.Relative));
            //}    
            GlobalVars.member = new Member();
            GlobalVars.member.Name = textBox_name.Text;
            GlobalVars.member.Email = textBox_email.Text;
            GlobalVars.member.Phone = textBox_phone.Text;
            NavigationService.GoBack();
        }
        private void textBox_name_Tap(object sender, EventArgs e)
        {
            InputScope Keyboard = new InputScope();
            InputScopeName ScopeName = new InputScopeName();
            ScopeName.NameValue = InputScopeNameValue.Text;
            Keyboard.Names.Add(ScopeName);
            textBox_name.InputScope = Keyboard;
        }
        private void textBox_email_Tap(object sender, EventArgs e)
        {
            InputScope Keyboard = new InputScope();
            InputScopeName ScopeName = new InputScopeName();
            ScopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
            Keyboard.Names.Add(ScopeName);
            textBox_email.InputScope = Keyboard;
        }
        private void textBox_phone_Tap(object sender, EventArgs e)
        {
            InputScope Keyboard = new InputScope();
            InputScopeName ScopeName = new InputScopeName();
            ScopeName.NameValue = InputScopeNameValue.Digits;
            Keyboard.Names.Add(ScopeName);
            textBox_phone.InputScope = Keyboard;

        }
        //private bool checkMembers(string mem_name)
        //{
        //    foreach (Member mem in Database_Functions.GetAllMembers())
        //    {
        //        if (mem.Name.Equals(mem_name))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}