﻿using System;
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
    public partial class SelectMembers : PhoneApplicationPage
    {
        string msg;

        public SelectMembers()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            msg = NavigationContext.QueryString["msg"];
            memberSelectList.ItemsSource = GlobalVars.members;
            GlobalVars.members = null;
        }

        private void ApplicationBarSelectButton_Click(object sender, EventArgs e)
        {
            GlobalVars.selectedMembers = (IList<Member>)memberSelectList.SelectedItems;
            GlobalVars.selectMode = msg;
            NavigationService.GoBack();
        }

    }
}