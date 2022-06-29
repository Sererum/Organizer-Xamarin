using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Organizer.Internal.Activity;
using Organizer.Internal.Data;
using Organizer.Internal.Model.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Organizer.Internal.ArrayAdapters
{
    public static class TaskViewConstructor
    {
        private static Android.App.Activity _context;
        private static MainActivity _mainActivity;

        public static void InitialConstructor(Android.App.Activity context)
        {
            _context = context;
            _mainActivity = context as MainActivity;
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
        
        public static View GetTaskView(BaseTask task, View convertView)
        {
            #region Initialize views
            ViewHolder holder;
            View view = convertView;

            if (view is null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.list_item_task, null);
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

            holder.CompleteCheckBox.Checked = task.Complete;
            holder.CompleteCheckBox.CheckedChange += (s, e) => CompleteCheckBox_CheckedChange(holder);
            ChangeTextStyle(holder);

            InitializePriorityView(holder, task);

            holder.TitleTextView.Text = task.Text;

            if (task.StartTime != "" || task.EndTime != "")
            {
                holder.FirstLineTextView.Visibility = ViewStates.Visible;
                holder.TimeTextView.Visibility = ViewStates.Visible;
                holder.TimeTextView.Text = task.StartTime + " ~ " + task.EndTime;
            }
            if (task.Text != "")
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
                foreach (BaseTask taskProject in project.Tasks)
                {
                    holder.TasksListLayout.AddView(GetTaskView(taskProject, null));
                }
            }

            return view;
        }

        private static void CompleteCheckBox_CheckedChange (ViewHolder holder)
        {
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
    }
}