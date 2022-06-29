using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Organizer.Internal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Organizer.Internal.Data
{
    public static class Server
    {
        public static ListTasks ListTasks { get; set; }

        public static void SetListTasks ()
        {
            ListTasks list = new ListTasks();

            list += new ListTasks("0|P1||09:00||0|1|2|^0|P2||||0|1|1|^1|Reg|text of reg|||0|5^1|Reg||00:00||0|5");
            list += new ListTasks("1|Reg|text of reg||23:59|0|9");
            list += new ListTasks("2|Rout||10:00|11:00|0|0");

            list += new ListTasks("1|Reg9||||0|9");
            list += new ListTasks("1|Reg5||||0|5");
            list += new ListTasks("1|Reg1||||0|1");

            ListTasks = list;
        }
    }
}