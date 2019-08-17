using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Google.Android.Flexbox;

namespace AoTracker.Android.Utils
{
    public class CustomFlexboxLayoutManager : FlexboxLayoutManager
    {
        protected CustomFlexboxLayoutManager(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public CustomFlexboxLayoutManager(Context p0, IAttributeSet p1, int p2, int p3) : base(p0, p1, p2, p3)
        {
        }

        public CustomFlexboxLayoutManager(Context p0, int p1, int p2) : base(p0, p1, p2)
        {
        }

        public CustomFlexboxLayoutManager(Context p0, int p1) : base(p0, p1)
        {
        }

        public CustomFlexboxLayoutManager(Context p0) : base(p0)
        {
        }

        public override RecyclerView.LayoutParams GenerateLayoutParams(ViewGroup.LayoutParams lp)
        {
            return new LayoutParams(lp);
        }
    }
}