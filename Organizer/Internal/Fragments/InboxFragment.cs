using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Organizer.Internal.Activity;

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
        private ListView _listView;

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
            _listView = view.FindViewById<ListView>(Resource.Id.InboxTasksListView);

            return view;
        }

        public void UpdateListView ()
        {

        }

        public void PaintViews ()
        {

        }
    }
}