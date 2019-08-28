using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Utilities.Android;
using AoTracker.Android.Themes;
using AoTracker.Android.Utils;
using AoTracker.Android.ViewHolders;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Helpers;

namespace AoTracker.Android.Fragments
{
    [NavigationPage(PageIndex.WatchedItems)]
    public partial class WatchedItemsPageFragment : CustomFragmentBase<WatchedItemsViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_watched_items;

        protected override void InitBindings()
        {
            RecyclerView.SetAdapter(new RecyclerViewAdapterBuilder<WatchedItemViewModel, RecyclerView.ViewHolder>()
                .WithItems(ViewModel.WatchedItems)
                .WithContentStretching()
                .WithMultipleViews()
                .WithGroup<WatchedItemViewModel, WatchedItemHolder>(builder =>
                {
                    builder.WithResourceId(LayoutInflater, Resource.Layout.item_feed);
                    builder.WithDataTemplate(WatchedItemDataTemplate);
                })
                .WithGroup<WatchedItemViewModel<YahooItem>, WatchedItemYahooHolder>(builder =>
                {
                    builder.WithResourceId(LayoutInflater, Resource.Layout.item_feed_yahoo);
                    builder.WithDataTemplate(WatchedItemYahooDataTemplate);
                })
                .Build());

            SwipeToRefreshLayout.ScrollingView = RecyclerView;
            SwipeToRefreshLayout.Refresh += SwipeToRefreshLayoutOnRefresh;
            RecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            ViewModel.WatchedItems.SetUpWithEmptyState(EmptyState);
            var touchHelper = new ItemTouchHelper(new ItemTouchHelperCallback(this));
            touchHelper.AttachToRecyclerView(RecyclerView);
        }

        private void SwipeToRefreshLayoutOnRefresh(object sender, EventArgs e)
        {
            SwipeToRefreshLayout.Refreshing = false;
            ViewModel.RefreshAll();
        }

        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }

        private void WatchedItemYahooDataTemplate(WatchedItemViewModel<YahooItem> item, WatchedItemYahooHolder holder, int position)
        {
            MerchItemYahooHolderTemplate.DataTemplate(item, holder, position);
            holder.ClickSurface.SetOnClickCommand(item.NavigateItemWebsiteCommand);
        }

        private void WatchedItemDataTemplate(WatchedItemViewModel item, WatchedItemHolder holder, int position)
        {
            MerchItemHolderTemplate.DataTemplate(item, holder, position);
            holder.ClickSurface.SetOnClickCommand(item.NavigateItemWebsiteCommand);
            if (item.Item.Domain == CrawlerDomain.Surugaya)
                holder.Title.Text = item.Item.Name;
        }
    }
}