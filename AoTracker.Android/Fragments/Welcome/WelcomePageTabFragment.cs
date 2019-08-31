using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoLibs.Navigation.Android.Navigation;
using AoTracker.Android.Utils;
using AoTracker.Domain.Models;

namespace AoTracker.Android.Fragments.Welcome
{
    public class WelcomePageTabFragment : NavigationFragmentBase
    {
        private readonly WelcomeTabEntry _welcomeTabEntry;

        public override int LayoutResourceId { get; } = Resource.Layout.page_welcome_tab;

        public WelcomePageTabFragment(WelcomeTabEntry welcomeTabEntry)
        {
            _welcomeTabEntry = welcomeTabEntry;
        }

        protected override void InitBindings()
        {
            Title.Text = _welcomeTabEntry.Title;
            Subtitle.Text = _welcomeTabEntry.Subtitle;
            Icon.SetImageResource(_welcomeTabEntry.WelcomeStage.ToIconResource());
        }

        #region Views

        private ImageView _icon;
        private TextView _title;
        private TextView _subtitle;

        public ImageView Icon => _icon ?? (_icon = FindViewById<ImageView>(Resource.Id.Icon));
        public TextView Title => _title ?? (_title = FindViewById<TextView>(Resource.Id.Title));
        public TextView Subtitle => _subtitle ?? (_subtitle = FindViewById<TextView>(Resource.Id.Subtitle));

        #endregion
    }
}