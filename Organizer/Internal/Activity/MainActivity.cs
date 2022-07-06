using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Organizer.Internal.Data;
using Organizer.Internal.Fragments;
using Organizer.Internal.Model;
using Organizer.Internal.Model.Task;
using Organizer.Internal.Resources;
using System.Collections.Generic;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Organizer.Internal.Activity
{
    [Activity(Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public enum StartScreen { Calendar, Schedule, List, Inbox, Account }

        private CalendarFragment _calendarFragment;
        private ScheduleFragment _scheduleFragment;
        private ListTasksFragment _listTasksFragment;
        private InboxFragment _inboxFragment;
        private AccountFragment _accountFragment;
        private CreateFragment _createFragment;
        private TutorialFragment _tutorialFragment;

        private Fragment _currentFragment;
        private Stack<Fragment> _lastFragments;
        private Fragment[] _fragments;

        private Translater _translater;
        private Designer _designer;

        private ImageButton _calendarButton;
        private ImageButton _scheduleButton;
        private ImageButton _listTasksButton;
        private ImageButton _inboxButton;
        private ImageButton _accountButton;

        public Fragment CurrentFragment => _currentFragment;
        public Translater Translater => _translater;
        public Designer Designer => _designer;

        public ImageButton CalendarButton => _calendarButton;
        public ImageButton ScheduleButton => _scheduleButton;
        public ImageButton ListTasksButton => _listTasksButton;
        public ImageButton InboxButton => _inboxButton;
        public ImageButton AccountButton => _accountButton;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            InitializeResources();
            Storage.InitializeListsTasks(this);

            InitializeButtons();
            InitializeFragments(savedInstanceState);
            Window.SetSoftInputMode(SoftInput.StateHidden);

            PaintActivity();
            TutorialCheck();
        }

        private void TutorialCheck ()
        {
            if (Server.Tutorial == false)
            {
                return;
            }
            AlertDialog.Builder alert = new AlertDialog.Builder(this)
                .SetMessage("Привет, давай я быстренько объясню что здесь происходит")
                .SetPositiveButton("Да, давай", (s, e) => { StartTutorial(); })
                .SetNegativeButton("Нет, я сам", (s, e) => { return;});
            alert.Create().Show();
        }

        public void StartTutorial ()
        {
            var fragmentTransaction = SupportFragmentManager.BeginTransaction();
            fragmentTransaction.Show(_tutorialFragment);
            fragmentTransaction.AddToBackStack(null);
            fragmentTransaction.Commit();
            _tutorialFragment.StartTutorial();
        }

        public void EndTutorial ()
        {
            var fragmentTransaction = SupportFragmentManager.BeginTransaction();
            fragmentTransaction.Hide(_tutorialFragment);
            fragmentTransaction.AddToBackStack(null);
            fragmentTransaction.Commit();
            PaintButtons();
            Server.Tutorial = false;
        }

        private void InitializeResources ()
        {
            _translater = new Translater();
            _designer = new Designer();
        }

        public void InitializeButtons ()
        {
            _calendarButton = FindViewById<ImageButton>(Resource.Id.MainCalendarButton);
            _scheduleButton = FindViewById<ImageButton>(Resource.Id.MainScheduleButton);
            _listTasksButton = FindViewById<ImageButton>(Resource.Id.MainListButton);
            _inboxButton = FindViewById<ImageButton>(Resource.Id.MainInboxButton);
            _accountButton = FindViewById<ImageButton>(Resource.Id.MainAccountButton);

            _calendarButton.Click += (s, e) => ShowFragment(_calendarFragment);
            _scheduleButton.Click += (s, e) => ShowFragment(_scheduleFragment);
            _listTasksButton.Click += (s, e) => ShowFragment(_listTasksFragment);
            _inboxButton.Click += (s, e) => ShowFragment(_inboxFragment);
            _accountButton.Click += (s, e) => ShowFragment(_accountFragment);
        }

        #region Initialize fragments
        private void InitializeFragments (Bundle savedInstanceState)
        {
            _lastFragments = new Stack<Fragment>();

            _calendarFragment = new CalendarFragment(this);
            _scheduleFragment = new ScheduleFragment(this);
            _listTasksFragment = new ListTasksFragment(this);
            _inboxFragment = new InboxFragment(this);
            _accountFragment = new AccountFragment(this);

            _tutorialFragment = new TutorialFragment(this);

            _fragments = new Fragment[] { 
                _calendarFragment, _scheduleFragment, _listTasksFragment, 
                _inboxFragment, _accountFragment, _tutorialFragment };

            _currentFragment = _fragments[Server.StartScreen];

            var fragmentTransaction = SupportFragmentManager.BeginTransaction();
            if (savedInstanceState is null)
            {
                foreach (Fragment fragment in _fragments)
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

            PaintButtons();
            fragmentTransaction.AddToBackStack(null);
            fragmentTransaction.Commit();
        }

        #endregion

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
            _inboxFragment.UpdateListView();
            _accountFragment.UpdateListView();
        }

        public void RepaintFragments ()
        {
            PaintActivity();
            _calendarFragment.PaintViews();
            _scheduleFragment.PaintViews();
            _listTasksFragment.PaintViews();
            _inboxFragment.PaintViews();
            _accountFragment.UpdateListView();
        }

        private void PaintActivity ()
        {
            RelativeLayout mainLayout = FindViewById<RelativeLayout>(Resource.Id.MainLayout);
            RelativeLayout fragmentLayout = FindViewById<RelativeLayout>(Resource.Id.MainFragmentLayout);
            RelativeLayout buttonsLayout = FindViewById<RelativeLayout>(Resource.Id.MainButtonsLayout);

            Color mainColor = Storage.GetColor(Designer.GetIdMainColor());
            Color downColor = Storage.GetColor(Designer.GetIdDownPanelColor());
            PorterDuffColorFilter downFilter = new PorterDuffColorFilter(downColor, PorterDuff.Mode.SrcAtop);

            mainLayout.SetBackgroundColor(mainColor);
            fragmentLayout.SetBackgroundColor(mainColor);
            buttonsLayout.Background.SetColorFilter(downFilter);

            PaintButtons();
        }

        private void PaintButtons ()
        {
            Color buttonColor = Storage.GetColor(Designer.GetIdDownButtonsColor());
            PorterDuffColorFilter colorFilter = new PorterDuffColorFilter(buttonColor, PorterDuff.Mode.SrcAtop);
            Color toolelementsColor = Storage.GetColor(Designer.GetIdElementsColor());
            PorterDuffColorFilter toolElementsFilter = new PorterDuffColorFilter(toolelementsColor, PorterDuff.Mode.SrcAtop);

            _calendarButton.Background.SetColorFilter(CurrentFragment is CalendarFragment ? toolElementsFilter : colorFilter);
            _scheduleButton.Background.SetColorFilter(CurrentFragment is ScheduleFragment ? toolElementsFilter : colorFilter);
            _listTasksButton.Background.SetColorFilter(CurrentFragment is ListTasksFragment ? toolElementsFilter : colorFilter);
            _inboxButton.Background.SetColorFilter(CurrentFragment is InboxFragment ? toolElementsFilter : colorFilter);
            _accountButton.Background.SetColorFilter(CurrentFragment is AccountFragment ? toolElementsFilter : colorFilter);

        }

        public void ShowCalendar () => ShowFragment(_calendarFragment);
        public void ShowSchedule () => ShowFragment(_scheduleFragment);
        public void ShowListTasks () => ShowFragment(_listTasksFragment);
        public void ShowInbox () => ShowFragment(_inboxFragment);
        public void ShowAccount () => ShowFragment(_accountFragment);

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