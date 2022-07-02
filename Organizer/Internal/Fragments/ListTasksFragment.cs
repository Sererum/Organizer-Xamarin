using Android.Content;
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

namespace Organizer.Internal.Fragments
{
    public class ListTasksFragment : Fragment
    {
        private readonly Android.App.Activity _context;
        private readonly MainActivity _mainActivity;

        private Spinner _periodSpinner;
        private ImageButton _nextPeriodButton;
        private ImageButton _lastPeriodButton;
        private ImageButton _sortButton;
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

            RelativeLayout toolBarLayout = view.FindViewById<RelativeLayout>(Resource.Id.ListToolBarLayout);
            TextView periodTextView = view.FindViewById<TextView>(Resource.Id.ListPeriodTextView);
            _periodSpinner = view.FindViewById<Spinner>(Resource.Id.ListPeriodSpinner);
            _lastPeriodButton = view.FindViewById<ImageButton>(Resource.Id.ListLastPeriodButton);
            _nextPeriodButton = view.FindViewById<ImageButton>(Resource.Id.ListNextPeriodButton);
            _sortButton = view.FindViewById<ImageButton>(Resource.Id.ListSortButton);
            _addButton = view.FindViewById<ImageButton>(Resource.Id.ListAddTaskButton);
            _tasksLayout = view.FindViewById<LinearLayout>(Resource.Id.ListTasksLinearLayout);

            _periodSpinner.ItemSelected += (s, e)
                => PeriodSpinner_ItemSelected(view.FindViewById<TextView>(Resource.Id.ListPeriodTextView));
            UpdatePeriods();

            _lastPeriodButton.Click += (s, e) => PeriodButton_Click(isNext: false);
            _nextPeriodButton.Click += (s, e) => PeriodButton_Click(isNext: true);

            _sortButton.Click += (s, e) => SortButton_Click(_sortButton);

            _addButton.Click += (s, e) => _mainActivity.ShowCreateFragment(Storage.MainListTasks);
            UpdateListView();

            #region Paint views

            Color toolBarColor = Storage.GetColor(_mainActivity.Designer.GetIdToolBarColor());
            Color toolElementsColor = Storage.GetColor(_mainActivity.Designer.GetIdToolBarElementsColor());

            toolBarLayout.SetBackgroundColor(toolBarColor);
            periodTextView.SetBackgroundColor(toolBarColor);

            PorterDuffColorFilter colorFilter = new PorterDuffColorFilter(toolElementsColor, PorterDuff.Mode.SrcAtop);
            _lastPeriodButton.Background.SetColorFilter(colorFilter);
            _nextPeriodButton.Background.SetColorFilter(colorFilter);
            _sortButton.Background.SetColorFilter(colorFilter);
            _addButton.Background.SetColorFilter(colorFilter);

            #endregion

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

        private void PeriodSpinner_ItemSelected (TextView textView)
        {
            textView.Text = _periodSpinner.SelectedItem.ToString();

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
            UpdatePeriods((int) _periodSpinner.SelectedItemId);
            UpdateListView();
        }

        public void Update ()
        {
            UpdateListView();
            UpdatePeriods();
        }

        public void UpdateListView ()
        {
            Storage.MainListTasks.Sort();
            _tasksLayout.RemoveAllViews();

            foreach (BaseTask task in Storage.MainListTasks)
            {
                TaskViewConstructor constructor = new TaskViewConstructor(_context);
                _tasksLayout.AddView(constructor.GetTaskView(task));
            }
        }

        private void UpdatePeriods (int selectedItem = 3)
        {
            _periodSpinner.Adapter = new PeriodArrayAdapter(_context);
            _periodSpinner.SetSelection(selectedItem);
        }
    }
}