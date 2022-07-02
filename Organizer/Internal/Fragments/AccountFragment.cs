using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Organizer.Internal.Activity;
using Organizer.Internal.Data;
using Organizer.Internal.Model;
using Organizer.Internal.Resources;

namespace Organizer.Internal.Fragments
{
    public class AccountFragment : Fragment
    {
        private readonly Android.App.Activity _context;
        private readonly MainActivity _mainActivity;
        private LinearLayout _mainLayout;

        public AccountFragment(Android.App.Activity context)
        {
            _context = context;
            _mainActivity = context as MainActivity;
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

            view.FindViewById<TextView>(Resource.Id.CounterTaskTextView).Text
                = _mainActivity.Translater.GetString(Resource.String.complete_tasks);

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

            view.FindViewById<TextView>(Resource.Id.ChangeLanguageTextView).Text
                = _mainActivity.Translater.GetString(Resource.String.current_language);

            RelativeLayout mainLayout = view.FindViewById<RelativeLayout>(Resource.Id.ChangeLanguageLayout);
            TextView selectTextView = view.FindViewById<TextView>(Resource.Id.ChangeLanguageSelectTextView);

            switch (_mainActivity.Translater.CurrentLanguage)
            {
                case Translater.Language.English:
                    selectTextView.Text = _mainActivity.Translater.GetString(Resource.String.english);
                    break;
                case Translater.Language.Russian:
                    selectTextView.Text = _mainActivity.Translater.GetString(Resource.String.russian);
                    break;
            }

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
                            selectTextView.Text = _mainActivity.Translater.GetString(Resource.String.english);
                            _mainActivity.Translater.CurrentLanguage = Translater.Language.English;
                            break;
                        case Resource.Id.language_russian:
                            selectTextView.Text = _mainActivity.Translater.GetString(Resource.String.russian);
                            _mainActivity.Translater.CurrentLanguage = Translater.Language.Russian;
                            break;
                    }
                    _mainActivity.UpdateFragments();
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