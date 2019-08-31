using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Utilities.Android;
using AoTracker.Android.PagerAdapters;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.ViewModels;

namespace AoTracker.Android.Fragments
{
    [NavigationPage(PageIndex.Welcome, NavigationPageAttribute.PageProvider.Oneshot)]
    public class WelcomePageFragment : CustomFragmentBase<WelcomeViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_welcome;

        protected override void InitBindings()
        {
            ViewPager.Adapter = new WelcomePagerAdapter(ChildFragmentManager, ViewModel.WelcomeTabEntries);
            DotsLayout.SetupWithViewPager(ViewPager);
            ViewPager.PageSelected += ViewPagerOnPageSelected;

            FinishButton.Visibility = ViewStates.Gone;

            FinishButton.SetOnClickCommand(ViewModel.FinishCommand);
            SkipButton.SetOnClickCommand(ViewModel.FinishCommand);
        }

        private void ViewPagerOnPageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if (e.Position == ViewModel.WelcomeTabEntries.Count - 1)
            {
                (DotsLayout.LayoutParameters as FrameLayout.LayoutParams).Gravity = GravityFlags.Start;
                SkipButton.Visibility = ViewStates.Gone;
                FinishButton.Visibility = ViewStates.Visible;
            }
            else
            {
                (DotsLayout.LayoutParameters as FrameLayout.LayoutParams).Gravity = GravityFlags.Center;
                SkipButton.Visibility = ViewStates.Visible;
                FinishButton.Visibility = ViewStates.Gone;
            }
        }

        #region Views

        private ViewPager _viewPager;
        private TabLayout _dotsLayout;
        private Button _skipButton;
        private Button _finishButton;

        public ViewPager ViewPager => _viewPager ?? (_viewPager = FindViewById<ViewPager>(Resource.Id.ViewPager));
        public TabLayout DotsLayout => _dotsLayout ?? (_dotsLayout = FindViewById<TabLayout>(Resource.Id.DotsLayout));
        public Button SkipButton => _skipButton ?? (_skipButton = FindViewById<Button>(Resource.Id.SkipButton));
        public Button FinishButton => _finishButton ?? (_finishButton = FindViewById<Button>(Resource.Id.FinishButton));

        #endregion
    }
}