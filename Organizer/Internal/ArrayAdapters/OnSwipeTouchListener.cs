using Android.Util;
using Android.Views;
using Organizer.Internal.Activity;
using System;

namespace Organizer.Internal.ArrayAdapters
{
    public class OnSwipeTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        private GestureDetector _gestureDetector;
        public OnSwipeTouchListener (MainActivity mainActivity)
        {
            _gestureDetector = new GestureDetector(mainActivity, new GestureListener(mainActivity));
        }

        public bool OnTouch (View v, MotionEvent e) => _gestureDetector.OnTouchEvent(e);
    }

    public class GestureListener : GestureDetector.SimpleOnGestureListener
    {
        private static int _swipeThreshold = 100;
        private static int _swipeThresholdVelocity = 100;
        private readonly MainActivity _mainActivity;

        public GestureListener (MainActivity mainActivity)
        {
            _mainActivity = mainActivity;
        }

        public override bool OnFling (MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            bool result = false;
            try
            {
                float diffY = e2.GetY() - e1.GetY();
                float diffX = e2.GetX() - e1.GetX();
                if (Math.Abs(diffX) > Math.Abs(diffY))
                {
                    if (Math.Abs(diffX) > _swipeThreshold && Math.Abs(velocityX) > _swipeThresholdVelocity)
                    {
                        if (diffX > 0)
                        {
                            _mainActivity.OnSwipe(inLeft: true);
                        }
                        else
                        {
                            _mainActivity.OnSwipe(inLeft: false);
                        }
                        result = true;
                    }
                }
            }
            catch (Exception exception)
            {
                _ = exception.StackTrace;
            }
            return result;

        }
    }
}