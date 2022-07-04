using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Organizer.Internal.Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Organizer.Internal.ArrayAdapters
{
    public static class PopupConstructor
    {
        public static PopupMenu GetPopupMenu (MainActivity mainActivity, View anchor, int idGroup, int[] idItems, int[] idTitles)
        {
            int popupStyleRes = Resource.Style.MainPopup;

            switch (mainActivity.Designer.CurrentTheme)
            {
                case Resources.Designer.Theme.Soft:
                    popupStyleRes = Resource.Style.SoftPopup;
                    break;
                case Resources.Designer.Theme.Purple:
                    popupStyleRes = Resource.Style.PurplePopup;
                    break;
                case Resources.Designer.Theme.MainDark:
                    popupStyleRes = Resource.Style.MainDarkPopup;
                    break;
                case Resources.Designer.Theme.DeepWater:
                    popupStyleRes = Resource.Style.DeepWaterPopup;
                    break;
                case Resources.Designer.Theme.DarkPurple:
                    popupStyleRes = Resource.Style.DarkPurplePopup;
                    break;
            }

            PopupMenu popup = new PopupMenu(mainActivity, anchor, GravityFlags.Right, 0, popupStyleRes);

            for (int i = 0; i < idItems.Length; i++)
            {
                popup.Menu.Add(idGroup, idItems[i], i, mainActivity.Translater.GetString(idTitles[i]));
            }

            return popup;
        }
    }
}