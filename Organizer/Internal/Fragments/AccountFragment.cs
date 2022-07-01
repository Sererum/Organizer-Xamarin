using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Organizer.Internal.Data;
using Organizer.Internal.Model;
using System;

namespace Organizer.Internal.Fragments
{
    public class AccountFragment : Fragment
    {
        private readonly Android.App.Activity _context;

        private LinearLayout _mainLayout;

        public AccountFragment(Android.App.Activity context)
        {
            _context = context;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_account, container, false);
            _mainLayout = view.FindViewById<LinearLayout>(Resource.Id.AccountMainLayout);
            UpdateListView();
            return view;
        }

        public void UpdateListView ()
        {
            _mainLayout.RemoveAllViews();

            _mainLayout.AddView(GetCounterView());
        }

        private View GetCounterView ()
        {
            View view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_counter_complete_task, null);

            ListTasks todayList = Storage.MainListTasks;
            int countAllTasks = todayList.GetCountTasks(ListTasks.TaskCounter.WithoutProject);
            int countCompleteTasks = todayList.GetCountTasks(ListTasks.TaskCounter.Complete_WithoutProject);

            TextView counterTextView = view.FindViewById<TextView>(Resource.Id.CounterTaskCounterTextView);
            counterTextView.Text = countCompleteTasks + " / " + countAllTasks;

            SeekBar counterSeekBar = view.FindViewById<SeekBar>(Resource.Id.CounterTaskSeekBar);
            counterSeekBar.Max = countAllTasks;
            counterSeekBar.Progress = countCompleteTasks;

            return view;
        }

        public override void OnHiddenChanged (bool hidden)
        {
            base.OnHiddenChanged(hidden);
            UpdateListView();
        }
    }
}