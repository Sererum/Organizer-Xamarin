using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Organizer.Internal.Activity;
using Organizer.Internal.ArrayAdapters;
using Organizer.Internal.Data;
using Organizer.Internal.Model;
using Organizer.Internal.Model.Task;
using static Android.App.ActionBar;

namespace Organizer.Internal.Fragments
{
    public class InboxFragment : Fragment
    {
        private readonly Android.App.Activity _context;
        private readonly MainActivity _mainActivity;

        private RelativeLayout _searchLayout;
        private TextView _searchBackgroundView;
        private ImageButton _searchButton;
        private EditText _searchEditText;
        private LinearLayout _listLayout;
        private ImageButton _addButton;

        public InboxFragment(Android.App.Activity context)
        {
            _context = context;
            _mainActivity = context as MainActivity;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_inbox, container, false);

            _searchLayout = view.FindViewById<RelativeLayout>(Resource.Id.InboxSearchLayout);
            _searchBackgroundView = view.FindViewById<TextView>(Resource.Id.InboxSearchTextView);
            _searchButton = view.FindViewById<ImageButton>(Resource.Id.InboxSearchButton);
            _searchEditText = view.FindViewById<EditText>(Resource.Id.InboxSearchEditText);
            _listLayout = view.FindViewById<LinearLayout>(Resource.Id.InboxTasksLayout);
            _addButton = view.FindViewById<ImageButton>(Resource.Id.InboxAddTaskButton);

            _searchEditText.ClearFocus();
            _searchEditText.AfterTextChanged += (s, e) => Search_AfterTextChanged();

            _addButton.Click += (s, e) => _mainActivity.ShowCreateFragment(Storage.InboxListTasks, disableRoutine: true);

            UpdateListView();
            PaintViews();

            return view;
        }

        private void Search_AfterTextChanged ()
        {
            ListTasks nowList = new ListTasks();
            string searchText = _searchEditText.Text;

            foreach (BaseTask task in Storage.InboxListTasks)
            {
                string title = task.Title;
                if (Storage.GetEditRatio(searchText, title) < 2)
                {
                    nowList.Add(task);
                }
            }
            nowList.Sort((task1, task2) => Storage.GetEditRatio(searchText, task1.Title) - Storage.GetEditRatio(searchText, task2.Title));
            UpdateListView(searchText == "" ? null : nowList);
        }

        public void UpdateListView (ListTasks list = null)
        {
            _searchEditText.Hint = _mainActivity.Translater.GetString(Resource.String.search);

            if (list is null)
            {
                list = Storage.InboxListTasks;
                list.Sort();
            }

            _listLayout.RemoveAllViews();
            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            layoutParams.SetMargins(16, 12, 14, 0);

            foreach (BaseTask task in list)
            {
                TaskViewConstructor constructor = new TaskViewConstructor (_context, isInbox: true);
                _listLayout.AddView(constructor.GetTaskView(task), layoutParams);
            }
        }

        public void PaintViews ()
        {
            Color textColor = Storage.GetColor(_mainActivity.Designer.GetIdTextColor());
            PorterDuffColorFilter textFilter = new PorterDuffColorFilter(textColor, PorterDuff.Mode.SrcAtop);
            Color toolBarColor = Storage.GetColor(_mainActivity.Designer.GetIdToolBarColor());
            PorterDuffColorFilter toolBarFilter = new PorterDuffColorFilter(toolBarColor, PorterDuff.Mode.SrcAtop);
            Color toolElementsColor = Storage.GetColor(_mainActivity.Designer.GetIdElementsColor());
            PorterDuffColorFilter buttonFilter = new PorterDuffColorFilter(toolElementsColor, PorterDuff.Mode.SrcAtop);

            _searchLayout.Background.SetColorFilter(textFilter);
            _searchBackgroundView.Background.SetColorFilter(toolBarFilter);

            _addButton.Background.SetColorFilter(buttonFilter);
            _searchButton.Background.SetColorFilter(buttonFilter);

            _searchEditText.SetTextColor(textColor);
            _searchEditText.SetHintTextColor(textColor);
        }
    }
}