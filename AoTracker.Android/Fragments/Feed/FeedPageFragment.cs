using System;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.View.Menu;
using Android.Util;
using Android.Views;
using Android.Widget;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Utilities.Android.Listeners;
using AoTracker.Android.PagerAdapters;
using AoTracker.Android.Utils;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.ViewModels.Feed;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Helpers;

namespace AoTracker.Android.Fragments.Feed
{
    [NavigationPage(PageIndex.Feed)]
    public class FeedPageFragment : CustomFragmentBase<FeedViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_feed;

        private FeedPagerAdapter _adapter;
        private FeedPageTabFragment _lastSelectedFragment;

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.FeedTabEntries).WhenSourceChanges(() =>
            {
                if(ViewModel.FeedTabEntries is null)
                    return;

                _adapter = new FeedPagerAdapter(ChildFragmentManager, ViewModel.FeedTabEntries, this);
                ViewPager.Adapter = _adapter;
                UpdateTabIcons();
            }));

            Bindings.Add(this.SetBinding(() => ViewModel.JumpToButtonVisibility).WhenSourceChanges(() =>
            {
                if (ViewModel.JumpToButtonVisibility)
                {
                    JumpToButton.Show();
                }
                else
                {
                    JumpToButton.Hide();
                }
            }));

            JumpToButton.SetOnClickListener(new OnClickListener(HandleJumpToPopup));
            TabStrip.SetupWithViewPager(ViewPager);
            ViewPager.PageSelected += ViewPagerOnPageSelected;
        }

        private void HandleJumpToPopup(View obj)
        {
            var menuBuilder = new MenuBuilder(Activity);

            int i = 0;
            int j = ViewModel.ContainsAggregate ? 0 : 1;
            foreach (var tabEntry in ViewModel.FeedTabEntries)
            {
                menuBuilder.Add(0, i++, 0, tabEntry.Name).SetIcon(Util.IndexToIconResource(j++));
            }

            menuBuilder.SetCallback(new MenuCallback((sender, menuItem) =>
            {
                ViewPager.SetCurrentItem(menuItem.ItemId, true);
            }));

            var menuPopupHelper = new MenuPopupHelper(Context, menuBuilder);
            menuPopupHelper.SetAnchorView(JumpToButton);
            menuPopupHelper.SetForceShowIcon(true);
            menuPopupHelper.Show();
        }

        public void UpdateTabIcons()
        {
            EmptyState.Visibility = BindingConverters.BoolToVisibility(ViewModel.FeedTabEntries.Count == 0);

            if (ViewModel.FeedTabEntries.Count <= 1)
            {
                TabStrip.Visibility = ViewStates.Gone;
                return;
            }

            TabStrip.Visibility = ViewStates.Visible;

            if (ViewModel.ContainsAggregate)
            {
                for (int i = 0; i < TabStrip.TabCount; i++)
                {
                    TabStrip.GetTabAt(i).SetIcon(Util.IndexToIconResource(i));
                }
            }
            else
            {
                for (int i = 1; i < TabStrip.TabCount + 1; i++)
                {
                    TabStrip.GetTabAt(i - 1).SetIcon(Util.IndexToIconResource(i));
                }
            }
        }

        public override void NavigatedTo()
        {
            // Why? Given usage ChildFragmentManager not to pollute activity's FragmentManager we have to
            // apply new ChildFragmentManager when navigating or else the old one would be retained while it has
            // been already detached by Android. Hence code below essentially recreates Pager state with never manager.
            if (_adapter != null)
            {
                var selectedPage = 
                    _lastSelectedFragment != null && _adapter.Fragments.Contains(_lastSelectedFragment)
                    ? _adapter.Fragments.IndexOf(_lastSelectedFragment)
                    : 0;
                _adapter = _adapter.Duplicate(ChildFragmentManager);
                ViewPager.Adapter = _adapter;
                ViewPager.SetCurrentItem(selectedPage, false);
                UpdateTabIcons();
            }

            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }

        public override void NavigatedBack()
        {
            base.NavigatedBack();
            ViewModel.NavigatedBack();
        }

        public override void NavigatedFrom()
        {
            base.NavigatedFrom();
            ViewModel.NavigatedFrom();
        }

        private void ViewPagerOnPageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            _lastSelectedFragment = _adapter.Fragments[e.Position];
            _lastSelectedFragment.NavigatedTo();
        }

        #region Views

        private TabLayout _tabStrip;
        private ViewPager _viewPager;
        private ImageView _emptyStateIcon;
        private LinearLayout _emptyState;
        private FloatingActionButton _jumpToButton;

        public TabLayout TabStrip => _tabStrip ?? (_tabStrip = FindViewById<TabLayout>(Resource.Id.TabStrip));
        public ViewPager ViewPager => _viewPager ?? (_viewPager = FindViewById<ViewPager>(Resource.Id.ViewPager));
        public ImageView EmptyStateIcon => _emptyStateIcon ?? (_emptyStateIcon = FindViewById<ImageView>(Resource.Id.EmptyStateIcon));
        public LinearLayout EmptyState => _emptyState ?? (_emptyState = FindViewById<LinearLayout>(Resource.Id.EmptyState));
        public FloatingActionButton JumpToButton => _jumpToButton ?? (_jumpToButton = FindViewById<FloatingActionButton>(Resource.Id.JumpToButton));

        #endregion
    }
}