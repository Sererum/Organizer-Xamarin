using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Organizer.Internal.Activity;
using Organizer.Internal.Data;
using Organizer.Internal.Model;
using Organizer.Internal.Resources;
using System;
using static Android.App.ActionBar;

namespace Organizer.Internal.Fragments
{
    public class AccountFragment : Fragment
    {
        private readonly Android.App.Activity _context;
        private readonly MainActivity _mainActivity;
        private LinearLayout _mainLayout;

        private Color _mainColor;
        private PorterDuffColorFilter _mainFilter;
        private Color _textColor;
        private PorterDuffColorFilter _textFilter;
        private Color _downColor;
        private PorterDuffColorFilter _downFilter;

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

            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            layoutParams.SetMargins(0, 10, 0, 10);
            LinearLayout.LayoutParams titleParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            titleParams.SetMargins(0, 80, 0, 10);

            #region Initialize paints

            _mainColor = Storage.GetColor(_mainActivity.Designer.GetIdMainColor());
            _mainFilter = new PorterDuffColorFilter(_mainColor, PorterDuff.Mode.SrcAtop);
            _textColor = Storage.GetColor(_mainActivity.Designer.GetIdTextColor());
            _textFilter = new PorterDuffColorFilter(_textColor, PorterDuff.Mode.SrcAtop);
            _downColor = Storage.GetColor(_mainActivity.Designer.GetIdDownPanelColor());
            _downFilter = new PorterDuffColorFilter(_downColor, PorterDuff.Mode.SrcAtop);

            #endregion

            _mainLayout.AddView(GetTitleView(Resource.String.title_statistics), titleParams);
            _mainLayout.AddView(GetCounterView(), layoutParams);

            _mainLayout.AddView(GetTitleView(Resource.String.title_settings), titleParams);
            _mainLayout.AddView(GetChangeLanguageView(), layoutParams);
            _mainLayout.AddView(GetChangeMainScreenView(), layoutParams);
            _mainLayout.AddView(GetThemeChangeView(), layoutParams);
        }

        private View GetTitleView(int idTitle)
        {
            #region Initialize views

            View view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_account_title, null);

            RelativeLayout backgroundLayout = view.FindViewById<RelativeLayout>(Resource.Id.TitleBackgroundLayout);
            RelativeLayout mainLayout = view.FindViewById<RelativeLayout>(Resource.Id.TitleMainLayout);
            TextView titleTextView = view.FindViewById<TextView>(Resource.Id.TitleTextView);

            #endregion

            titleTextView.Text = _mainActivity.Translater.GetString(idTitle);

            #region Paint view

            backgroundLayout.Background.SetColorFilter(_textFilter);
            mainLayout.Background.SetColorFilter(_downFilter);

            titleTextView.SetTextColor(_textColor);

            #endregion

