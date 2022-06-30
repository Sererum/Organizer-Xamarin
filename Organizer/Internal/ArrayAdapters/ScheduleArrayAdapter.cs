using Android.Graphics;
using Android.Views;
using Android.Widget;
using Organizer.Internal.Data;
using Organizer.Internal.Model;
using Organizer.Internal.Model.Task;
using System;
using static Android.App.ActionBar;

namespace Organizer.Internal.ArrayAdapters
{
    class ScheduleArrayAdapter : BaseAdapter<string>
    {
        private readonly Android.App.Activity _context;
        private readonly ListTasks _listTasks;

        public ScheduleArrayAdapter(Android.App.Activity context, ListTasks listTasks)
        {
            _context = context;
            _listTasks = listTasks;
        }

        public override string this[int position] => position.ToString();

        public override int Count => 24;

        public override long GetItemId (int position) => position;

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view is null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.list_tasks_schedule, null);
            }

            TextView lineTextView = view.FindViewById<TextView>(Resource.Id.ScheduleItemLineTextView);
            TextView hourTextView = view.FindViewById<TextView>(Resource.Id.ScheduleItemHourTextView);
            LinearLayout tasksLayout = view.FindViewById<LinearLayout>(Resource.Id.ScheduleItemListLayout);

            if (position == DateTime.Now.Hour || position == DateTime.Now.Hour + 1)
            {
                lineTextView.SetBackgroundColor(Color.Red);
            }
            else
            {
                lineTextView.SetBackgroundColor(Color.Black);
            }
            string sStartHour = (position < 10 ? "0" : "") + position + ":00";
            string sEndHour = (position < 9 ? "0" : "") + (position + 1) + ":00";
            hourTextView.Text = sStartHour;

            tasksLayout.RemoveAllViews();
            _listTasks.Sort();

            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            layoutParams.SetMargins(0, 4, 0, 2);
            foreach (BaseTask task in _listTasks)
            {
                bool startTaskLaterEndHour = TaskSorter.CompareTime(sEndHour, task.StartTime) >= 0;
                bool startHourLaterEndTask = TaskSorter.CompareTime(task.EndTime, sStartHour) >= 0;
                if ((startTaskLaterEndHour || startHourLaterEndTask) == false)
                {
                    tasksLayout.AddView(TaskViewConstructor.GetTaskView(task, null), layoutParams);
                }
            }

            return view;
        }
    }
}