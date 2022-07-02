using Organizer.Internal.Activity;
using System;
using static Organizer.Internal.Data.Server;

namespace Organizer.Internal.Data
{
    class NameDatePeriod
    {
        public static string GetNameDate (DateTime date, Period period = Period.Day)
        {
            string nameDate = "";
            Android.App.Activity context = Storage.Context;
            MainActivity mainActivity = context as MainActivity;
            DateTime now = DateTime.Now;
            switch (period)
            {
                case Period.Day:
                    if (Storage.EqualsDate(date, now.AddDays(-1)))
                    {
                        nameDate += mainActivity.Translater.GetString(Resource.String.last_day) + ", ";
                    }
                    if (Storage.EqualsDate(date, now))
                    {
                        nameDate += mainActivity.Translater.GetString(Resource.String.this_day) + ", ";
                    }
                    if (Storage.EqualsDate(date, now.AddDays(1)))
                    {
                        nameDate += mainActivity.Translater.GetString(Resource.String.next_day) + ", ";
                    }
                    nameDate += date.Day + "/" + date.Month;
                    nameDate += ", " + mainActivity.Translater.GetString(Storage.DayWeekToNameId[(int) date.DayOfWeek]);
                    break;
                case Period.Month:
                    if (Storage.EqualsDate(date, now.AddMonths(-1)))
                    {
                        nameDate += mainActivity.Translater.GetString(Resource.String.last_month) + ", ";
                    }
                    if (Storage.EqualsDate(date, now))
                    {
                        nameDate += mainActivity.Translater.GetString(Resource.String.this_month) + ", ";
                    }
                    if (Storage.EqualsDate(date, now.AddMonths(1)))
                    {
                        nameDate += mainActivity.Translater.GetString(Resource.String.next_month) + ", ";
                    }
                    nameDate += Server.GetKey(date, Period.Month);
                    break;
                case Period.Year:
                    if (Storage.EqualsDate(date, now.AddYears(-1)))
                    {
                        nameDate += mainActivity.Translater.GetString(Resource.String.last_year) + ", ";
                    }
                    if (Storage.EqualsDate(date, now))
                    {
                        nameDate += mainActivity.Translater.GetString(Resource.String.this_year) + ", ";
                    }
                    if (Storage.EqualsDate(date, now.AddYears(1)))
                    {
                        nameDate += mainActivity.Translater.GetString(Resource.String.next_year) + ", ";
                    }
                    nameDate += Server.GetKey(date, Period.Year);
                    break;
                case Period.Global:
                    nameDate += mainActivity.Translater.GetString(Resource.String.global);
                    break;
            }
            return nameDate;
        }
    }
}