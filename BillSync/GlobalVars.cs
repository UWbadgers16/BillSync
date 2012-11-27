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
using System.Windows.Controls.Primitives;
using System.Collections.Generic;

namespace BillSync
{
    public class GlobalVars
    {
        public static NewItem item;
        public static NewItem editItem;
        public static IList<NewItem> items;
        public static NewGroup group;
        public static int group_id = -1;
    }
}
