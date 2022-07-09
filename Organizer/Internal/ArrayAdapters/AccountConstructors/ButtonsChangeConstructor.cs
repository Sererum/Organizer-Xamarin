using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Organizer.Internal.Activity;
using Organizer.Internal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Organizer.Internal.ArrayAdapters.AccountConstructors
{
    class ButtonsChangeConstructor
    {
        public static readonly string Sep = "_";
        private readonly MainActivity _mainActivity;

        #region Declaring  View-fields

        private RelativeLayout _backgroundLayout;
        private LinearLayout _mainLayout;

        private LinearLayout _layoutCalendar;
        private ImageView _imageCalendar;
        private ImageButton _deleteCalendar;
        private ImageButton _leftCalendar;
        private ImageButton _rightCalendar;

        private LinearLayout _layoutSchedule;
        private ImageView _imageSchedule;
        private ImageButton _deleteSchedule;
        private ImageButton _leftSchedule;
        private ImageButton _rightSchedule;

        private LinearLayout _layoutList;
        private ImageView _imageList;
        private ImageButton _deleteList;
        private ImageButton _leftList;
        private ImageButton _rightList;

        private LinearLayout _layoutInbox;
        private ImageView _imageInbox;
        private ImageButton _deleteInbox;
        private ImageButton _leftInbox;
        private ImageButton _rightInbox;

        private LinearLayout _layoutAccount;
        private ImageView _imageAccount;
        private ImageButton _leftAccount;
        private ImageButton _rightAccount;

        #endregion

        char[] _positions;
        char[] _visibility;

        public ButtonsChangeConstructor(MainActivity mainActivity)
        {
            _mainActivity = mainActivity;
        }

        public View GetView ()
        {
            #region Initialize fields

            View view = _mainActivity.LayoutInflater.Inflate(Resource.Layout.list_item_account_buttons, null);

            _backgroundLayout = view.FindViewById<RelativeLayout>(Resource.Id.ButtonsBackgroundLayout);
            _mainLayout = view.FindViewById<LinearLayout>(Resource.Id.ButtonsMainLayout);

            _layoutCalendar = view.FindViewById<LinearLayout>(Resource.Id.ButtonsCalendarLayout);
            _imageCalendar = view.FindViewById<ImageView>(Resource.Id.ButtonsCalendarImage);
            _deleteCalendar = view.FindViewById<ImageButton>(Resource.Id.ButtonsCalendarDeleteButton);
            _leftCalendar = view.FindViewById<ImageButton>(Resource.Id.ButtonsCalendarLeftButton);
            _rightCalendar = view.FindViewById<ImageButton>(Resource.Id.ButtonsCalendarRightLeftButton);

            _layoutSchedule = view.FindViewById<LinearLayout>(Resource.Id.ButtonsScheduleLayout);
            _imageSchedule = view.FindViewById<ImageView>(Resource.Id.ButtonsScheduleImage);
            _deleteSchedule = view.FindViewById<ImageButton>(Resource.Id.ButtonsScheduleDeleteButton);
            _leftSchedule = view.FindViewById<ImageButton>(Resource.Id.ButtonsScheduleLeftButton);
            _rightSchedule = view.FindViewById<ImageButton>(Resource.Id.ButtonsScheduleRightLeftButton);

            _layoutList = view.FindViewById<LinearLayout>(Resource.Id.ButtonsListLayout);
            _imageList = view.FindViewById<ImageView>(Resource.Id.ButtonsListImage);
            _deleteList = view.FindViewById<ImageButton>(Resource.Id.ButtonsListDeleteButton);
            _leftList = view.FindViewById<ImageButton>(Resource.Id.ButtonsListLeftButton);
            _rightList = view.FindViewById<ImageButton>(Resource.Id.ButtonsListRightLeftButton);

            _layoutInbox = view.FindViewById<LinearLayout>(Resource.Id.ButtonsInboxLayout);
            _imageInbox = view.FindViewById<ImageView>(Resource.Id.ButtonsInboxImage);
            _deleteInbox = view.FindViewById<ImageButton>(Resource.Id.ButtonsInboxDeleteButton);
            _leftInbox = view.FindViewById<ImageButton>(Resource.Id.ButtonsInboxLeftButton);
            _rightInbox = view.FindViewById<ImageButton>(Resource.Id.ButtonsInboxRightLeftButton);

            _layoutAccount = view.FindViewById<LinearLayout>(Resource.Id.ButtonsAccountLayout);
            _imageAccount = view.FindViewById<ImageView>(Resource.Id.ButtonsAccountImage);
            _leftAccount = view.FindViewById<ImageButton>(Resource.Id.ButtonsAccountLeftButton);
            _rightAccount = view.FindViewById<ImageButton>(Resource.Id.ButtonsAccountRightLeftButton);

            #endregion

            InitializeButtons();
            PaintView();
            
            _deleteCalendar.Click += (s, e) => DeleteButton_Click(_deleteCalendar, (int) MainActivity.StartScreen.Calendar);
            _deleteSchedule.Click += (s, e) => DeleteButton_Click(_deleteSchedule, (int) MainActivity.StartScreen.Schedule);
            _deleteList.Click += (s, e) => DeleteButton_Click(_deleteList, (int) MainActivity.StartScreen.List);
            _deleteInbox.Click += (s, e) => DeleteButton_Click(_deleteInbox, (int) MainActivity.StartScreen.Inbox);



            return view;
        }

        private void DeleteButton_Click (ImageButton button, int idScreen)
        {
            char charVisible = _visibility[idScreen] == '1' ? '0' : '1';
            int idDrawable = _visibility[idScreen] == '1' ? Resource.Drawable.ic_ok : Resource.Drawable.ic_cancel;

            _visibility[idScreen] = charVisible;
            button.Background = _mainActivity.GetDrawable(idDrawable);
            PaintView();
            SetPositions();
            _mainActivity.UpdateButtonsPositions();
        }

        private void InitializeButtons ()
        {
            string[] buttonProperties = Server.Buttons.Split(Sep);
            _positions = buttonProperties[0].ToCharArray();
            _visibility = buttonProperties[1].ToCharArray();
            
            _deleteCalendar.Background = _mainActivity.GetDrawable(
                (_visibility[(int) MainActivity.StartScreen.Calendar] == '0') ? Resource.Drawable.ic_ok : Resource.Drawable.ic_cancel);
            _deleteSchedule.Background = _mainActivity.GetDrawable(
                (_visibility[(int) MainActivity.StartScreen.Schedule] == '0') ? Resource.Drawable.ic_ok : Resource.Drawable.ic_cancel);
            _deleteList.Background = _mainActivity.GetDrawable(
                (_visibility[(int) MainActivity.StartScreen.List] == '0') ? Resource.Drawable.ic_ok : Resource.Drawable.ic_cancel);
            _deleteInbox.Background = _mainActivity.GetDrawable(
                (_visibility[(int) MainActivity.StartScreen.Inbox] == '0') ? Resource.Drawable.ic_ok : Resource.Drawable.ic_cancel);


        }

        private void PaintView ()
        {
            _mainActivity.PaintActivity();

            Color textColor = Storage.GetColor(_mainActivity.Designer.GetIdTextColor());
            PorterDuffColorFilter textFilter = new PorterDuffColorFilter(textColor, PorterDuff.Mode.SrcAtop);
            Color mainColor = Storage.GetColor(_mainActivity.Designer.GetIdMainColor());
            PorterDuffColorFilter mainFilter = new PorterDuffColorFilter(mainColor, PorterDuff.Mode.SrcAtop);
            Color elementColor = Storage.GetColor(_mainActivity.Designer.GetIdElementsColor());
            PorterDuffColorFilter elementFilter = new PorterDuffColorFilter(elementColor, PorterDuff.Mode.SrcAtop);
            Color downElementColor = Storage.GetColor(_mainActivity.Designer.GetIdDownButtonsColor());
            PorterDuffColorFilter downElementFilter = new PorterDuffColorFilter(downElementColor, PorterDuff.Mode.SrcAtop);
            PorterDuffColorFilter redFilter = new PorterDuffColorFilter(Color.Red, PorterDuff.Mode.SrcAtop);

            _backgroundLayout.Background.SetColorFilter(textFilter);
            _mainLayout.Background.SetColorFilter(mainFilter);

            _imageCalendar.Background.SetColorFilter(downElementFilter);
            _leftCalendar.Background.SetColorFilter(elementFilter);
            _rightCalendar.Background.SetColorFilter(elementFilter);
            _imageSchedule.Background.SetColorFilter(downElementFilter);
            _leftSchedule.Background.SetColorFilter(elementFilter);
            _rightSchedule.Background.SetColorFilter(elementFilter);
            _imageList.Background.SetColorFilter(downElementFilter);
            _leftList.Background.SetColorFilter(elementFilter);
            _rightList.Background.SetColorFilter(elementFilter);
            _imageInbox.Background.SetColorFilter(downElementFilter);
            _leftInbox.Background.SetColorFilter(elementFilter);
            _rightInbox.Background.SetColorFilter(elementFilter);
            _imageAccount.Background.SetColorFilter(downElementFilter);
            _leftAccount.Background.SetColorFilter(elementFilter);
            _rightAccount.Background.SetColorFilter(elementFilter);

            _deleteCalendar.Background.SetColorFilter(
                (_visibility[(int) MainActivity.StartScreen.Calendar] == '0') ? elementFilter : redFilter);
            _deleteSchedule.Background.SetColorFilter(
                (_visibility[(int) MainActivity.StartScreen.Schedule] == '0') ? elementFilter : redFilter);
            _deleteList.Background.SetColorFilter(
                (_visibility[(int) MainActivity.StartScreen.List] == '0') ? elementFilter : redFilter);
            _deleteInbox.Background.SetColorFilter(
                (_visibility[(int) MainActivity.StartScreen.Inbox] == '0') ? elementFilter : redFilter);
        }

        private void SetPositions () => Server.Buttons = new string(_positions) + Sep + new string(_visibility);
    }
}