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
using System.Collections.Generic;

namespace BillSync
{
    public class Item2
    {
        private int id;
        private int group_id;
        private String title;
        private String description;
        private List<Member> members;
        //private List<Group> groups;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int Group_ID
        {
            get { return group_id; }
            set { group_id = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public List<Member> Members
        {
            get { return members; }
            set { members = value; }
        }

        //public List<Group> Groups
        //{
        //    get { return groups; }
        //    set { groups = value; }
        //}

        public void addMember(Member newMember)
        {
            members.Add(newMember);
        }

        //public void addGroup(Group newGroup)
        //{
        //    groups.Add(newGroup);
        //}
    }
}
