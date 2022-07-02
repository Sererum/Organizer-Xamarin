using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Java.Util;
using Organizer.Internal.Activity;
using Organizer.Internal.Data;
using Organizer.Internal.Fragments;
using Organizer.Internal.Model;
using Organizer.Internal.Model.Task;
using System;
using LayoutParams = Android.App.ActionBar.LayoutParams;

namespace Organizer.Internal.ArrayAdapters
{
    public class TaskViewConstructor
    {
        private readonly Android.App.Activity _context;
        private readonly MainActivity _mainActivity;
        private long _periodClick;

        private CheckBox _completeCheckBox;
        private TextView _priorityTextView;
        private ImageView _typeImageView;
        private TextView _titleTextView;
        private TextView _firstLineTextView;
        private TextView _timeTextView;
        private TextView _secondLineTextView;
        private TextView _textTextView;
        private TextView _thirdLineTextView;
        private LinearLayout _tasksListLayout;
        private ImageButton _hideTasksButton;

        public TaskViewConstructor (Android.App.Activity context)
        {
            _context = context;
            _mainActivity = context as MainActivity;
            _periodClick = (new Date()).Time;
        }

        public View GetTaskView (BaseTask task, bool isSimple = false)
        {
            #region Initialize views

            View view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_task, null);

            RelativeLayout mainLayout = view.FindViewById<RelativeLayout>(Resource.Id.TaskMainLayout);
            _completeCheckBox = view.FindViewById<CheckBox>(Resource.Id.TaskCompleteCheckBox);
            _priorityTextView = view.FindViewById<TextView>(Resource.Id.TaskPriorityTextView);
            _typeImageView = view.FindViewById<ImageView>(Resource.Id.TaskTypeImageView);
            _titleTextView = view.FindViewById<TextView>(Resource.Id.TaskTitleTextView);
            _firstLineTextView = view.FindViewById<TextView>(Resource.Id.TaskFirstLineTextView);
            _timeTextView = view.FindViewById<TextView>(Resource.Id.TaskTimeTextView);
            _secondLineTextView = view.FindViewById<TextView>(Resource.Id.TaskSecondLineTextView);
            _textTextView = view.FindViewById<TextView>(Resource.Id.TaskTextTextView);
            _thirdLineTextView = view.FindViewById<TextView>(Resource.Id.TaskThirdLineTextView);
            _tasksListLayout = view.FindViewById<LinearLayout>(Resource.Id.TasksListLayout);
            _hideTasksButton = view.FindViewById<ImageButton>(Resource.Id.TaskHideProjectButton);

            #endregion

            mainLayout.SetBackgroundColor(Storage.GetColor(_mainActivity.Designer.GetIdMainColor()));

            if (isSimple)
            {
                _completeCheckBox.Visibility = ViewStates.Gone;
            }
            _completeCheckBox.Checked = task.Complete;
            _completeCheckBox.CheckedChange += (s, e) => CompleteCheckBox_CheckedChange(task);
            ChangeTextStyle();

            InitializeTypeTaskView(task);

            _titleTextView.Text = task.Title;

            if (isSimple == false && (task.StartTime != "" || task.EndTime != ""))
            {
                _firstLineTextView.Visibility = ViewStates.Visible;
                _timeTextView.Visibility = ViewStates.Visible;
                _timeTextView.Text = task.StartTime + " ~ " + task.EndTime;
            }
            if (isSimple == false && (task.Text != ""))
            {
                _secondLineTextView.Visibility = ViewStates.Visible;
                _textTextView.Visibility = ViewStates.Visible;
                _textTextView.Text = task.Text;
            }
            if (task is Project)
            {
                Project project = task as Project;

                _hideTasksButton.Visibility = ViewStates.Visible;
                _hideTasksButton.Click += (s, e) =>
                {
                    bool listIsVisible = (_tasksListLayout.Visibility == ViewStates.Visible);
                    ViewStates viewState = listIsVisible ? ViewStates.Gone : ViewStates.Visible;
                    _thirdLineTextView.Visibility = viewState;
                    _tasksListLayout.Visibility = viewState;
                };

                _thirdLineTextView.Visibility = project.TasksVisible ? ViewStates.Visible : ViewStates.Gone;
                _tasksListLayout.Visibility = project.TasksVisible ? ViewStates.Visible : ViewStates.Gone;
                _tasksListLayout.RemoveAllViews();
                LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
                layoutParams.SetMargins(0, 2, 0, 2);
                project.Tasks.Sort();
                foreach (BaseTask taskProject in project.Tasks)
                {
                    TaskViewConstructor constructor = new TaskViewConstructor(_context);
                    _tasksListLayout.AddView(constructor.GetTaskView(taskProject), layoutParams);
                }
            }

            view.LongClick += (s, e) => View_LongClick(view, task);

            return view;
        }

        private void CompleteCheckBox_CheckedChange (BaseTask task)
        {
            task.Complete = (task.Complete == false);
            ChangeTextStyle();
            _mainActivity.UpdateFragments();
        }

        private void ChangeTextStyle ()
        {
            PaintFlags paintFlag = _completeCheckBox.Checked ? PaintFlags.StrikeThruText : PaintFlags.LinearText;
            _titleTextView.PaintFlags = paintFlag;
            _timeTextView.PaintFlags = paintFlag;
            _textTextView.PaintFlags = paintFlag;
        }

