using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using Organizer.Internal.Data;
using Organizer.Internal.Fragments;
using Organizer.Internal.Model;
using Organizer.Internal.Model.Task;
using System.Collections.Generic;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Organizer.Internal.Activity
{
    [Activity(Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ListTasksFragment _listTasksFragment;
        private CalendarFragment _calendarFragment;
        private ScheduleFragment _scheduleFragment;
        private AccountFragment _accountFragment;
        private CreateFragment _createFragment;

        private Fragment _currentFragment;
        private Stack<Fragment> _lastFragments;

        public Fragment CurrentFragment => _currentFragment;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Storage.InitializeListsTasks(this);

            InitializeFragments(savedInstanceState);
            InitializeButtons();
        }

        #region Initialize fragments
        private void InitializeFragments (Bundle savedInstanceState)
        {
            _lastFragments = new Stack<Fragment>();

            _listTasksFragment = new ListTasksFragment(this);
            _calendarFragment = new CalendarFragment(this);
            _scheduleFragment = new ScheduleFragment(this);
            _accountFragment = new AccountFragment(this);

            Fragment[] fragments = { _listTasksFragment, _calendarFragment, _scheduleFragment, _accountFragment };
            _currentFragment = _listTasksFragment;
            var fragmentTransaction = SupportFragmentManager.BeginTransaction();
            if (savedInstanceState is null)
            {
                foreach (Fragment fragment in fragments)
                {
                    fragmentTransaction.Add(Resource.Id.MainFragmentLayout, fragment).Hide(fragment);
                }
                fragmentTransaction.Commit();
            }
            ShowFragment(_currentFragment);
        }

        public void ShowFragment (Fragment fragment, bool returnBack = false)
        {
            if (fragment.IsVisible == true)
            {
                return;
            }
            var fragmentTransaction = SupportFragmentManager.BeginTransaction();

            fragmentTransaction.Hide(_currentFragment);
            fragmentTransaction.Show(fragment);
            if (returnBack == false)
            {
                _lastFragments.Push(_currentFragment);
            }
            _currentFragment = fragment;

            fragmentTransaction.AddToBackStack(null);
            fragmentTransaction.Commit();
        }

        public void InitializeButtons ()
        {
            FindViewById<ImageButton>(Resource.Id.MainCalendarButton).Click += (s, e) => ShowFragment(_calendarFragment);
            FindViewById<ImageButton>(Resource.Id.MainScheduleButton).Click += (s, e) => ShowFragment(_scheduleFragment);
            FindViewById<ImageButton>(Resource.Id.MainListButton).Click += (s, e) => ShowFragment(_listTasksFragment);
            FindViewById<ImageButton>(Resource.Id.MainAccountButton).Click += (s, e) => ShowFragment(_accountFragment);
        }
        #endregion

        public void ShowCreateFragment (ListTasks list, BaseTask editTask = null, bool disableRoutine = false)
        {
            _createFragment = new CreateFragment(this, list, disableRoutine, editTask);
            var fragmentTransaction = SupportFragmentManager.BeginTransaction();
            fragmentTransaction.Add(Resource.Id.MainFragmentLayout, _createFragment).Hide(_createFragment);
            fragmentTransaction.Commit();
            ShowFragment(_createFragment);
        }

        public void UpdateFragments ()
        {
            Storage.SynchronizeLists(_currentFragment);
            _listTasksFragment.UpdateListView();
            _calendarFragment.UpdateListView();
            _scheduleFragment.UpdateListView();
        }

        public override void OnBackPressed ()
        {
            if (_lastFragments.Count > 0)
            {
                ShowFragment(_lastFragments.Pop(), true);
            }
        }

        protected override void OnPause ()
        {
            base.OnPause();
            Storage.SaveListsTasks();
        }
    }
}