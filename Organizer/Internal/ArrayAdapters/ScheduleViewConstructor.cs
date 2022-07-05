﻿using Android.Graphics;
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
    public class ScheduleViewConstructor
    {
        private readonly Android.App.Activity _context;
        private readonly MainActivity _mainActivity;
        private long _periodClick;

        public ScheduleViewConstructor (Android.App.Activity context)
        {
            _context = context;
            _mainActivity = context as MainActivity;
            _periodClick = (new Date()).Time;
        }

        public View GetView (int hour)
        {
            View view = _context.LayoutInflater.Inflate(Resource.Layout.list_tasks_schedule, null);

            RelativeLayout mainLayout = view.FindViewById<RelativeLayout>(Resource.Id.ScheduleItemMainLayout);
            TextView lineTextView = view.FindViewById<TextView>(Resource.Id.ScheduleItemLineTextView);
            TextView hourTextView = view.FindViewById<TextView>(Resource.Id.ScheduleItemHourTextView);
            LinearLayout tasksLayout = view.FindViewById<LinearLayout>(Resource.Id.ScheduleItemListLayout);

            Color textColor = Storage.GetColor(_mainActivity.Designer.GetIdTextColor());

            lineTextView.SetBackgroundColor(textColor);
            hourTextView.SetTextColor(textColor);

            if (hour == DateTime.Now.Hour)
            {
                mainLayout.SetBackgroundColor(Storage.GetColor(_mainActivity.Designer.GetIdDownPanelColor()));
            }
            else
            {
                mainLayout.SetBackgroundColor(Storage.GetColor(_mainActivity.Designer.GetIdMainColor()));
            }

            string sStartHour = (hour < 10 ? "0" : "") + hour + ":00";
            string sEndHour = (hour < 9 ? "0" : "") + (hour + 1) + ":00";
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
                if ((new Date()).Time - _periodClick < 500)
                {
                    return;
                }
                _periodClick = (new Date()).Time;
                _mainActivity.ShowCreateFragment(Storage.ScheduleListTasks, scheduleHour: hour);
            };

            return view;
        }

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