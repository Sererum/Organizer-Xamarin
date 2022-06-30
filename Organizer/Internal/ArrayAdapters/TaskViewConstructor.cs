using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Java.Util;
using Organizer.Internal.Activity;
using Organizer.Internal.Data;
using Organizer.Internal.Model;
using Organizer.Internal.Model.Task;
using System;
using LayoutParams = Android.App.ActionBar.LayoutParams;

namespace Organizer.Internal.ArrayAdapters
{
    public static class TaskViewConstructor
    {
        private static Android.App.Activity _context;
        private static MainActivity _mainActivity;
        private static long _periodClick;

        public static void InitialConstructor(Android.App.Activity context)
        {
            _context = context;
            _mainActivity = context as MainActivity;
            _periodClick = (new Date()).Time;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public CheckBox CompleteCheckBox;
            public TextView PriorityTextView;
            public TextView TitleTextView;
            public TextView FirstLineTextView;
            public TextView TimeTextView;
            public TextView SecondLineTextView;
            public TextView TextTextView;
            public TextView ThirdLineTextView;
            public LinearLayout TasksListLayout;
            public ImageButton HideTasksButton;
        }
        
        public static View GetTaskView(BaseTask task, View convertView, bool isSimple = false)
        {
            #region Initialize views
            ViewHolder holder;
            View view = convertView;

            if (view is null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_task, null);
                holder = new ViewHolder();

                holder.CompleteCheckBox = view.FindViewById<CheckBox>(Resource.Id.TaskCompleteCheckBox);
                holder.PriorityTextView = view.FindViewById<TextView>(Resource.Id.TaskPriorityTextView);
                holder.TitleTextView = view.FindViewById<TextView>(Resource.Id.TaskTitleTextView);
                holder.FirstLineTextView = view.FindViewById<TextView>(Resource.Id.TaskFirstLineTextView);
                holder.TimeTextView = view.FindViewById<TextView>(Resource.Id.TaskTimeTextView);
                holder.SecondLineTextView = view.FindViewById<TextView>(Resource.Id.TaskSecondLineTextView);
                holder.TextTextView = view.FindViewById<TextView>(Resource.Id.TaskTextTextView);
                holder.ThirdLineTextView = view.FindViewById<TextView>(Resource.Id.TaskThirdLineTextView);
                holder.TasksListLayout = view.FindViewById<LinearLayout>(Resource.Id.TasksListLayout);
                holder.HideTasksButton = view.FindViewById<ImageButton>(Resource.Id.TaskHideProjectButton);

                view.SetTag(Resource.String.key_task_holder, holder);
            }
            else
            {
                holder = (ViewHolder) view.GetTag(Resource.String.key_task_holder);
            }
            #endregion

            if (isSimple)
            {
                holder.CompleteCheckBox.Visibility = ViewStates.Gone;
            }
            holder.CompleteCheckBox.Checked = task.Complete;
            holder.CompleteCheckBox.CheckedChange += (s, e) => CompleteCheckBox_CheckedChange(holder, task);
            ChangeTextStyle(holder);

            InitializePriorityView(holder, task);

            holder.TitleTextView.Text = task.Title;

