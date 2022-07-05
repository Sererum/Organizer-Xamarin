using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Organizer.Internal.Activity;
using Organizer.Internal.ArrayAdapters;
using Organizer.Internal.Data;
using Organizer.Internal.Model;
using Organizer.Internal.Model.Task;
using System;
using static Android.App.ActionBar;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Organizer.Internal.Fragments
{
    public class CreateFragment : Fragment
    {
        private readonly Android.App.Activity _context;
        private readonly MainActivity _mainActivity;
        private readonly ListTasks _mainList;
        private readonly bool _disableRoutine;
        private readonly bool _isEdit;
        private readonly int _scheduleHour;

        private BaseTask _editTask;
        private ListTasks _projectList;

        private EditText _titleEditText;
        private EditText _textEditText;
        private TextView _startTextView;
        private TextView _endTextView;
        private RadioGroup _typeTaskRadioGroup;
        private RelativeLayout _projectLayout;
        private RelativeLayout _regularLayout;
        private RelativeLayout _routineLayout;
        private RadioButton _regularRadioButton;
        private RadioButton _projectRadioButton;
        private RadioButton _routineRadioButton;

        private LinearLayout _projectTasksLayout;
        private Spinner _prioritySpinner;
        private CheckBox _sundayCheckBox;
        private CheckBox _mondayCheckBox;
        private CheckBox _tuesdayCheckBox;
        private CheckBox _wednesdayCheckBox;
        private CheckBox _thursdayCheckBox;
        private CheckBox _fridayCheckBox;
        private CheckBox _saturdayCheckBox;

        public CreateFragment (Android.App.Activity context, ListTasks mainList, bool disableRoutine = false, BaseTask editTask = null, int scheduleHour = -1)
        {
            _context = context;
            _mainActivity = context as MainActivity;
            _mainList = mainList;
            _disableRoutine = disableRoutine;
            _editTask = editTask;
            _scheduleHour = scheduleHour;
            _isEdit = (editTask != null);
            _projectList = new ListTasks();
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            #region Initialize Views

            View view =  inflater.Inflate(Resource.Layout.fragment_create, container, false);

            RelativeLayout titleLayout = view.FindViewById<RelativeLayout>(Resource.Id.CreateTitleLayout);
            _titleEditText = view.FindViewById<EditText>(Resource.Id.CreateTitleEditText);
            _textEditText = view.FindViewById<EditText>(Resource.Id.CreateTextEditText);
            TextView startTextView = view.FindViewById<TextView>(Resource.Id.ListStartTimeTextView);
            _startTextView = view.FindViewById<TextView>(Resource.Id.StartTextView);
            TextView endTextView = view.FindViewById<TextView>(Resource.Id.ListEndTimeTextView);
            _endTextView = view.FindViewById<TextView>(Resource.Id.EndTextView);
            _typeTaskRadioGroup = view.FindViewById<RadioGroup>(Resource.Id.CreateTypeRadioGroup);
            _regularRadioButton = view.FindViewById<RadioButton>(Resource.Id.CreateRegularRadioButton);
            _projectRadioButton = view.FindViewById<RadioButton>(Resource.Id.CreateProjectRadioButton);
            _routineRadioButton = view.FindViewById<RadioButton>(Resource.Id.CreateRoutineRadioButton);

            _projectLayout = view.FindViewById<RelativeLayout>(Resource.Id.CreateProjectLayout);
            TextView projectTextView = view.FindViewById<TextView>(Resource.Id.CreateProjectTextView);
            _projectTasksLayout = view.FindViewById<LinearLayout>(Resource.Id.ProjectTasksLayout);

            TextView priorityTextView = view.FindViewById<TextView>(Resource.Id.PriorityTextView);
            _regularLayout = view.FindViewById<RelativeLayout>(Resource.Id.CreateRegularLayout);
            _prioritySpinner = view.FindViewById<Spinner>(Resource.Id.PrioritySpinner);

            TextView routineDaysTextView = view.FindViewById<TextView>(Resource.Id.RoutineDaysTextView);
            _routineLayout = view.FindViewById<RelativeLayout>(Resource.Id.CreateRoutineLayout);
            _sundayCheckBox = view.FindViewById<CheckBox>(Resource.Id.SundayCheckBox);
            _mondayCheckBox = view.FindViewById<CheckBox>(Resource.Id.MondayCheckBox);
            _tuesdayCheckBox = view.FindViewById<CheckBox>(Resource.Id.TuesdayCheckBox);
            _wednesdayCheckBox = view.FindViewById<CheckBox>(Resource.Id.WednesdayCheckBox);
            _thursdayCheckBox = view.FindViewById<CheckBox>(Resource.Id.ThursdayCheckBox);
            _fridayCheckBox = view.FindViewById<CheckBox>(Resource.Id.FridayCheckBox);
            _saturdayCheckBox = view.FindViewById<CheckBox>(Resource.Id.SaturdayCheckBox);

            ImageButton okButton = view.FindViewById<ImageButton>(Resource.Id.OkCreateButton);
            ImageButton cancelButton = view.FindViewById<ImageButton>(Resource.Id.CancelCreateButton);
            ImageButton addButton = view.FindViewById<ImageButton>(Resource.Id.ProjectAddButton);

            #endregion

            #region Paint view

            Color mainColor = Storage.GetColor(_mainActivity.Designer.GetIdMainColor());
            PorterDuffColorFilter mainFilter = new PorterDuffColorFilter(mainColor, PorterDuff.Mode.SrcAtop);
            Color downColor = Storage.GetColor(_mainActivity.Designer.GetIdDownPanelColor());
            PorterDuffColorFilter downFilter = new PorterDuffColorFilter(downColor, PorterDuff.Mode.SrcAtop);
            Color buttonColor = Storage.GetColor(_mainActivity.Designer.GetIdElementsColor());
            PorterDuffColorFilter buttonFilter = new PorterDuffColorFilter(buttonColor, PorterDuff.Mode.SrcAtop);
            Color textColor = Storage.GetColor(_mainActivity.Designer.GetIdTextColor());
            PorterDuffColorFilter textFilter = new PorterDuffColorFilter(textColor, PorterDuff.Mode.SrcAtop);

            titleLayout.Background.SetColorFilter(downFilter);

            _projectLayout.Background.SetColorFilter(textFilter);
            projectTextView.Background.SetColorFilter(mainFilter);

            okButton.Background.SetColorFilter(buttonFilter);
            cancelButton.Background.SetColorFilter(buttonFilter);
            addButton.Background.SetColorFilter(buttonFilter);

            foreach (TextView textView in new TextView[] { _startTextView, startTextView,
                _endTextView, endTextView, priorityTextView, routineDaysTextView })
            {
                textView.SetTextColor(textColor);
            }

            _titleEditText.SetTextColor(textColor);
            _titleEditText.SetHintTextColor(textColor);
            _textEditText.SetTextColor(textColor);
            _textEditText.SetHintTextColor(textColor);

            foreach (RadioButton radio in new RadioButton[] { _regularRadioButton, _projectRadioButton, _routineRadioButton })
            {
                radio.ButtonDrawable.SetColorFilter(textFilter);
                radio.SetTextColor(textColor);
            }

            foreach (CheckBox check in new CheckBox[] { _sundayCheckBox , _mondayCheckBox , _tuesdayCheckBox,
                _wednesdayCheckBox, _thursdayCheckBox, _fridayCheckBox, _saturdayCheckBox })
            {
                check.ButtonDrawable.SetColorFilter(textFilter);
                check.SetTextColor(textColor);
            }

            #endregion

            #region Set text

            _titleEditText.Hint = _mainActivity.Translater.GetString(Resource.String.hint_title);
            _textEditText.Hint = _mainActivity.Translater.GetString(Resource.String.hint_text);
            startTextView.Text = _mainActivity.Translater.GetString(Resource.String.start_time);
            endTextView.Text = _mainActivity.Translater.GetString(Resource.String.end_time);
            _regularRadioButton.Text = _mainActivity.Translater.GetString(Resource.String.regular);
            _routineRadioButton.Text = _mainActivity.Translater.GetString(Resource.String.routine);
            _projectRadioButton.Text = _mainActivity.Translater.GetString(Resource.String.project);
            priorityTextView.Text = _mainActivity.Translater.GetString(Resource.String.regular_priority);
            routineDaysTextView.Text = _mainActivity.Translater.GetString(Resource.String.routine_days);

            int length = 1;
            _sundayCheckBox.Text = _mainActivity.Translater.GetString(Resource.String.sunday_short).Substring(0, length);
            _mondayCheckBox.Text = _mainActivity.Translater.GetString(Resource.String.monday_short).Substring(0, length);
            _tuesdayCheckBox.Text = _mainActivity.Translater.GetString(Resource.String.tuesday_short).Substring(0, length);
            _wednesdayCheckBox.Text = _mainActivity.Translater.GetString(Resource.String.wednesday_short).Substring(0, length);
            _thursdayCheckBox.Text = _mainActivity.Translater.GetString(Resource.String.thursday_short).Substring(0, length);
            _fridayCheckBox.Text = _mainActivity.Translater.GetString(Resource.String.friday_short).Substring(0, length);
            _saturdayCheckBox.Text = _mainActivity.Translater.GetString(Resource.String.saturday_short).Substring(0, length);

            #endregion

            _titleEditText.Text = _isEdit ? _editTask.Title : "";
            _textEditText.Text = _isEdit ? _editTask.Text : "";

            view.FindViewById<RelativeLayout>(Resource.Id.CreateStartLayout).Click += (s, e) => TimeLayout_Click(_startTextView);
            view.FindViewById<RelativeLayout>(Resource.Id.CreateEndLayout).Click += (s, e) => TimeLayout_Click(_endTextView);

            string emptyTime = _context.GetString(Resource.String.empty_time);

            if (_isEdit)
            {
                _startTextView.Text = _editTask.StartTime == "" ? emptyTime : _editTask.StartTime;
                _endTextView.Text = _editTask.EndTime == "" ? emptyTime : _editTask.EndTime;
            }
            else if (_scheduleHour != -1)
            {
                _startTextView.Text = Storage.TimeToStandart(_scheduleHour, 0);
                _endTextView.Text = Storage.TimeToStandart(_scheduleHour + 1, 0);
            }

            _typeTaskRadioGroup.CheckedChange += (s, e) => ChangeTasksLayoutVisible();

            if (_disableRoutine)
            {
                _routineRadioButton.Visibility = ViewStates.Gone;
            }

            UpdateProjectLayout();
            addButton.Click += (s, e) => _mainActivity.ShowCreateFragment(_projectList, disableRoutine: true);

            _prioritySpinner.Adapter = new PriorityArrayAdapter(_context);
            _prioritySpinner.SetSelection(4);

            if (_isEdit)
            {
                switch (_editTask.TypeTask)
                {
                    case (int) BaseTask.Type.Project:
                        _projectRadioButton.Checked = true;
                        _projectList = (_editTask as Project).Tasks;
                        UpdateProjectLayout();
                        break;
                    case (int) BaseTask.Type.Regular:
                        _regularRadioButton.Checked = true;
                        _prioritySpinner.SetSelection(9 - (_editTask as Regular).Priority);
                        break;
                    case (int) BaseTask.Type.Routine:
                        _routineRadioButton.Checked = true;
                        Routine routine = (_editTask as Routine);
                        _sundayCheckBox.Checked = routine.SDays.Contains("0");
                        _mondayCheckBox.Checked = routine.SDays.Contains("1");
                        _tuesdayCheckBox.Checked = routine.SDays.Contains("2");
                        _wednesdayCheckBox.Checked = routine.SDays.Contains("3");
                        _thursdayCheckBox.Checked = routine.SDays.Contains("4");
                        _fridayCheckBox.Checked = routine.SDays.Contains("5");
                        _saturdayCheckBox.Checked = routine.SDays.Contains("6");
                        break;
                }
            }
            else
            {
                _regularRadioButton.Checked = true;
            }

            okButton.Click += (s, e) => OkButton_Click();
            cancelButton.Click += (s, e) => _mainActivity.OnBackPressed();

            return view;
        }

        private void TimeLayout_Click (TextView textView)
        {
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            string emptyTime = _context.GetString(Resource.String.empty_time);
            if (textView.Text != emptyTime)
            {
                hour = Int32.Parse(textView.Text.Substring(0, 2));
                minute = Int32.Parse(textView.Text.Substring(3, 2));
            }
            new TimePickerDialog(
                _context, (s, e) => textView.Text = Storage.TimeToStandart(e.HourOfDay, e.Minute), hour, minute, true
                ).Show();
        }

        private void ChangeTasksLayoutVisible ()
        {
            _projectLayout.Visibility = ViewStates.Gone;
            _regularLayout.Visibility = ViewStates.Gone;
            _routineLayout.Visibility = ViewStates.Gone;
            switch (_typeTaskRadioGroup.CheckedRadioButtonId)
            {
                case Resource.Id.CreateProjectRadioButton:
                    _projectLayout.Visibility = ViewStates.Visible;
                    break;
                case Resource.Id.CreateRegularRadioButton:
                    _regularLayout.Visibility = ViewStates.Visible;
                    break;
                case Resource.Id.CreateRoutineRadioButton:
                    _routineLayout.Visibility = ViewStates.Visible;
                    break;
            }
        }

        private void UpdateProjectLayout ()
        {
            _projectTasksLayout.RemoveAllViews();

            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            layoutParams.SetMargins(12, 10, 10, 0);

            foreach (BaseTask task in _projectList)
            {
                TaskViewConstructor constructor = new TaskViewConstructor(_context, isSimple: true);
                _projectTasksLayout.AddView(constructor.GetTaskView(task), layoutParams);
            }
        }

        private string GetSRoutineDays() => (_sundayCheckBox.Checked ? "0" : "") +
                    (_mondayCheckBox.Checked ? "1" : "") + (_tuesdayCheckBox.Checked ? "2" : "") +
                    (_wednesdayCheckBox.Checked ? "3" : "") + (_thursdayCheckBox.Checked ? "4" : "") +
                    (_fridayCheckBox.Checked ? "5" : "") + (_saturdayCheckBox.Checked ? "6" : "");

        private void OkButton_Click ()
        {
            if (BaseTask.ToStandart(_titleEditText.Text) == "")
            {
                _titleEditText.SetBackgroundColor(Color.Red);
                _titleEditText.Text = "";
                return;
            }
            string emptyTime = _context.GetString(Resource.String.empty_time);
            string startTime = (_startTextView.Text == emptyTime) ? "" : _startTextView.Text;
            string endTime = (_endTextView.Text == emptyTime) ? "" : _endTextView.Text;

            switch (_typeTaskRadioGroup.CheckedRadioButtonId)
            {
                case Resource.Id.CreateProjectRadioButton:
                    if (_isEdit)
                    {
                        _mainList.Remove(_editTask);
                    }
                    _mainList.Add(new Project(_titleEditText.Text, _textEditText.Text, startTime, endTime, _projectList));
                    break;
                case Resource.Id.CreateRegularRadioButton:
                    if (_isEdit)
                    {
                        _mainList.Remove(_editTask);
                    }
                    _mainList.Add(new Regular(_titleEditText.Text, _textEditText.Text, 
                        startTime, endTime, (int) _prioritySpinner.SelectedItemId));
                    break;
                case Resource.Id.CreateRoutineRadioButton:
                    if (GetSRoutineDays() == "")
                    {
                        _routineLayout.SetBackgroundColor(Color.Red);
                        return;
                    }
                    ListTasks routines = Server.Routines;
                    if (_isEdit)
                    {
                        routines.Remove(_editTask);
                        _mainList.Remove(_editTask);
                    }
                    routines.Add(new Routine(_titleEditText.Text, _textEditText.Text, startTime, endTime, GetSRoutineDays()));
                    Server.Routines = routines;
                    
                    break;
            }
            _mainActivity.OnBackPressed();
            _mainActivity.UpdateFragments();
        }

        public override void OnHiddenChanged (bool hidden)
        {
            base.OnHiddenChanged(hidden);
            UpdateProjectLayout();
        }
    }
}