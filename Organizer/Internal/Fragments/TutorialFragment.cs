using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Organizer.Internal.Activity;
using Organizer.Internal.Data;
using System;

namespace Organizer.Internal.Fragments
{
    public class TutorialFragment : Fragment
    {
        private readonly MainActivity _mainActivity;

        private readonly ImageButton _calendarButton;
        private readonly ImageButton _scheduleButton;
        private readonly ImageButton _listTasksButton;
        private readonly ImageButton _inboxButton;
        private readonly ImageButton _accountButton;

        private RelativeLayout _toolBarLayout;
        private RelativeLayout _centerLayout;
        private TextView _tutorTextView;
        private ImageButton _lastButton;
        private ImageButton _nextButton;

        private Color _backgroundColor;
        private PorterDuffColorFilter _backgroundFilter;
        private Color _textColor;
        private PorterDuffColorFilter _textFilter;
        private PorterDuffColorFilter _downPanelFilter;
        private PorterDuffColorFilter _brightFilter;

        private int _currentStep;

        public TutorialFragment (MainActivity mainActivity)
        {
            _mainActivity = mainActivity;

            _calendarButton = mainActivity.CalendarButton;
            _scheduleButton = mainActivity.ScheduleButton;
            _listTasksButton = mainActivity.ListTasksButton;
            _inboxButton = mainActivity.InboxButton;
            _accountButton = mainActivity.AccountButton;
    }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_tutorial, container, false);

            _toolBarLayout = view.FindViewById<RelativeLayout>(Resource.Id.TutorialToolBarLayout);
            _centerLayout = view.FindViewById<RelativeLayout>(Resource.Id.TutorialCenterLayout);
            _tutorTextView = view.FindViewById<TextView>(Resource.Id.TutorialTextView);
            _lastButton = view.FindViewById<ImageButton>(Resource.Id.TutorialLastStepButton);
            _nextButton = view.FindViewById<ImageButton>(Resource.Id.TutorialNextStepButton);

            _lastButton.Click += (s, e) => SetStep(isNextStep: false);
            _nextButton.Click += (s, e) => SetStep(isNextStep: true);

            _backgroundColor = Storage.GetColor(_mainActivity.Designer.GetIdTextColor());
            _backgroundColor.A = 200;
            _backgroundFilter = new PorterDuffColorFilter(_backgroundColor, PorterDuff.Mode.SrcAtop);
            _textColor = Storage.GetColor(_mainActivity.Designer.GetIdMainColor());
            _textFilter = new PorterDuffColorFilter(_textColor, PorterDuff.Mode.SrcAtop);
            Color downColor = Storage.GetColor(_mainActivity.Designer.GetIdDownPanelColor());
            _downPanelFilter = new PorterDuffColorFilter (downColor, PorterDuff.Mode.SrcAtop);
            _brightFilter = new PorterDuffColorFilter(Color.Orange, PorterDuff.Mode.SrcAtop);

            _lastButton.Background.SetColorFilter(_textFilter);
            _nextButton.Background.SetColorFilter(_textFilter);
            _tutorTextView.SetTextColor(_textColor);

            return view;
        }

        public void StartTutorial() => ShowStepTutorial(1);

        private void ShowStepTutorial(int step)
        {
            _currentStep = step;

            _toolBarLayout.SetBackgroundColor(_backgroundColor);
            _centerLayout.SetBackgroundColor(_backgroundColor);
            
            switch (step)
            {
                case 1:
                    _mainActivity.ShowListTasks();
                    _tutorTextView.Text = _mainActivity.Translater.GetString(Resource.String.text_step_one);
                    _lastButton.Visibility = ViewStates.Invisible;
                    break;
                case 2:
                    _mainActivity.ShowListTasks();
                    _tutorTextView.Text = _mainActivity.Translater.GetString(Resource.String.text_step_two);
                    _toolBarLayout.SetBackgroundColor(new Color());
                    _lastButton.Visibility = ViewStates.Visible;
                    break;
                case 3:
                    _mainActivity.ShowAccount();
                    _tutorTextView.Text = _mainActivity.Translater.GetString(Resource.String.text_step_three);
                    break;
                case 4:
                    _mainActivity.ShowCalendar();
                    _tutorTextView.Text = _mainActivity.Translater.GetString(Resource.String.text_step_four);
                    break;
                case 5:
                    _mainActivity.ShowCalendar();
                    _tutorTextView.Text = _mainActivity.Translater.GetString(Resource.String.text_step_five);
                    break;
                case 6:
                    _mainActivity.ShowInbox();
                    _tutorTextView.Text = _mainActivity.Translater.GetString(Resource.String.text_step_six);
                    _toolBarLayout.SetBackgroundColor(new Color());
                    break;
                case 7:
                    _mainActivity.ShowSchedule();
                    _tutorTextView.Text = _mainActivity.Translater.GetString(Resource.String.text_step_seven);
                    break;
                case 8:
                    _mainActivity.ShowSchedule();
                    _tutorTextView.Text = _mainActivity.Translater.GetString(Resource.String.text_step_eight);
                    break;
                case 9:
                    _mainActivity.ShowListTasks();
                    _tutorTextView.Text = _mainActivity.Translater.GetString(Resource.String.text_step_nine);
                    break;
                case 10:
                    _mainActivity.EndTutorial();
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void SetStep (bool isNextStep)
        {
            int addition = isNextStep ? 1 : -1;
            ShowStepTutorial(_currentStep + addition);
        }
    }
}