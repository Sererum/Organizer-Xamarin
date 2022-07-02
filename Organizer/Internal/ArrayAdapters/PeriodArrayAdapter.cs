using Android.Views;
using Android.Widget;
using Organizer.Internal.Activity;
using Organizer.Internal.Data;
using Period = Organizer.Internal.Data.Server.Period;

namespace Organizer.Internal.ArrayAdapters
{
    class PeriodArrayAdapter : BaseAdapter<string>
    {
        private Android.App.Activity _context;
        private MainActivity _mainActivity;
        private string[] _namePeriods = new string[4];

        public PeriodArrayAdapter (Android.App.Activity context)
        {
            _context = context;
            _mainActivity = context as MainActivity;
        }

        public override string this[int position] => _namePeriods[position];

        public override int Count => 4;

        public override long GetItemId (int position) => position;

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view is null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_period, null);
            }

            TextView namePeriodView = view.FindViewById<TextView>(Resource.Id.NamePeriodTextView);

            string text = NameDatePeriod.GetNameDate(Storage.MainDate, (Period) position);
            namePeriodView.Text = text;
            _namePeriods[position] = text;

            int verticalPadding = (int) (_context.Resources.GetDimension(Resource.Dimension.height_tool_bar) - Storage.DpToPx(30));
            namePeriodView.SetPadding(0, verticalPadding / 2, 0, verticalPadding / 2);

            namePeriodView.SetBackgroundColor(Storage.GetColor(_mainActivity.Designer.GetIdToolBarColor()));
            namePeriodView.SetTextColor(Storage.GetColor(_mainActivity.Designer.GetIdTextColor()));

            return view;
        }
    }
}