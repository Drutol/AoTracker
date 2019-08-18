using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AoTracker.Android.Views
{
    public class ClickInterceptingContainer : FrameLayout
    {
        protected ClickInterceptingContainer(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ClickInterceptingContainer(Context context) : base(context)
        {
        }

        public ClickInterceptingContainer(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public ClickInterceptingContainer(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public ClickInterceptingContainer(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return true;
        }
    }
}