            if (isSimple == false && (task.StartTime != "" || task.EndTime != ""))
            {
                holder.FirstLineTextView.Visibility = ViewStates.Visible;
                holder.TimeTextView.Visibility = ViewStates.Visible;
                holder.TimeTextView.Text = task.StartTime + " ~ " + task.EndTime;
            }
            if (isSimple == false && (task.Text != ""))
            {
                holder.SecondLineTextView.Visibility = ViewStates.Visible;
                holder.TextTextView.Visibility = ViewStates.Visible;
                holder.TextTextView.Text = task.Text;
            }
            if (task is Project)
            {
                Project project = task as Project;

                holder.HideTasksButton.Visibility = ViewStates.Visible;
                holder.HideTasksButton.Click += (s, e) =>
                {
                    bool listIsVisible = (holder.TasksListLayout.Visibility == ViewStates.Visible);
                    ViewStates viewState = listIsVisible ? ViewStates.Gone : ViewStates.Visible;
                    holder.ThirdLineTextView.Visibility = viewState;
                    holder.TasksListLayout.Visibility = viewState;
                };

                holder.ThirdLineTextView.Visibility = project.TasksVisible ? ViewStates.Visible : ViewStates.Gone;
                holder.TasksListLayout.Visibility = project.TasksVisible ? ViewStates.Visible : ViewStates.Gone;
                holder.TasksListLayout.RemoveAllViews();
                LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
                layoutParams.SetMargins(0, 2, 0, 2);
                project.Tasks.Sort();
                foreach (BaseTask taskProject in project.Tasks)
                {
                    holder.TasksListLayout.AddView(GetTaskView(taskProject, null), layoutParams);
                }
            }

            view.Click += (s, e) => View_LongClick(view, task);

            return view;
        }

        private static void CompleteCheckBox_CheckedChange (ViewHolder holder, BaseTask task)
        {
            task.Complete = holder.CompleteCheckBox.Checked;
            ChangeTextStyle(holder);
            _mainActivity.UpdateFragments();
        }

        private static void ChangeTextStyle (ViewHolder holder)
        {
            PaintFlags paintFlag = holder.CompleteCheckBox.Checked ? PaintFlags.StrikeThruText : PaintFlags.LinearText;
            holder.TitleTextView.PaintFlags = paintFlag;
            holder.TimeTextView.PaintFlags = paintFlag;
            holder.TextTextView.PaintFlags = paintFlag;
        }

        private static void InitializePriorityView (ViewHolder holder, BaseTask task)
        {
            string text = "";
            Color color = new Color();
            switch (task.TypeTask)
            {
                case (int) BaseTask.Type.Project:
                    text = _context.Resources.GetString(Resource.String.project_task_short);
                    color = new Color(ContextCompat.GetColor(_context, Resource.Color.project));
                    break;
                case (int) BaseTask.Type.Regular:
                    int priorityTask = (task as Regular).Priority;
                    text = priorityTask.ToString();
                    color = new Color(ContextCompat.GetColor(_context, Storage.PriorityToColorId[priorityTask]));
                    break;
                case (int) BaseTask.Type.Routine:
                    text = _context.Resources.GetString(Resource.String.routine_task_short);
                    color = new Color(ContextCompat.GetColor(_context, Resource.Color.routine));
                    break;
            }
            holder.PriorityTextView.Text = text;
            holder.PriorityTextView.SetTextColor(color);
            color.A = 25;
            holder.PriorityTextView.SetBackgroundColor(color);
        }

        private static void View_LongClick (View view, BaseTask task)
        {
            if ((new Date()).Time - _periodClick < 1000)
            {
                return;
            }
            _periodClick = (new Date()).Time;

            int idMenu = Resource.Menu.task_action_menu_day;
            ListTasks currentList = new ListTasks();
            bool disableRoutine = task is Project;

            if (Storage.MainListTasks.Contains(task))
            {
                currentList = Storage.MainListTasks.GetRootList(task) ?? throw new ArgumentException();
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
            else if (Storage.CalendarListTasks.Contains(task))
            {
                currentList = Storage.CalendarListTasks.GetRootList(task) ?? throw new ArgumentException();
                if (Storage.IsPast(Storage.CalendarDate))
                {
                    idMenu = Resource.Menu.task_action_menu_past;
                }
            }
            else if(Storage.ScheduleListTasks.Contains(task))
            {
                currentList = Storage.ScheduleListTasks.GetRootList(task) ?? throw new ArgumentException();
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
                        _mainActivity.UpdateFragments();
                        break;
                    case Resource.Id.action_move_next:
                        break;
                    case Resource.Id.action_move_lower:
                        break;
                }
            };
        }
    }
}