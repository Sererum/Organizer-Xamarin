using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Organizer.Internal.Activity;
using Organizer.Internal.ArrayAdapters;
using Organizer.Internal.Data;
using System;

namespace Organizer.Internal.Fragments
{
    public class CalendarFragment : Fragment
    {
        private readonly Android.App.Activity _context;
        private readonly MainActivity _mainActivity;

        private CalendarView _calendarView;
        private ImageButton _hideButton;
        private ListView _tasksListView;
        private ImageButton _addButton;

        public CalendarFragment(Android.App.Activity context)
        {
            _context = context;
            _mainActivity = context as MainActivity;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_calendar, container, false);

            _calendarView = view.FindViewById<CalendarView>(Resource.Id.CalendarMainView);
            _hideButton = view.FindViewById<ImageButton>(Resource.Id.CalendarHideTasksButton);
            _tasksListView = view.FindViewById<ListView>(Resource.Id.CalendarListView);
            _addButton = view.FindViewById<ImageButton>(Resource.Id.CalendarAddTaskButton);

            _calendarView.DateChange += (s, e) => Calendar_DateChange(e.Year, e.Month + 1, e.DayOfMonth);
            _hideButton.Click += (s, e) => HideButton_Click(_calendarView.Visibility == ViewStates.Visible);
            _addButton.Click += (s, e) => _mainActivity.ShowCreateFragment(Storage.CalendarListTasks);

            UpdateListView();

            return view;
        }

        private void HideButton_Click (bool isHideCalendar)
        {
            _calendarView.Visibility = isHideCalendar ? ViewStates.Gone : ViewStates.Visible;
        }

        private void Calendar_DateChange (int year, int month, int day)
        {
            Storage.ChangeCalendarListTasks(new DateTime(year, month, day));
            _addButton.Visibility = Storage.IsPast(Storage.CalendarDate) ? ViewStates.Gone : ViewStates.Visible;
            UpdateListView();
        }

        public void UpdateListView ()
        {
            Storage.CalendarListTasks.Sort();
            _tasksListView.Adapter = new TaskArrayAdapter(_context, Storage.CalendarListTasks);
        }
    }
}