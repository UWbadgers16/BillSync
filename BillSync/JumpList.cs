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
    public class JumpList
    {
        public string Name
        {
            get;
            set;
        }


        public string GroupBy
        {
            get;
            set;
        }
         public string SelectedComponentImage
        {
            get;
            set;
        }
        public string GroupHeader
        {
            get
            {
                switch (Name.ToLower().Substring(0, 1))
                {
                    case "a": return "a";
                    case "b": return "b";
                    case "c": return "c";
                    case "d": return "d";
                    case "e": return "e";
                    case "f": return "f";
                    case "g": return "g";
                    case "h": return "h";
                    case "i": return "i";
                    case "j": return "j";
                    case "k": return "k";
                    case "l": return "l";
                    case "m": return "m";
                    case "n": return "n";
                    case "o": return "o";
                    case "p": return "p";
                    case "q": return "q";
                    case "r": return "r";
                    case "s": return "s";
                    case "t": return "t";
                    case "u": return "u";
                    case "v": return "v";
                    case "w": return "w";
                    case "x": return "x";
                    case "y": return "y";
                    case "z": return "z";
                    default: return "#";
                }
            }
        }


    }
}
