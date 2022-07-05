using Android.Graphics;
using Android.Util;
using AndroidX.Core.Content;
using AndroidX.Fragment.App;
using Organizer.Internal.Fragments;
using Organizer.Internal.Model;
using System;
using System.Collections.Generic;
using static Organizer.Internal.Data.Server;

namespace Organizer.Internal.Data
{
    public static class Storage
    {
        #region Fields and Properties

        private static ListTasks _mainListTasks;
        private static ListTasks _calendarListTasks;
        private static ListTasks _scheduleListTasks;
        private static ListTasks _inboxListTasks;

        private static DateTime _mainDate = DateTime.Now;
        private static DateTime _calendarDate = DateTime.Now;
        private static DateTime _scheduleDate = DateTime.Now;

        private static Period _mainPeriod = Period.Day;

        private static DateTime _dayDate = DateTime.Now;
        private static DateTime _monthDate = DateTime.Now;
        private static DateTime _yearDate = DateTime.Now;

        public static ListTasks MainListTasks => _mainListTasks;
        public static ListTasks CalendarListTasks => _calendarListTasks;
        public static ListTasks ScheduleListTasks => _scheduleListTasks;
        public static ListTasks InboxListTasks => _inboxListTasks;

        public static DateTime MainDate => _mainDate;
        public static DateTime CalendarDate => _calendarDate;
        public static DateTime ScheduleDate => _scheduleDate;

        public static Period MainPeriod => _mainPeriod;

        public static DateTime DayDate => _dayDate;
        public static DateTime MonthDate => _monthDate;
        public static DateTime YearDate => _yearDate;

        public static Android.App.Activity Context { get; set; }

        #endregion

        public static void InitializeListsTasks (Android.App.Activity context)
        {
            LoadLastTasks();
            _mainListTasks = GetList(Period.Day, DateTime.Now);
            _calendarListTasks = GetList(Period.Day, DateTime.Now);
            _scheduleListTasks = GetList(Period.Day, DateTime.Now);
            _inboxListTasks = InboxList;
            Context = context;
        }

        public static void ChangeMainListTasks(Period period, DateTime date)
        {
            SetList(_mainPeriod, _mainDate, _mainListTasks);
            _mainPeriod = period;

            _dayDate = period == Period.Day ? date : _dayDate;
            _monthDate = period == Period.Month ? date : _monthDate;
            _yearDate = period == Period.Year ? date : _yearDate;

            _mainDate = date;
            _mainListTasks = GetList(_mainPeriod, _mainDate);
        }

        public static void ChangeCalendarListTasks (DateTime date)
        {
            SetList(Period.Day, _calendarDate, _calendarListTasks);
            _calendarDate = date;
            _calendarListTasks = GetList(Period.Day, _calendarDate);
        }

        public static void ChangeScheduleListTasks (DateTime date)
        {
            SetList(Period.Day, _scheduleDate, _scheduleListTasks);
            _scheduleDate = date;
            _scheduleListTasks = GetList(Period.Day, _scheduleDate);
        }

        public static void SaveListsTasks ()
        {
            SetList(_mainPeriod, _mainDate, _mainListTasks);
            SetList(Period.Day, _calendarDate, _calendarListTasks);
            SetList(Period.Day, _scheduleDate, _scheduleListTasks);
            InboxList = _inboxListTasks;
        }

        public static void SynchronizeLists (Fragment currentFragment)
        {
            if (currentFragment is ListTasksFragment && MainPeriod == Period.Day)
            {
                _mainListTasks += GetRoutinesOnDay((int) MainDate.DayOfWeek, _mainListTasks);
                if (EqualsDate(CalendarDate, MainDate))
                {
                    _calendarListTasks = MainListTasks;
                }
                if (EqualsDate(ScheduleDate, MainDate))
                {
                    _scheduleListTasks = MainListTasks;
                }
            }
            if (currentFragment is CalendarFragment)
            {
                _calendarListTasks += GetRoutinesOnDay((int) CalendarDate.DayOfWeek, _calendarListTasks);
                if (EqualsDate(MainDate, CalendarDate) && MainPeriod == Period.Day)
                {
                    _mainListTasks = CalendarListTasks;
                }
                if (EqualsDate(ScheduleDate, CalendarDate))
                {
                    _scheduleListTasks = CalendarListTasks;
                }
            }
            if (currentFragment is ScheduleFragment)
            {
                _scheduleListTasks += GetRoutinesOnDay((int) ScheduleDate.DayOfWeek, _scheduleListTasks);
                if (EqualsDate(MainDate, ScheduleDate) && MainPeriod == Period.Day)
                {
                    _mainListTasks = ScheduleListTasks;
                }
                if (EqualsDate(CalendarDate, ScheduleDate))
                {
                    _calendarListTasks = ScheduleListTasks;
                }
            }
            if (currentFragment is InboxFragment)
            {
                if (EqualsDate(MainDate, DateTime.Now))
                {
                    _mainListTasks = GetList(MainPeriod, DateTime.Now);
                }
                if (EqualsDate(CalendarDate, DateTime.Now))
                {
                    _calendarListTasks = GetList(Period.Day, DateTime.Now);
                }
                if (EqualsDate(ScheduleDate, DateTime.Now))
                {
                    _scheduleListTasks = GetList(Period.Day, DateTime.Now);
                }
            }
        }

