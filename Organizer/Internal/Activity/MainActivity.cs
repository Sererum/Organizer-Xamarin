using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Organizer.Internal.Data;
using Organizer.Internal.Fragments;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Organizer.Internal.Activity
{
    [Activity(Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ListTasksFragment _listTasksFragment;
        private CalendarFragment _calendarFragment;
        private ScheduleFragment _scheduleFragment;
        private TimerFragment _timerFragment;
        private AccountFragment _accountFragment;
        private Fragment _lastFragment;
        private Fragment _currentFragment;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Server.SetListTasks();

            InitializeFragments(savedInstanceState);
            InitializeButtons();
        }

        #region Work with fragments
        private void InitializeFragments (Bundle savedInstanceState)
        {
            _listTasksFragment = new ListTasksFragment(this);
            _calendarFragment = new CalendarFragment();
            _scheduleFragment = new ScheduleFragment();
            _timerFragment = new TimerFragment();
            _accountFragment = new AccountFragment();

            Fragment[] fragments = { _listTasksFragment, _calendarFragment, _scheduleFragment, _timerFragment, _accountFragment };

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

        public void ShowFragment (Fragment fragment)
        {
            if (fragment.IsVisible == true)
            {
                return;
            }
            var fragmentTransaction = SupportFragmentManager.BeginTransaction();

            fragmentTransaction.Hide(_currentFragment);
            fragmentTransaction.Show(fragment);
            _lastFragment = _currentFragment;
            _currentFragment = fragment;

            fragmentTransaction.AddToBackStack(null);
            fragmentTransaction.Commit();
        }

        public void InitializeButtons ()
        {
            FindViewById<ImageButton>(Resource.Id.MainCalendarButton).Click += (s, e) => ShowFragment(_calendarFragment);
            FindViewById<ImageButton>(Resource.Id.MainScheduleButton).Click += (s, e) => ShowFragment(_scheduleFragment);
            FindViewById<ImageButton>(Resource.Id.MainListButton).Click += (s, e) => ShowFragment(_listTasksFragment);
            FindViewById<ImageButton>(Resource.Id.MainTimerButton).Click += (s, e) => ShowFragment(_timerFragment);
            FindViewById<ImageButton>(Resource.Id.MainAccountButton).Click += (s, e) => ShowFragment(_accountFragment);
        }
        #endregion

        public void UpdateFragments ()
        {
            _listTasksFragment.UpdateListView();
        }
    }
}