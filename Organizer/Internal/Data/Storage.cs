using Android.Views;
using System;
using System.Collections.Generic;

namespace Organizer.Internal.Data
{
    public static class Storage
    {
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
    }
}