            return view;

        }

        private View GetCounterView ()
        {
            #region Initialize views

            View view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_account_counter_tasks, null);

            RelativeLayout backgroundLayout = view.FindViewById<RelativeLayout>(Resource.Id.CounterBackgroundLayout);
            RelativeLayout mainLayout = view.FindViewById<RelativeLayout>(Resource.Id.CounterMainLayout);
            TextView completedTaskTextView = view.FindViewById<TextView>(Resource.Id.CounterTaskTextView);
            TextView counterTextView = view.FindViewById<TextView>(Resource.Id.CounterTaskCounterTextView);
            SeekBar counterSeekBar = view.FindViewById<SeekBar>(Resource.Id.CounterTaskSeekBar);

            #endregion

            completedTaskTextView.Text = _mainActivity.Translater.GetString(Resource.String.complete_tasks);

            ListTasks todayList = Server.GetList(Server.Period.Day, DateTime.Now);
            int countAllTasks = todayList.GetCountTasks(ListTasks.TaskCounter.WithoutProject);
            int countCompleteTasks = todayList.GetCountTasks(ListTasks.TaskCounter.Complete_WithoutProject);
            counterTextView.Text = countCompleteTasks + " / " + countAllTasks;

            #region Paint view

            backgroundLayout.Background.SetColorFilter(_textFilter);
            mainLayout.Background.SetColorFilter(_mainFilter);

            completedTaskTextView.SetTextColor(_textColor);
            counterTextView.SetTextColor(_textColor);

            #endregion

            counterSeekBar.Max = countAllTasks;
            counterSeekBar.Progress = countCompleteTasks;
            counterSeekBar.Enabled = false;

            return view;
        }

        private View GetChangeLanguageView ()
        {
            #region Initialize views

            View view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_account_change, null);

            RelativeLayout backgroundLayout = view.FindViewById<RelativeLayout>(Resource.Id.ChangeBackgroundLayout);
            RelativeLayout mainLayout = view.FindViewById<RelativeLayout>(Resource.Id.ChangeMainLayout);
            TextView languageTextView = view.FindViewById<TextView>(Resource.Id.ChangeTextView);
            TextView selectTextView = view.FindViewById<TextView>(Resource.Id.ChangeSelectTextView);

            #endregion

            languageTextView.Text = _mainActivity.Translater.GetString(Resource.String.current_language);

            #region Paint views

            backgroundLayout.Background.SetColorFilter(_textFilter);
            mainLayout.Background.SetColorFilter(_mainFilter);

            languageTextView.SetTextColor(_textColor);
            selectTextView.SetTextColor(_textColor);

            #endregion

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
                popup.MenuInflater.Inflate(Resource.Menu.change_language_menu, popup.Menu);
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

        private View GetChangeMainScreenView ()
        {
            #region Initialize views

            View view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_account_change, null);

            RelativeLayout backgroundLayout = view.FindViewById<RelativeLayout>(Resource.Id.ChangeBackgroundLayout);
            RelativeLayout mainLayout = view.FindViewById<RelativeLayout>(Resource.Id.ChangeMainLayout);
            TextView screenTextView = view.FindViewById<TextView>(Resource.Id.ChangeTextView);
            TextView selectTextView = view.FindViewById<TextView>(Resource.Id.ChangeSelectTextView);

            #endregion

            screenTextView.Text = _mainActivity.Translater.GetString(Resource.String.current_screen);

            #region Paint views

            backgroundLayout.Background.SetColorFilter(_textFilter);
            mainLayout.Background.SetColorFilter(_mainFilter);

            screenTextView.SetTextColor(_textColor);
            selectTextView.SetTextColor(_textColor);

            #endregion

            switch (Server.StartScreen)
            {
                case (int) MainActivity.StartScreen.Calendar:
                    selectTextView.Text = _mainActivity.Translater.GetString(Resource.String.calendar);
                    break;
                case (int) MainActivity.StartScreen.Schedule:
                    selectTextView.Text = _mainActivity.Translater.GetString(Resource.String.schedule);
                    break;
                case (int) MainActivity.StartScreen.List:
                    selectTextView.Text = _mainActivity.Translater.GetString(Resource.String.list_tasks);
                    break;
                case (int) MainActivity.StartScreen.Account:
                    selectTextView.Text = _mainActivity.Translater.GetString(Resource.String.account);
                    break;
            }

            mainLayout.Click += (s, e) =>
            {
                PopupMenu popup = new PopupMenu(_context, view);
                popup.MenuInflater.Inflate(Resource.Menu.change_screen_menu, popup.Menu);
                popup.Show();

                popup.MenuItemClick += (s, e) =>
                {
                    switch (e.Item.ItemId)
                    {
                        case Resource.Id.screen_list:
                            selectTextView.Text = _mainActivity.Translater.GetString(Resource.String.list_tasks);
                            Server.StartScreen = (int) MainActivity.StartScreen.List;
                            break;
                        case Resource.Id.screen_schedule:
                            selectTextView.Text = _mainActivity.Translater.GetString(Resource.String.schedule);
                            Server.StartScreen = (int) MainActivity.StartScreen.Schedule;
                            break;
                        case Resource.Id.screen_calendar:
                            selectTextView.Text = _mainActivity.Translater.GetString(Resource.String.calendar);
                            Server.StartScreen = (int) MainActivity.StartScreen.Calendar;
                            break;
                        case Resource.Id.screen_account:
                            selectTextView.Text = _mainActivity.Translater.GetString(Resource.String.account);
                            Server.StartScreen = (int) MainActivity.StartScreen.Account;
                            break;
                    }
                };
            };

            return view;
        }

        private View GetThemeChangeView ()
        {
            #region Initialize views

            View view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_account_change, null);

            RelativeLayout backgroundLayout = view.FindViewById<RelativeLayout>(Resource.Id.ChangeBackgroundLayout);
            RelativeLayout mainLayout = view.FindViewById<RelativeLayout>(Resource.Id.ChangeMainLayout);
            TextView screenTextView = view.FindViewById<TextView>(Resource.Id.ChangeTextView);
            TextView selectTextView = view.FindViewById<TextView>(Resource.Id.ChangeSelectTextView);

            #endregion

            screenTextView.Text = _mainActivity.Translater.GetString(Resource.String.current_theme);

            #region Paint views

            backgroundLayout.Background.SetColorFilter(_textFilter);
            mainLayout.Background.SetColorFilter(_mainFilter);

            screenTextView.SetTextColor(_textColor);
            selectTextView.SetTextColor(_textColor);

            #endregion

            selectTextView.Text = GetThemeName(_mainActivity.Designer.CurrentTheme);

            mainLayout.Click += (s, e) =>
            {
                PopupMenu popup = new PopupMenu(_context, view);
                popup.MenuInflater.Inflate(Resource.Menu.change_theme_menu, popup.Menu);
                popup.Show();

                popup.MenuItemClick += (s, e) =>
                {
                    switch (e.Item.ItemId)
                    {
                        case Resource.Id.theme_main:
                            selectTextView.Text = GetThemeName(Designer.Theme.Main);
                            break;
                        case Resource.Id.theme_soft:
                            selectTextView.Text = GetThemeName(Designer.Theme.Soft);
                            break;
                        case Resource.Id.theme_purple:
                            selectTextView.Text = GetThemeName(Designer.Theme.Purple);
                            break;
                        case Resource.Id.theme_main_dark:
                            selectTextView.Text = GetThemeName(Designer.Theme.MainDark);
                            break;
                        case Resource.Id.theme_deep_water:
                            selectTextView.Text = GetThemeName(Designer.Theme.DeepWater);
                            break;
                        case Resource.Id.theme_dark_purple:
                            selectTextView.Text = GetThemeName(Designer.Theme.DarkPurple);
                            break;
                    }
                    _mainActivity.UpdateFragments();
                    _mainActivity.RepaintFragments();
                };
            };

            return view;
        }

        private string GetThemeName(Designer.Theme setTheme)
        {
            _mainActivity.Designer.CurrentTheme = setTheme;
            switch (_mainActivity.Designer.CurrentTheme)
            {
                case Designer.Theme.Main:
                    return _mainActivity.Translater.GetString(Resource.String.main);
                case Designer.Theme.Soft:
                    return _mainActivity.Translater.GetString(Resource.String.soft);
                case Designer.Theme.Purple:
                    return _mainActivity.Translater.GetString(Resource.String.purple);
                case Designer.Theme.MainDark:
                    return _mainActivity.Translater.GetString(Resource.String.main_dark);
                case Designer.Theme.DeepWater:
                    return _mainActivity.Translater.GetString(Resource.String.deep_water);
                case Designer.Theme.DarkPurple:
                    return _mainActivity.Translater.GetString(Resource.String.dark_purple);
                default:
                    throw new ArgumentException();
            }
        }

        public override void OnHiddenChanged (bool hidden)
        {
            base.OnHiddenChanged(hidden);
            UpdateListView();
        }
    }
}