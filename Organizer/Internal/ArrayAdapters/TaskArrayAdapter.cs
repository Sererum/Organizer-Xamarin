using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Organizer.Internal.Model;
using Organizer.Internal.Model.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Organizer.Internal.ArrayAdapters
{
    class TaskArrayAdapter : BaseAdapter<BaseTask>
    {
        Android.App.Activity _context;
        private readonly ListTasks _showList;
        private readonly ListTasks _mainList;

        public override int Count => _showList.Count;

        public override BaseTask this[int position] => _showList[position];

        public TaskArrayAdapter (Android.App.Activity context, ListTasks showList, ListTasks mainList)
        {
            _context = context;
            _showList = showList;
            if (showList is null)
            {
                _showList = mainList;
            }
            _mainList = mainList;
            var s = _mainList.Archive(ListTasks.Mode.All);
            TaskViewConstructor.InitialConstructor(_context);
        }

        public override long GetItemId (int position) => position;

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            return TaskViewConstructor.GetTaskView(_showList[position], convertView);
        }
    }
}