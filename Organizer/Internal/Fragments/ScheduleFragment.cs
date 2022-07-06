using Android.Graphics;
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

        private RelativeLayout _toolBarLayout;
        private RelativeLayout _periodLayout;
        private TextView _periodTextView;
        private ImageButton _lastPeriodButton;
        private ImageButton _nextPeriodButton;
        private ListView _listView;

        private int _countHidden = 0;

        public ScheduleFragment(Android.App.Activity context)
        {
            _context = context;
            _mainActivity = context as MainActivity;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_schedule, container, false);

            _toolBarLayout = view.FindViewById<RelativeLayout>(Resource.Id.ScheduleToolBarLayout);
            _periodLayout = view.FindViewById<RelativeLayout>(Resource.Id.SchedulePeriodLayout);
            _periodTextView = view.FindViewById<TextView>(Resource.Id.SchedulePeriodTextView);
            _lastPeriodButton = view.FindViewById<ImageButton>(Resource.Id.ScheduleLastPeriodButton);
            _nextPeriodButton = view.FindViewById<ImageButton>(Resource.Id.ScheduleNextPeriodButton);
            _listView = view.FindViewById<ListView>(Resource.Id.ScheduleMainListView);

            _periodTextView.Text = NameDatePeriod.GetNameDate(DateTime.Now);
            _lastPeriodButton.Click += (s, e) => PeriodButton_Click(isNext: false);
            _nextPeriodButton.Click += (s, e) => PeriodButton_Click(isNext: true);

            PaintViews();

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
            int firstVisiblePosition = _listView.FirstVisiblePosition;
            _listView.Adapter = new ScheduleArrayAdapter(_context);
            _listView.SetSelection(firstVisiblePosition);
        }

        public void PaintViews ()
        {
            Color textColor = Storage.GetColor(_mainActivity.Designer.GetIdTextColor());
            PorterDuffColorFilter textFilter = new PorterDuffColorFilter(textColor, PorterDuff.Mode.SrcAtop);
            Color toolBarColor = Storage.GetColor(_mainActivity.Designer.GetIdToolBarColor());
            PorterDuffColorFilter toolBarFilter = new PorterDuffColorFilter(toolBarColor, PorterDuff.Mode.SrcAtop);
            Color toolElementsColor = Storage.GetColor(_mainActivity.Designer.GetIdElementsColor());
            PorterDuffColorFilter buttonFilter = new PorterDuffColorFilter(toolElementsColor, PorterDuff.Mode.SrcAtop);

            _periodLayout.Background.SetColorFilter(textFilter);
            _toolBarLayout.Background.SetColorFilter(toolBarFilter);
            _periodTextView.Background.SetColorFilter(toolBarFilter);
            _periodTextView.SetTextColor(textColor);

            _lastPeriodButton.Background.SetColorFilter(buttonFilter);
            _nextPeriodButton.Background.SetColorFilter(buttonFilter);
        }

        public override void OnHiddenChanged (bool hidden)
        {
            base.OnHiddenChanged(hidden);
            if (hidden)
            {
                UpdateListView();
                _countHidden++;
            }
            if (_countHidden == 1)
            {
                _listView.SetSelection(DateTime.Now.Hour);
            }
        }
    }
}