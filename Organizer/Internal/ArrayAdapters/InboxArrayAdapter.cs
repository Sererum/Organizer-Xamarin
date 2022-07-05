using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Organizer.Internal.Data;
using Organizer.Internal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Organizer.Internal.ArrayAdapters
{
    class InboxArrayAdapter : BaseAdapter<string>
    {
        private readonly Android.App.Activity _context;
        private readonly ListTasks _listTasks;

        public InboxArrayAdapter (Android.App.Activity context)
        {
            _context = context;
            _listTasks = Storage.InboxListTasks;
        }

        public override string this[int position] => _listTasks[position].ToString();

        public override int Count => _listTasks.Count;

        public override long GetItemId (int position) => position;

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            return new TaskViewConstructor(_context, isInbox: true).GetTaskView(_listTasks[position]);
        }
    }
}