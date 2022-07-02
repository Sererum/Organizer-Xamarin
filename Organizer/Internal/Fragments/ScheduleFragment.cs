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
        private LinearLayout _scheduleLayout;

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
            _scheduleLayout = view.FindViewById<LinearLayout>(Resource.Id.ScheduleMainLinearLayout);

            _periodTextView.Text = NameDatePeriod.GetNameDate(DateTime.Now);
            _lastPeriodButton.Click += (s, e) => PeriodButton_Click(isNext: false);
            _nextPeriodButton.Click += (s, e) => PeriodButton_Click(isNext: true);

            UpdateListView();

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
            _periodTextView.Text = NameDatePeriod.GetNameDate(Storage.ScheduleDate);

            Storage.ScheduleListTasks.Sort();
            _scheduleLayout.RemoveAllViews();

            for (int i = 0; i < 24; i++)
            {
                ScheduleViewConstructor constructor = new ScheduleViewConstructor(_context);
                _scheduleLayout.AddView(constructor.GetView(i));
            }
        }
    }
}