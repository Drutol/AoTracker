﻿using System;
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
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.ViewModels;

namespace AoTracker.Android.Fragments
{
    [NavigationPage(PageIndex.Feed)]
    public class FeedPageFragment : CustomFragmentBase<FeedViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_feed;

        protected override void InitBindings()
        {

        }

        public override void NavigatedTo()
        {
            ViewModel.NavigatedTo();
        }
    }
}