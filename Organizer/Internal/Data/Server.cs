using Android.App;
using Android.Content;
using Organizer.Internal.Model;
using Organizer.Internal.Model.Task;
using System;

namespace Organizer.Internal.Data
{
    public static class Server
    {
        public enum Period { Global, Year, Month, Day }
        private enum Key { Global, Routine, Sort, Language, Theme }
        private static ISharedPreferences _preferences = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
        private static ISharedPreferencesEditor _preferencesEdit = _preferences.Edit();

        public static ListTasks Routines
        {
            get { return new ListTasks(_preferences.GetString(Key.Routine.ToString(), "")); }
            set { _preferencesEdit.PutString(Key.Routine.ToString(), value.Archive()).Commit(); }
        }

        public static int SortType
        {
            get { return _preferences.GetInt(Key.Sort.ToString(), 0); }
            set { _preferencesEdit.PutInt(Key.Sort.ToString(), value).Commit(); }
        }

        public static int Language
        {
            get { return _preferences.GetInt(Key.Language.ToString(), 0); }
            set { _preferencesEdit.PutInt(Key.Language.ToString(), value).Commit(); }
        }

        public static int Theme
        {
            get { return _preferences.GetInt(Key.Theme.ToString(), 0); }
            set { _preferencesEdit.PutInt(Key.Theme.ToString(), value).Commit(); }
        }

        public static ListTasks GetList (Period period, DateTime date)
        {
            switch (period)
            {
                case Period.Day:
                    ListTasks dayList = new ListTasks(_preferences.GetString(GetKey(date, Period.Day), ""));
                    if (Storage.IsPast(date) == false)
                    {
                        dayList += GetRoutinesOnDay((int) date.DayOfWeek, dayList);
                    }
                    return dayList;
                case Period.Month:
                    return new ListTasks(_preferences.GetString(GetKey(date, Period.Month), ""));
                case Period.Year:
                    return new ListTasks(_preferences.GetString(GetKey(date, Period.Year), ""));
                case Period.Global:
                    return new ListTasks(_preferences.GetString(Key.Global.ToString(), ""));
            }
            throw new ArgumentException();
        }

        public static void SetList (Period period, DateTime date, ListTasks list)
        {
            switch (period)
            {
                case Period.Day:
                    _preferencesEdit.PutString(GetKey(date, Period.Day), list.Archive());
                    break;
                case Period.Month:
                    _preferencesEdit.PutString(GetKey(date, Period.Month), list.Archive());
                    break;
                case Period.Year:
                    _preferencesEdit.PutString(GetKey(date, Period.Year), list.Archive());
                    break;
                case Period.Global:
                    _preferencesEdit.PutString(Key.Global.ToString(), list.Archive());
                    break;
                default:
                    throw new ArgumentException();
            }
            _preferencesEdit.Commit();
        }

        public static ListTasks GetRoutinesOnDay(int dayOfWeek, ListTasks list)
        {
            ListTasks routinesOnDay = new ListTasks();
            ListTasks listRoutines = list.CutRoutines();
            foreach (Routine routine in Routines)
            {
                if (routine.SDays.Contains(dayOfWeek.ToString()))
                {
                    foreach (BaseTask listRoutine in listRoutines)
                    {
                        if (routine.Equals(listRoutine))
                        {
                            routinesOnDay.Add(listRoutine);
                        }
                    }
                    if (routinesOnDay.Contains(routine) == false)
                    {
                        routinesOnDay.Add(routine);
                    }
                }
            }
            return routinesOnDay;
        }

        public static string GetKey (DateTime date, Period period)
        {
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;

            switch (period)
            {
                case Period.Day:
                    return day + "/" + month + "/" + year;
                case Period.Month:
                    return month + "/" + year;
                case Period.Year:
                    return year.ToString();
            }
            throw new ArgumentException();
        }
    }
}