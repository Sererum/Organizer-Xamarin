using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Organizer.Internal.Data;

namespace Organizer.Internal.ArrayAdapters
{
    class PriorityArrayAdapter : BaseAdapter<string>
    {
        private readonly Android.App.Activity _context;

        public PriorityArrayAdapter (Android.App.Activity context)
        {
            _context = context;
        }

        public override string this[int position] => (9 - position).ToString();

        public override int Count => 9;

        public override long GetItemId (int position) => 9 - position;

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view is null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_priority, null);
            }

            TextView _priorityView = view.FindViewById<TextView>(Resource.Id.ItemPriorityTextView);
            Color color = new Color(ContextCompat.GetColor(_context, Storage.PriorityToColorId[(int) GetItemId(position)]));

            _priorityView.Text = this[position];
            _priorityView.SetTextColor(color);
            color.A = 25;
            _priorityView.SetBackgroundColor(color);

            return view;
        }
    }
}