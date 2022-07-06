using Android.Graphics;
using Android.Views;
using Android.Widget;
using Organizer.Internal.Activity;
using Organizer.Internal.Data;
using System;
using Period = Organizer.Internal.Data.Server.Period;

namespace Organizer.Internal.ArrayAdapters
{
    class PeriodArrayAdapter : BaseAdapter<string>
    {
        private readonly Android.App.Activity _context;
        private readonly MainActivity _mainActivity;
        private string[] _namePeriods = new string[4];

        public PeriodArrayAdapter (Android.App.Activity context, bool firstCreate = false)
        {
            _context = context;
            _mainActivity = context as MainActivity;
        }

        public override string this[int position] => _namePeriods[position];

        public override int Count => 4;

        public override long GetItemId (int position) => position;

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            #region Initialize

            View view = convertView;

            if (view is null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_period, null);
            }

            RelativeLayout mainLayout = view.FindViewById<RelativeLayout>(Resource.Id.PeriodItemMainLayout);
            TextView namePeriodView = view.FindViewById<TextView>(Resource.Id.NamePeriodTextView);

            #endregion

            string text = "";
            switch ((Period) position)
            {
                case Period.Day:
                    text = NameDatePeriod.GetNameDate(Storage.DayDate, Period.Day);
                    break;
                case Period.Month:
                    text = NameDatePeriod.GetNameDate(Storage.MonthDate, Period.Month);
                    break;
                case Period.Year:
                    text = NameDatePeriod.GetNameDate(Storage.YearDate, Period.Year);
                    break;
                case Period.Global:
                    text = NameDatePeriod.GetNameDate(DateTime.Now, Period.Global);
                    break;
            }

            namePeriodView.Text = text;
            _namePeriods[position] = text;

            int verticalPadding = (int) Storage.DpToPx(26);
            namePeriodView.SetPadding(0, verticalPadding / 2, 0, verticalPadding / 2);

            #region Paint view

            Color textColor = Storage.GetColor(_mainActivity.Designer.GetIdTextColor());
            PorterDuffColorFilter textFilter = new PorterDuffColorFilter(textColor, PorterDuff.Mode.SrcAtop);
            Color toolBarColor = Storage.GetColor(_mainActivity.Designer.GetIdToolBarColor());
            PorterDuffColorFilter toolBarFilter = new PorterDuffColorFilter(toolBarColor, PorterDuff.Mode.SrcAtop);

            namePeriodView.Background.SetColorFilter(toolBarFilter);

            mainLayout.Background.SetColorFilter(textFilter);
            namePeriodView.SetTextColor(textColor);

            #endregion

            return view;
        }
    }
}