        private static void LoadLastTasks ()
        {
            ListTasks nowDayList = GetList(Period.Day, DateTime.Now);
            DateTime dayDate = DateTime.Now.AddDays(-1);
            for (int i = 1; i <= 7; i++)
            {
                ListTasks lastList = GetList(Period.Day, dayDate);
                nowDayList += lastList.CutUncompleteTask();
                SetList(Period.Day, dayDate, lastList);

                dayDate = dayDate.AddDays(-1);
            }
            SetList(Period.Day, DateTime.Now, nowDayList);

            if (DateTime.Now.Day <= 7)
            {
                ListTasks nowMonthList = GetList(Period.Month, DateTime.Now);
                ListTasks lastMonthList = GetList(Period.Month, DateTime.Now.AddMonths(-1));
                nowMonthList += lastMonthList.CutUncompleteTask();
                SetList(Period.Month, DateTime.Now, nowMonthList);
                SetList(Period.Month, DateTime.Now.AddMonths(-1), lastMonthList);
            }

            if (DateTime.Now.Month == 1)
            {
                ListTasks nowYearList = GetList(Period.Year, DateTime.Now);
                ListTasks lastYearList = GetList(Period.Year, DateTime.Now.AddYears(-1));
                nowYearList += lastYearList.CutUncompleteTask();
                SetList(Period.Year, DateTime.Now, nowYearList);
                SetList(Period.Year, DateTime.Now.AddYears(-1), lastYearList);
            }
        }


        #region Dictionaries

        public static Dictionary<int, int> PriorityToColorId = new Dictionary<int, int>()
        {
            { 1, Resource.Color.priority_one },
            { 2, Resource.Color.priority_two },
            { 3, Resource.Color.priority_three },
            { 4, Resource.Color.priority_four },
            { 5, Resource.Color.priority_five },
            { 6, Resource.Color.priority_six },
            { 7, Resource.Color.priority_seven },
            { 8, Resource.Color.priority_eight },
            { 9, Resource.Color.priority_nine },
        };

        public static Dictionary<int, int> DayWeekToNameId = new Dictionary<int, int>()
        {
            {0, Resource.String.sunday_short },
            {1, Resource.String.monday_short },
            {2, Resource.String.tuesday_short },
            {3, Resource.String.wednesday_short },
            {4, Resource.String.thursday_short },
            {5, Resource.String.friday_short },
            {6, Resource.String.saturday_short },
        };

        #endregion

        #region Public methods

        public static float DpToPx (int dp) => TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, Context.Resources.DisplayMetrics);

        public static string TimeToStandart (int hour, int minute) => (hour <= 9 ? "0" : "") + hour + ":" + (minute <= 9 ? "0" : "") + minute;

        public static bool EqualsDate (DateTime dateOne, DateTime dateTwo) => dateOne.ToShortDateString() == dateTwo.ToShortDateString();

        public static bool EqualsMonths (DateTime dateOne, DateTime dateTwo) => dateOne.Month == dateTwo.Month && dateOne.Year == dateTwo.Year;

        public static bool EqualsYears(DateTime dateOne, DateTime dateTwo) => dateOne.Year == dateTwo.Year;

        public static bool IsPast (DateTime date)
            => DateTime.Now.CompareTo(date) > 0 && EqualsDate(DateTime.Now, date) == false;

        public static Color GetColor(int id) => new Color(ContextCompat.GetColor(Context, id));

        #endregion
    }
}