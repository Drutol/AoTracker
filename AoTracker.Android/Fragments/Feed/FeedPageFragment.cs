using Android.Support.Design.Widget;
using Android.Support.V4.View;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoTracker.Android.PagerAdapters;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.ViewModels.Feed;
using GalaSoft.MvvmLight.Helpers;

namespace AoTracker.Android.Fragments.Feed
{
    [NavigationPage(PageIndex.Feed)]
    public class FeedPageFragment : CustomFragmentBase<FeedViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_feed;

        private FeedPagerAdapter _adapter;

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.FeedTabEntries).WhenSourceChanges(() =>
            {
                if(ViewModel.FeedTabEntries is null)
                    return;

                _adapter = new FeedPagerAdapter(ChildFragmentManager, ViewModel.FeedTabEntries);
                ViewPager.Adapter = _adapter;
            }));

            TabStrip.SetupWithViewPager(ViewPager);
            ViewPager.PageSelected += ViewPagerOnPageSelected;
        }

        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }

        private void ViewPagerOnPageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            _adapter.Fragments[e.Position].NavigatedTo();
        }

        #region Views

        private TabLayout _tabStrip;
        private ViewPager _viewPager;

        public TabLayout TabStrip => _tabStrip ?? (_tabStrip = FindViewById<TabLayout>(Resource.Id.TabStrip));
        public ViewPager ViewPager => _viewPager ?? (_viewPager = FindViewById<ViewPager>(Resource.Id.ViewPager));

        #endregion
    }
}