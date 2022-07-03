using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Organizer.Internal.Activity;
using Organizer.Internal.ArrayAdapters;
using Organizer.Internal.Data;
using Organizer.Internal.Model.Task;
using System;
using static Android.App.ActionBar;

namespace Organizer.Internal.Fragments
{
    public class CalendarFragment : Fragment
    {
        private readonly Android.App.Activity _context;
        private readonly MainActivity _mainActivity;

        private RelativeLayout _calendarLayout;
        private CalendarView _calendarView;
        private RelativeLayout _hideLayout;
        private ImageView _hideView;
        private LinearLayout _tasksLayout;
        private ImageButton _addButton;

        public CalendarFragment(Android.App.Activity context)
        {
            _context = context;
            _mainActivity = context as MainActivity;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_calendar, container, false);

            #region Initialize CalendarViews

            view.FindViewById<CalendarView>(Resource.Id.NeonCalendarMainView).Visibility = ViewStates.Gone;
            view.FindViewById<CalendarView>(Resource.Id.CalendarMainView).Visibility = ViewStates.Gone;

            int idMainCalendarView = Resource.Id.CalendarMainView;
            switch (_mainActivity.Designer.CurrentTheme)
            {
                case Internal.Resources.Designer.Theme.GreenNeon:
                    idMainCalendarView = Resource.Id.NeonCalendarMainView;
                    break;
            }
            _calendarView = view.FindViewById<CalendarView>(idMainCalendarView);
            _calendarView.Visibility = ViewStates.Visible;

            #endregion

            _calendarLayout = view.FindViewById<RelativeLayout>(Resource.Id.CalendarLayout);
            _hideLayout = view.FindViewById<RelativeLayout>(Resource.Id.CalendarHideLayout);
            _hideView = view.FindViewById<ImageView>(Resource.Id.CalendarHideImageView);
            _tasksLayout = view.FindViewById<LinearLayout>(Resource.Id.CalendarLinearLayout);
            _addButton = view.FindViewById<ImageButton>(Resource.Id.CalendarAddTaskButton);

            _calendarView.DateChange += (s, e) => Calendar_DateChange(e.Year, e.Month + 1, e.DayOfMonth);
            _hideLayout.Click += (s, e) => HideButton_Click(_calendarView.Visibility == ViewStates.Visible);
            _addButton.Click += (s, e) => _mainActivity.ShowCreateFragment(Storage.CalendarListTasks);

            UpdateListView();
            PaintViews();

            return view;
        }

        private void HideButton_Click (bool hideCalendar)
        {
            _calendarView.Visibility = hideCalendar ? ViewStates.Gone : ViewStates.Visible;
            int idDrawable = hideCalendar ? Resource.Drawable.ic_show : Resource.Drawable.ic_hide;
            _hideView.Background = _context.GetDrawable(idDrawable);

            Color toolElementsColor = Storage.GetColor(_mainActivity.Designer.GetIdToolBarElementsColor());
            PorterDuffColorFilter buttonFilter = new PorterDuffColorFilter(toolElementsColor, PorterDuff.Mode.SrcAtop);
            _hideView.Background.SetColorFilter(buttonFilter);
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
            _tasksLayout.RemoveAllViews();

            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            layoutParams.SetMargins(4, 4, 2, 4);
            bool isPast = Storage.IsPast(Storage.CalendarDate);

            foreach (BaseTask task in Storage.CalendarListTasks)
            {
                TaskViewConstructor constuctor = new TaskViewConstructor(_context, isPast);
                _tasksLayout.AddView(constuctor.GetTaskView(task), layoutParams);
            }
        }

        public void PaintViews ()
        {
            Color toolBarColor = Storage.GetColor(_mainActivity.Designer.GetIdToolBarColor());
            Color toolElementsColor = Storage.GetColor(_mainActivity.Designer.GetIdToolBarElementsColor());
            PorterDuffColorFilter buttonFilter = new PorterDuffColorFilter(toolElementsColor, PorterDuff.Mode.SrcAtop);

            _hideLayout.SetBackgroundColor(toolBarColor);

            _hideView.Background.SetColorFilter(buttonFilter);
            _addButton.Background.SetColorFilter(buttonFilter);
        }
    }
}