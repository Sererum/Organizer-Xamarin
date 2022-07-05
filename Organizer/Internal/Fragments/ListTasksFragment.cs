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
    public class ListTasksFragment : Fragment
    {
        private readonly Android.App.Activity _context;
        private readonly MainActivity _mainActivity;

        private RelativeLayout _toolBarLayout;
        private RelativeLayout _periodLayout;
        private TextView _periodTextView;
        private Spinner _periodSpinner;
        private ImageButton _nextPeriodButton;
        private ImageButton _lastPeriodButton;
        private ImageButton _addButton;
        private LinearLayout _tasksLayout;

        public ListTasksFragment(Android.App.Activity context)
        {
            _context = context;
            _mainActivity = context as MainActivity;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view =  inflater.Inflate(Resource.Layout.fragment_list, container, false);

            _toolBarLayout = view.FindViewById<RelativeLayout>(Resource.Id.ListToolBarLayout);
            _periodLayout = view.FindViewById<RelativeLayout>(Resource.Id.ListPeriodLayout);
            _periodTextView = view.FindViewById<TextView>(Resource.Id.ListPeriodTextView);
            _periodSpinner = view.FindViewById<Spinner>(Resource.Id.ListPeriodSpinner);
            _lastPeriodButton = view.FindViewById<ImageButton>(Resource.Id.ListLastPeriodButton);
            _nextPeriodButton = view.FindViewById<ImageButton>(Resource.Id.ListNextPeriodButton);
            _addButton = view.FindViewById<ImageButton>(Resource.Id.ListAddTaskButton);
            _tasksLayout = view.FindViewById<LinearLayout>(Resource.Id.ListTasksLinearLayout);

            _periodSpinner.ItemSelected += (s, e) => PeriodSpinner_ItemSelected();
            UpdatePeriods(firstCreate: true);

            _lastPeriodButton.Click += (s, e) => PeriodButton_Click(isNext: false);
            _nextPeriodButton.Click += (s, e) => PeriodButton_Click(isNext: true);
            _addButton.Click += (s, e) => _mainActivity.ShowCreateFragment(Storage.MainListTasks);

            UpdateListView();
            PaintViews();

            return view;
        }

        private void SortButton_Click (View view)
        {
            PopupMenu popup = new PopupMenu(_context, view);
            popup.MenuInflater.Inflate(Resource.Menu.type_sort_menu, popup.Menu);
            popup.Show();

            popup.MenuItemClick += (s, e) =>
            {
                switch (e.Item.ItemId)
                {
                    case Resource.Id.time_start_sort:
                        TaskSorter.CurrentType = TaskSorter.Type.TimeStart;
                        break;
                    case Resource.Id.type_sort:
                        TaskSorter.CurrentType = TaskSorter.Type.TypeTask;
                        break;
                }
                _mainActivity.UpdateFragments();
            };
        }

        private void PeriodSpinner_ItemSelected ()
        {
            _nextPeriodButton.Visibility = ViewStates.Visible;
            _lastPeriodButton.Visibility = ViewStates.Visible;
            if ((int) _periodSpinner.SelectedItemId == (int) Server.Period.Global)
            {
                _nextPeriodButton.Visibility = ViewStates.Invisible;
                _lastPeriodButton.Visibility = ViewStates.Invisible;
            }

            Storage.ChangeMainListTasks((Server.Period) _periodSpinner.SelectedItemId, Storage.MainDate);

            UpdateListView();
        }

        public void PeriodButton_Click (bool isNext)
        {
            int addition = isNext ? 1 : -1;
            ViewStates viewAddButton = ViewStates.Visible;
            DateTime nowDate = Storage.MainDate;
            Server.Period period = (Server.Period) _periodSpinner.SelectedItemId;

            nowDate = nowDate.AddDays(period == Server.Period.Day ? addition : 0);
            nowDate = nowDate.AddMonths(period == Server.Period.Month ? addition : 0);
            nowDate = nowDate.AddYears(period == Server.Period.Year ? addition : 0);

            Storage.ChangeMainListTasks(Storage.MainPeriod, nowDate);

            if (Storage.IsPast(nowDate))
            {
                viewAddButton = ViewStates.Invisible;
            }
            _addButton.Visibility = viewAddButton;
            Update();
        }

        public void Update ()
        {
            UpdateListView();
            UpdatePeriods();
        }

        private void UpdateListView ()
        {
            Storage.MainListTasks.Sort();
            _tasksLayout.RemoveAllViews();

            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            layoutParams.SetMargins(16, 12, 14, 0);
            bool isPast = Storage.IsPast(Storage.MainDate);

            foreach (BaseTask task in Storage.MainListTasks)
            {
                TaskViewConstructor constructor = new TaskViewConstructor(_context, isPast);
                _tasksLayout.AddView(constructor.GetTaskView(task), layoutParams);
            }
        }

        private void UpdatePeriods (bool firstCreate = false)
        {
            int selectItem = 3;
            if (firstCreate == false)
            {
                selectItem = _periodSpinner.SelectedItemPosition;
            }
            _periodSpinner.Adapter = new PeriodArrayAdapter(_context, firstCreate: firstCreate);
            _periodSpinner.SetPopupBackgroundDrawable(null);
            _periodSpinner.SetSelection(selectItem);
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
            _periodTextView.Background.SetColorFilter(toolBarFilter);
            _periodSpinner.Background.SetColorFilter(textFilter);
            _toolBarLayout.Background.SetColorFilter(toolBarFilter);

            _lastPeriodButton.Background.SetColorFilter(buttonFilter);
            _nextPeriodButton.Background.SetColorFilter(buttonFilter);
            _addButton.Background.SetColorFilter(buttonFilter);
        }
    }
}