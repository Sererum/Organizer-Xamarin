using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Organizer.Internal.Model.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Organizer.Internal.Data
{
    class TaskSorter
    {
        public enum Type { TimeStart, TypeTask }

        private static Type _currentType = (Type) Server.SortType;

        public static Type CurrentType
        {
            get { return _currentType; }
            set
            {
                _currentType = value;
                Server.SortType = (int) value;
            }
        }

        public static int Compare (BaseTask taskOne, BaseTask taskTwo)
        {
            int compare = 0;

            compare += taskOne.Complete.CompareTo(taskTwo.Complete) * GetRate(4);

            switch (_currentType)
            {
                case Type.TypeTask:
                    compare += CompareType(taskOne, taskTwo) * GetRate(3);
                    break;
                case Type.TimeStart:
                    compare += CompareTime(taskOne.StartTime, taskTwo.StartTime) * -GetRate(3);
                    compare += CompareType(taskOne, taskTwo) * GetRate(2);
                    break;
            }

            if (taskOne.TypeTask == (int) BaseTask.Type.Regular && taskOne.TypeTask == taskTwo.TypeTask)
            {
                compare += (taskOne as Regular).Priority.CompareTo((taskTwo as Regular).Priority) * -GetRate(1);
            }
            compare += taskOne.Title.CompareTo(taskTwo.Title) * GetRate(0);

            return compare;
        }

        public static int CompareType(BaseTask taskOne, BaseTask taskTwo)
        {
            if (taskOne is Project || taskTwo is Project)
            {
                return (taskOne is Project && taskTwo is Project) ? 0 : (taskOne is Project ? -1 : 1);
            }
            if (taskOne is Routine || taskTwo is Routine)
            {
                return (taskOne is Routine && taskTwo is Routine) ? 0 : (taskOne is Routine ? -1 : 1);
            }
            if (taskOne is Regular || taskTwo is Regular)
            {
                return (taskOne is Regular && taskTwo is Regular) ? 0 : (taskOne is Regular ? -1 : 1);
            }
            throw new ArgumentException();
        }

        public static int CompareTime (string sTimeOne, string sTimeTwo)
        {
            if (sTimeOne == "" || sTimeTwo == "")
            {
                return (sTimeOne == sTimeTwo ? 0 : (sTimeOne == "" ? -1 : 1));
            }
            int hourOne = Int32.Parse(sTimeOne.Substring(0, 2));
            int minuteOne = Int32.Parse(sTimeOne.Substring(3, 2));
            DateTime timeOne = DateTime.MinValue.AddHours(hourOne).AddMinutes(minuteOne);
            int hourTwo = Int32.Parse(sTimeTwo.Substring(0, 2));
            int minuteTwo = Int32.Parse(sTimeTwo.Substring(3, 2));
            DateTime timeTwo = DateTime.MinValue.AddHours(hourTwo).AddMinutes(minuteTwo);
            return -timeOne.CompareTo(timeTwo);
        }

        private static int GetRate (int position) => (int) Math.Pow(2, position);

    }
}