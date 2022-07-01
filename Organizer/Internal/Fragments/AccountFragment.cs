using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Organizer.Internal.Data;
using Organizer.Internal.Model;

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
            _mainLayout.AddView(GetChangeLanguageView());
        }

        private View GetCounterView ()
        {
            View view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_account_counter_tasks, null);

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

        private View GetChangeLanguageView ()
        {
            View view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_account_change_language, null);

            RelativeLayout mainLayout = view.FindViewById<RelativeLayout>(Resource.Id.ChangeLanguageLayout);
            TextView selectTextView = view.FindViewById<TextView>(Resource.Id.ChangeLanguageSelectTextView);

            mainLayout.Click += (s, e) =>
            {
                PopupMenu popup = new PopupMenu(_context, view);
                popup.MenuInflater.Inflate(Resource.Menu.change_layout_menu, popup.Menu);
                popup.Show();

                popup.MenuItemClick += (s, e) =>
                {
                    switch (e.Item.ItemId)
                    {
                        case Resource.Id.language_english:
                            selectTextView.Text = _context.Resources.GetString(Resource.String.english);

                            break;
                        case Resource.Id.language_russian:
                            selectTextView.Text = _context.Resources.GetString(Resource.String.russian);

                            break;
                    }
                };
            };

            return view;
        }

        public override void OnHiddenChanged (bool hidden)
        {
            base.OnHiddenChanged(hidden);
            UpdateListView();
        }
    }
}