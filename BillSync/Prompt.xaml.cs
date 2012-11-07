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

namespace BillSync
{
    public partial class Prompt : UserControl
    {
        public enum Type {Group, Bill};
        private Boolean hasNew = false;
        private Boolean defaultMessage = false;

        public Prompt(Prompt.Type type)
        {
            InitializeComponent();
            switch (type)
            {
                case Prompt.Type.Group:
                    textBox_name.Text = "enter group name";
                    button_create.Content = "create new group";
                    break;
                case Prompt.Type.Bill:
                    textBox_name.Text = "enter bill name";
                    button_create.Content = "create new bill";
                    break;
            }
            defaultMessage = true;
        }

        public Boolean HasNew
        {
            get { return hasNew; }
        }
        
        public String Title
        {
            get { return textBox_name.Text; }
        }

        private void textBox_name_Tap(object sender, GestureEventArgs e)
        {
            if (defaultMessage)
            {
                textBox_name.Text = "";
                defaultMessage = false;
            }
        }

    }
}
