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

        private View _view;

        public CalendarFragment(Android.App.Activity context)
        {
            _context = context;
            _mainActivity = context as MainActivity;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.fragment_calendar, container, false);

            InitializeCalendarView(_view);

            _calendarLayout = _view.FindViewById<RelativeLayout>(Resource.Id.CalendarLayout);
            _hideLayout = _view.FindViewById<RelativeLayout>(Resource.Id.CalendarHideLayout);
            _hideView = _view.FindViewById<ImageView>(Resource.Id.CalendarHideImageView);
            _tasksLayout = _view.FindViewById<LinearLayout>(Resource.Id.CalendarLinearLayout);
            _addButton = _view.FindViewById<ImageButton>(Resource.Id.CalendarAddTaskButton);

            _calendarView.DateChange += (s, e) => Calendar_DateChange(e.Year, e.Month + 1, e.DayOfMonth);
            _hideLayout.Click += (s, e) => HideButton_Click(_calendarView.Visibility == ViewStates.Visible);
            _addButton.Click += (s, e) => _mainActivity.ShowCreateFragment(Storage.CalendarListTasks);

            UpdateListView();
            PaintViews();

            return _view;
        }

        private void InitializeCalendarView (View view)
        {
            view.FindViewById<CalendarView>(Resource.Id.CalendarMainView).Visibility = ViewStates.Gone;
            view.FindViewById<CalendarView>(Resource.Id.MainDarkCalendarMainView).Visibility = ViewStates.Gone;
            view.FindViewById<CalendarView>(Resource.Id.DeepWaterCalendarMainView).Visibility = ViewStates.Gone;
            view.FindViewById<CalendarView>(Resource.Id.DarkPurpleCalendarMainView).Visibility = ViewStates.Gone;

            int idMainCalendarView;
            switch (_mainActivity.Designer.CurrentTheme)
            {
                case Internal.Resources.Designer.Theme.MainDark:
                    idMainCalendarView = Resource.Id.MainDarkCalendarMainView;
                    break;
                case Internal.Resources.Designer.Theme.DeepWater:
                    idMainCalendarView = Resource.Id.DeepWaterCalendarMainView;
                    break;
                case Internal.Resources.Designer.Theme.DarkPurple:
                    idMainCalendarView = Resource.Id.DarkPurpleCalendarMainView;
                    break;
                default:
                    idMainCalendarView = Resource.Id.CalendarMainView;
                    break;
            }

            _calendarView = view.FindViewById<CalendarView>(idMainCalendarView);
            _calendarView.Visibility = ViewStates.Visible;
        }

        private void HideButton_Click (bool hideCalendar)
        {
            _calendarView.Visibility = hideCalendar ? ViewStates.Gone : ViewStates.Visible;
            int idDrawable = hideCalendar ? Resource.Drawable.ic_show : Resource.Drawable.ic_hide;
            _hideView.Background = _context.GetDrawable(idDrawable);

            Color toolElementsColor = Storage.GetColor(_mainActivity.Designer.GetIdElementsColor());
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
            layoutParams.SetMargins(16, 12, 14, 0);
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
            Color toolElementsColor = Storage.GetColor(_mainActivity.Designer.GetIdElementsColor());
            PorterDuffColorFilter buttonFilter = new PorterDuffColorFilter(toolElementsColor, PorterDuff.Mode.SrcAtop);

            _hideLayout.SetBackgroundColor(toolBarColor);

            _hideView.Background.SetColorFilter(buttonFilter);
            _addButton.Background.SetColorFilter(buttonFilter);
            InitializeCalendarView(_view);
        }
    }
}