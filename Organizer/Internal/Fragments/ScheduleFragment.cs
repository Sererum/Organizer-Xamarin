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
    public class ScheduleFragment : Fragment
    {
        private readonly Android.App.Activity _context;
        private readonly MainActivity _mainActivity;

        private TextView _periodTextView;
        private ImageButton _lastPeriodButton;
        private ImageButton _nextPeriodButton;
        private ListView _scheduleListView;

        public ScheduleFragment(Android.App.Activity context)
        {
            _context = context;
            _mainActivity = context as MainActivity;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_schedule, container, false);

            _periodTextView = view.FindViewById<TextView>(Resource.Id.SchedulePeriodTextView);
            _lastPeriodButton = view.FindViewById<ImageButton>(Resource.Id.ScheduleLastPeriodButton);
            _nextPeriodButton = view.FindViewById<ImageButton>(Resource.Id.ScheduleNextPeriodButton);
            _scheduleListView = view.FindViewById<ListView>(Resource.Id.ScheduleMainListView);

            _periodTextView.Text = NameDatePeriod.GetNameDate(DateTime.Now);
            _lastPeriodButton.Click += (s, e) => PeriodButton_Click(isNext: false);
            _nextPeriodButton.Click += (s, e) => PeriodButton_Click(isNext: true);

            UpdateListView();
            _scheduleListView.SetSelection(DateTime.Now.Hour);

            return view;
        }

        public void PeriodButton_Click (bool isNext)
        {
            int addition = isNext ? 1 : -1;
            Storage.ChangeScheduleListTasks(Storage.ScheduleDate.AddDays(addition));
            _periodTextView.Text = NameDatePeriod.GetNameDate(Storage.ScheduleDate);
            UpdateListView();
        }

        public void UpdateListView ()
        {
            int firstVisiblePosition = _scheduleListView.FirstVisiblePosition;
            _scheduleListView.Adapter = new ScheduleArrayAdapter(_context, Storage.ScheduleListTasks);
            _scheduleListView.SetSelection(firstVisiblePosition);
        }
    }
}