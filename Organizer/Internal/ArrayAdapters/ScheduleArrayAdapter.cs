using Android.Graphics;
using Android.Views;
using Android.Widget;
using Java.Util;
using Organizer.Internal.Activity;
using Organizer.Internal.Data;
using Organizer.Internal.Model.Task;
using System;
using static Android.App.ActionBar;

namespace Organizer.Internal.ArrayAdapters
{
    class ScheduleArrayAdapter : BaseAdapter<string>
    {
        private readonly Android.App.Activity _context;
        private readonly MainActivity _mainActivity;

        private long _periodClick;

        public ScheduleArrayAdapter (Android.App.Activity context)
        {
            _context = context;
            _mainActivity = context as MainActivity;
            _periodClick = (new Date()).Time;
        }
        public override string this[int position] => GetSHour(position);

        public override int Count => 24;

        public override long GetItemId (int position) => position;

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view is null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_schedule, null);
            }

            RelativeLayout mainLayout = view.FindViewById<RelativeLayout>(Resource.Id.ScheduleItemMainLayout);
            TextView lineTextView = view.FindViewById<TextView>(Resource.Id.ScheduleItemLineTextView);
            TextView hourTextView = view.FindViewById<TextView>(Resource.Id.ScheduleItemHourTextView);
            LinearLayout tasksLayout = view.FindViewById<LinearLayout>(Resource.Id.ScheduleItemListLayout);

            Color textColor = Storage.GetColor(_mainActivity.Designer.GetIdTextColor());

            lineTextView.SetBackgroundColor(textColor);
            hourTextView.SetTextColor(textColor);

            if (position == DateTime.Now.Hour)
            {
                mainLayout.SetBackgroundColor(Storage.GetColor(_mainActivity.Designer.GetIdDownPanelColor()));
            }
            else
            {
                mainLayout.SetBackgroundColor(Storage.GetColor(_mainActivity.Designer.GetIdMainColor()));
            }

            string sStartHour = GetSHour(position);
            string sEndHour = GetSHour(position + 1);
            hourTextView.Text = sStartHour;

            tasksLayout.RemoveAllViews();
            Storage.ScheduleListTasks.Sort();

            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            layoutParams.SetMargins(4, 7, 14, 5);
            bool isPast = Storage.IsPast(Storage.ScheduleDate);

            foreach (BaseTask task in Storage.ScheduleListTasks)
            {
                if (task.StartTime != "" && task.EndTime != "" && HourContainsTask(task, sStartHour, sEndHour))
                {
                    TaskViewConstructor constructor = new TaskViewConstructor(_context, isPast);
                    tasksLayout.AddView(constructor.GetTaskView(task), layoutParams);
                }
            }

            view.Click += (s, e) =>
            {
                long oldTime = _periodClick;
                _periodClick = (new Date()).Time;
                if (_periodClick - oldTime < 1000)
                {
                    return;
                }
                _mainActivity.ShowCreateFragment(Storage.ScheduleListTasks, scheduleHour: position);
            };

            view.SetOnTouchListener(new OnSwipeTouchListener(_mainActivity));

            return view;
        }

        private string GetSHour (int hour) => Storage.TimeToStandart(hour, 0);

        private bool HourContainsTask (BaseTask task, string startHour, string endHour)
        {
            if (TaskSorter.FirstEarlierOrEquals(task.StartTime, task.EndTime))
            {
                bool endHourEarlierStartTask = TaskSorter.FirstEarlierOrEquals(endHour, task.StartTime);
                bool endTimeEarlierStartHour = TaskSorter.FirstEarlierOrEquals(task.EndTime, startHour);
                return (endHourEarlierStartTask == false && endTimeEarlierStartHour == false);
            }
            else
            {
                bool startHourEarlierEndTask = TaskSorter.FirstEarlierOrEquals(startHour, task.EndTime);
                bool startTaskEarlierEndHour = TaskSorter.FirstEarlierOrEquals(task.StartTime, endHour);
                return (startHourEarlierEndTask || startTaskEarlierEndHour);
            }
        }
    }
}