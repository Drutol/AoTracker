using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.View.Menu;
using Android.Views;
using Android.Widget;

namespace AoTracker.Android.Utils
{
    public class MenuCallback : Java.Lang.Object, MenuBuilder.ICallback
    {
        private readonly Action<MenuBuilder, IMenuItem> _action;

        public MenuCallback(Action<MenuBuilder, IMenuItem> action)
        {
            _action = action;
        }

        public bool OnMenuItemSelected(MenuBuilder p0, IMenuItem p1)
        {
            _action(p0, p1);
            return true;
        }

        public void OnMenuModeChange(MenuBuilder p0)
        {

        }
    }
}