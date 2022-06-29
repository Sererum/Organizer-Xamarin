using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Organizer.Internal.ArrayAdapters;
using Organizer.Internal.Data;

namespace Organizer.Internal.Fragments
{
    public class ListTasksFragment : Fragment
    {
        private readonly Android.App.Activity _context;

        private ListView _tasksListView;

        public ListTasksFragment(Android.App.Activity context)
        {
            _context = context;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view =  inflater.Inflate(Resource.Layout.fragment_list, container, false);

            ImageButton addButton = view.FindViewById<ImageButton>(Resource.Id.ListAddTaskButton);
            _tasksListView = view.FindViewById<ListView>(Resource.Id.ListTasksListView);

            UpdateListView();

            return view;
        }

        public void UpdateListView ()
        {
            Server.ListTasks.Sort();
            _tasksListView.Adapter = new TaskArrayAdapter(_context, null, Server.ListTasks);
        }
    }
}