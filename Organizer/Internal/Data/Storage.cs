using Android.Views;
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

        private static DateTime _mainDate = DateTime.Now;
        private static DateTime _calendarDate = DateTime.Now;
        private static DateTime _scheduleDate = DateTime.Now;

        private static Period _mainPeriod = Period.Day;

        public static ListTasks MainListTasks => _mainListTasks;
        public static ListTasks CalendarListTasks => _calendarListTasks;
        public static ListTasks ScheduleListTasks => _scheduleListTasks;

        public static DateTime MainDate => _mainDate;
        public static DateTime CalendarDate => _calendarDate;
        public static DateTime ScheduleDate => _scheduleDate;

        public static Period MainPeriod => _mainPeriod;

        #endregion

        public static void InitializeListsTasks ()
        {
            _mainListTasks = GetList(Period.Day, DateTime.Now);
            _calendarListTasks = GetList(Period.Day, DateTime.Now);
            _scheduleListTasks = GetList(Period.Day, DateTime.Now);
        }

        public static void ChangeMainListTasks(Period period, DateTime date)
        {
            SetList(_mainPeriod, _mainDate, _mainListTasks);
            _mainPeriod = period;
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
    }
}