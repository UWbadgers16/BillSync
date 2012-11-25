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

namespace BillSync
{
    public class ItemState
    {
        private string desciption;
        private int total;
        private List<string> members;
        private List<decimal> amounts;

        public string Description
        {
            get { return desciption; }
            set { desciption = value; }
        }

        public int Total
        {
            get { return total; }
            set { total = value; }
        }

        public List<string> Members
        {
            get { return members; }
            set { members = value; }
        }

        public List<decimal> Amounts
        {
            get { return amounts; }
            set { amounts = value; }
        }
    }
}
