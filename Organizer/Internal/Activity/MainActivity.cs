using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Content;
using Organizer.Internal.ArrayAdapters;
using Organizer.Internal.Data;
using Organizer.Internal.Fragments;
using Organizer.Internal.Model;
using Organizer.Internal.Model.Task;
using Organizer.Internal.Resources;
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
        private Translater _translater;
        private Designer _designer;

        public Fragment CurrentFragment => _currentFragment;
        public Translater Translater => _translater;
        public Designer Designer => _designer;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            InitializeResources();
            Storage.InitializeListsTasks(this);

            InitializeFragments(savedInstanceState);
            InitializeButtons();

            PaintActivity();
        }

        private void InitializeResources ()
        {
            _translater = new Translater();
            _designer = new Designer();
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

        #endregion

        public void InitializeButtons ()
        {
            ImageButton calendarButton = FindViewById<ImageButton>(Resource.Id.MainCalendarButton);
            ImageButton scheduleButton = FindViewById<ImageButton>(Resource.Id.MainScheduleButton);
            ImageButton listTasksButton = FindViewById<ImageButton>(Resource.Id.MainListButton);
            ImageButton accountButton = FindViewById<ImageButton>(Resource.Id.MainAccountButton);

            Color buttonColor = Storage.GetColor(Designer.GetIdDownPanelElementsColor());
            PorterDuffColorFilter colorFilter = new PorterDuffColorFilter(buttonColor, PorterDuff.Mode.SrcAtop);
            calendarButton.Background.SetColorFilter(colorFilter);
            scheduleButton.Background.SetColorFilter(colorFilter);
            listTasksButton.Background.SetColorFilter(colorFilter);
            accountButton.Background.SetColorFilter(colorFilter);

            calendarButton.Click += (s, e) => ShowFragment(_calendarFragment);
            scheduleButton.Click += (s, e) => ShowFragment(_scheduleFragment);
            listTasksButton.Click += (s, e) => ShowFragment(_listTasksFragment);
            accountButton.Click += (s, e) => ShowFragment(_accountFragment);
        }

        public void ShowCreateFragment (ListTasks list, BaseTask editTask = null, bool disableRoutine = false, int scheduleHour = -1)
        {
            _createFragment = new CreateFragment(this, list, disableRoutine, editTask, scheduleHour);
            var fragmentTransaction = SupportFragmentManager.BeginTransaction();
            fragmentTransaction.Add(Resource.Id.MainFragmentLayout, _createFragment).Hide(_createFragment);
            fragmentTransaction.Commit();
            ShowFragment(_createFragment);
        }

        public void UpdateFragments ()
        {
            Storage.SynchronizeLists(_currentFragment);
            _listTasksFragment.Update();
            _calendarFragment.UpdateListView();
            _scheduleFragment.UpdateListView();
            _accountFragment.UpdateListView();
        }

        private void PaintActivity ()
        {
            RelativeLayout fragmentLayout = FindViewById<RelativeLayout>(Resource.Id.MainFragmentLayout);
            RelativeLayout buttonsLayout = FindViewById<RelativeLayout>(Resource.Id.MainButtonsLayout);

            fragmentLayout.SetBackgroundColor(Storage.GetColor(Designer.GetIdMainColor()));
            buttonsLayout.SetBackgroundColor(Storage.GetColor(Designer.GetIdDownPanelColor()));
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