        private void InitializeTypeTaskView (BaseTask task)
        {
            PorterDuffColorFilter colorFilter;
            Color color;
            switch (task.TypeTask)
            {
                case (int) BaseTask.Type.Project:
                    _typeImageView.Visibility = ViewStates.Visible;
                    _typeImageView.Background = _context.GetDrawable(Resource.Drawable.ic_project);

                    color = Storage.GetColor(_mainActivity.Designer.GetIdToolBarElementsColor());
                    colorFilter = new PorterDuffColorFilter(color, PorterDuff.Mode.SrcAtop);
                    _typeImageView.Background.SetColorFilter(colorFilter);
                    break;
                case (int) BaseTask.Type.Regular:
                    int priorityTask = (task as Regular).Priority;

                    _priorityTextView.Visibility = ViewStates.Visible;
                    _priorityTextView.Text = priorityTask.ToString();

                    color = new Color(ContextCompat.GetColor(_context, Storage.PriorityToColorId[priorityTask]));
                    _priorityTextView.SetTextColor(color);
                    color.A = 75;
                    _priorityTextView.SetBackgroundColor(color);
                    break;
                case (int) BaseTask.Type.Routine:
                    _typeImageView.Visibility = ViewStates.Visible;
                    _typeImageView.Background = _context.GetDrawable(Resource.Drawable.ic_routine);

                    color = Storage.GetColor(_mainActivity.Designer.GetIdToolBarElementsColor());
                    colorFilter = new PorterDuffColorFilter(color, PorterDuff.Mode.SrcAtop);
                    _typeImageView.Background.SetColorFilter(colorFilter);
                    break;
            }
        }

        private void View_LongClick (View view, BaseTask task)
        {
            if ((new Date()).Time - _periodClick < 500)
            {
                return;
            }
            _periodClick = (new Date()).Time;

            int idMenu = task is Routine ? Resource.Menu.task_action_menu_routine : Resource.Menu.task_action_menu_day;
            ListTasks currentList = new ListTasks();
            bool disableRoutine = task is Project;

            Server.Period currentPeriod = Storage.MainPeriod;
            DateTime currentDate = DateTime.MinValue;
            DateTime nextDate = DateTime.MinValue;
            ListTasks nextList = null;

            if (_mainActivity.CurrentFragment is ListTasksFragment)
            {
                currentList = Storage.MainListTasks.GetRootList(task) ?? throw new ArgumentException();

                currentDate = Storage.MainDate;
                nextDate = Storage.MainDate;
                nextDate = nextDate.AddDays(Storage.MainPeriod == Server.Period.Day ? 1 : 0);
                nextDate = nextDate.AddMonths(Storage.MainPeriod == Server.Period.Month ? 1 : 0);
                nextDate = nextDate.AddYears(Storage.MainPeriod == Server.Period.Year ? 1 : 0);
                if (Storage.MainPeriod == Server.Period.Month || Storage.MainPeriod == Server.Period.Year)
                {
                    idMenu = Resource.Menu.task_action_menu;
                    disableRoutine = true;
                }
                if (Storage.MainPeriod == Server.Period.Global)
                {
                    idMenu = Resource.Menu.task_action_menu_global;
                    disableRoutine = true;
                }
                if (Storage.IsPast(Storage.MainDate))
                {
                    idMenu = Resource.Menu.task_action_menu_past;
                }
            }
            else if (_mainActivity.CurrentFragment is CalendarFragment)
            {
                currentList = Storage.CalendarListTasks.GetRootList(task) ?? throw new ArgumentException();

                currentPeriod = Server.Period.Day;
                currentDate = Storage.CalendarDate;
                nextDate = Storage.CalendarDate.AddDays(1);
                if (Storage.IsPast(Storage.CalendarDate))
                {
                    idMenu = Resource.Menu.task_action_menu_past;
                }
            }
            else if (_mainActivity.CurrentFragment is ScheduleFragment)
            {
                currentList = Storage.ScheduleListTasks.GetRootList(task) ?? throw new ArgumentException();

                currentPeriod = Server.Period.Day;
                currentDate = Storage.ScheduleDate;
                nextDate = Storage.ScheduleDate.AddDays(1);
                if (Storage.IsPast(Storage.ScheduleDate))
                {
                    idMenu = Resource.Menu.task_action_menu_past;
                }
            }
            else
            {
                return;
            }

            PopupMenu popup = new PopupMenu(_context, view);
            popup.MenuInflater.Inflate(idMenu, popup.Menu);
            popup.Show();

            popup.MenuItemClick += (s, e) =>
            {
                switch (e.Item.ItemId)
                {
                    case Resource.Id.action_edit:
                        _mainActivity.ShowCreateFragment(currentList, task, disableRoutine);
                        break;
                    case Resource.Id.action_delete:
                        currentList.Remove(task);
                        if (task is Routine)
                        {
                            ListTasks routines = Server.Routines;
                            routines.Remove(task);
                            Server.Routines = routines;
                        }
                        _mainActivity.UpdateFragments();
                        break;
                    case Resource.Id.action_move_next:
                        currentList.Remove(task);

                        nextList = Server.GetList(currentPeriod, nextDate);
                        nextList.Add(task);
                        Server.SetList(currentPeriod, nextDate, nextList);

                        _mainActivity.UpdateFragments();
                        break;
                    case Resource.Id.action_move_lower:
                        currentList.Remove(task);

                        Server.Period nextPeriod = (Server.Period) ((int) currentPeriod + 1);
                        nextList = Server.GetList(nextPeriod, currentDate);
                        nextList.Add(task);
                        Server.SetList(nextPeriod, currentDate, nextList);

                        _mainActivity.UpdateFragments();
                        break;
                }
            };
        }
    }
}