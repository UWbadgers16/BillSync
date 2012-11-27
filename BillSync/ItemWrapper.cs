using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BillSync
{
    public class ItemWrapper
    {
        public NewItem ItemPage
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Date
        {
            get;
            set;
        }

        public string Amount
        {
            get;
            set;
        }

        public string GroupName
        {
            get;
            set;
        }
    }
}
