using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoLibs.Navigation.Android.Navigation;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Utilities.Android.Views;
using AoTracker.Android.Themes;
using AoTracker.Android.Utils;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
using AoTracker.Resources;
using FFImageLoading;
using GalaSoft.MvvmLight.Helpers;

namespace AoTracker.Android.Fragments
{
    [NavigationPage(PageIndex.Feed)]
    public class FeedPageFragment : CustomFragmentBase<FeedViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_feed;

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.IsLoading).WhenSourceChanges(() =>
            {
                SwipeToRefreshLayout.Refreshing = ViewModel.IsLoading;
            }));

            RecyclerView.SetAdapter(new RecyclerViewAdapterBuilder<IFeedItem, RecyclerView.ViewHolder>()
                .WithItems(ViewModel.Feed)
                .WithContentStretching()
                .WithMultipleViews()
                .WithGroup<FeedItemViewModel, FeedItemHolder>(builder =>
                {
                    builder.WithResourceId(LayoutInflater, Resource.Layout.item_feed);
                    builder.WithDataTemplate(FeedItemDataTemplate);
                })            
                .WithGroup<FeedChangeGroupItem, FeedChangeGroupHolder>(builder =>
                {
                    builder.WithResourceId(LayoutInflater, Resource.Layout.item_feed_change_time_group);
                    builder.WithDataTemplate(FeedChangeGroupDataTemplate);
                })
                .Build());

            SwipeToRefreshLayout.ScrollingView = RecyclerView;
            SwipeToRefreshLayout.Refresh += SwipeToRefreshLayoutOnRefresh;


            RecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
        }

        private void FeedChangeGroupDataTemplate(FeedChangeGroupItem item, FeedChangeGroupHolder holder, int position)
        {
            var diff = DateTime.UtcNow - item.LastChanged;
            if (diff > TimeSpan.FromMinutes(10))
            {
                var changedDiff = string.Empty;
                if (diff.TotalDays > 1)
                    changedDiff += $"{diff.Days}d ";
                if (diff.TotalHours > 1)
                    changedDiff += $"{diff.Hours}h ";
                changedDiff += $"{diff.Minutes}m";

                holder.Label.Text = string.Format(AppResources.Item_Feed_LastChanged, changedDiff);
            }
            else
            {
                holder.Label.Text = AppResources.Item_FeedChangeGroup_Recently;
            }
        }

        private void SwipeToRefreshLayoutOnRefresh(object sender, EventArgs e)
        {
            ViewModel.RefreshFeed(true);
        }

        private void FeedItemDataTemplate(FeedItemViewModel item, FeedItemHolder holder, int position)
        {
            if (item.BackingModel is SurugayaItem surugayaItem)
            {
                holder.Title.Text = surugayaItem.Category;
                holder.Detail.Text = surugayaItem.Name;
                holder.Detail.Visibility = ViewStates.Visible;
                holder.Subtitle.Text = surugayaItem.Brand;
                holder.StoreIcon.SetImageResource(Resource.Drawable.surugaya);
            }

            if (item.BackingModel is MandarakeItem mandarakeItem)
            {
                holder.Title.Text = mandarakeItem.Name;
                holder.Detail.Visibility = ViewStates.Gone;
                holder.Subtitle.Text = mandarakeItem.Shop;
                holder.StoreIcon.SetImageResource(Resource.Drawable.mandarake);
            }


            holder.Price.Text = item.BackingModel.Price + "¥";
            holder.NewAlertSection.Visibility = BindingConverters.BoolToVisibility(item.IsNew);

            switch (item.PriceChange)
            {
                case PriceChange.Stale:
                    holder.PriceTrendIcon.Visibility = ViewStates.Gone;
                    break;
                case PriceChange.Decrease:
                    holder.PriceTrendIcon.Visibility = ViewStates.Visible;
                    holder.PriceTrendIcon.SetImageResource(Resource.Drawable.icon_chevron_triple_down);
                    holder.PriceTrendIcon.ImageTintList = ColorStateList.ValueOf(ThemeManager.LimeColour);
                    break;
                case PriceChange.Increase:
                    holder.PriceTrendIcon.Visibility = ViewStates.Visible;
                    holder.PriceTrendIcon.SetImageResource(Resource.Drawable.icon_chevron_triple_up);
                    holder.PriceTrendIcon.ImageTintList = ColorStateList.ValueOf(ThemeManager.RedColour);
                    break;
            }

            ImageService.Instance.LoadUrl(item.BackingModel.ImageUrl).Into(holder.ImageLeft);
        }

        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }

        #region Views

        private RecyclerView _recyclerView;
        private ScrollableSwipeToRefreshLayout _swipeToRefreshLayout;

        public RecyclerView RecyclerView => _recyclerView ?? (_recyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView));
        public ScrollableSwipeToRefreshLayout SwipeToRefreshLayout => _swipeToRefreshLayout ?? (_swipeToRefreshLayout = FindViewById<ScrollableSwipeToRefreshLayout>(Resource.Id.SwipeToRefreshLayout));

        #endregion

        class FeedItemHolder : RecyclerView.ViewHolder
        {
            private readonly View _view;

            public FeedItemHolder(View view) : base(view)
            {
                _view = view;
            }
            private ImageView _imageLeft;
            private FloatingActionButton _newAlertSection;
            private TextView _title;
            private ImageView _storeIcon;
            private TextView _detail;
            private FrameLayout _detailSection;
            private TextView _subtitle;
            private ImageView _priceTrendIcon;
            private TextView _price;
            private LinearLayout _clickSurface;
            private TextView _lastUpdatedLabel;

            public ImageView ImageLeft => _imageLeft ?? (_imageLeft = _view.FindViewById<ImageView>(Resource.Id.ImageLeft));
            public FloatingActionButton NewAlertSection => _newAlertSection ?? (_newAlertSection = _view.FindViewById<FloatingActionButton>(Resource.Id.NewAlertSection));
            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));
            public ImageView StoreIcon => _storeIcon ?? (_storeIcon = _view.FindViewById<ImageView>(Resource.Id.StoreIcon));
            public TextView Detail => _detail ?? (_detail = _view.FindViewById<TextView>(Resource.Id.Detail));
            public FrameLayout DetailSection => _detailSection ?? (_detailSection = _view.FindViewById<FrameLayout>(Resource.Id.DetailSection));
            public TextView Subtitle => _subtitle ?? (_subtitle = _view.FindViewById<TextView>(Resource.Id.Subtitle));
            public ImageView PriceTrendIcon => _priceTrendIcon ?? (_priceTrendIcon = _view.FindViewById<ImageView>(Resource.Id.PriceTrendIcon));
            public TextView Price => _price ?? (_price = _view.FindViewById<TextView>(Resource.Id.Price));
            public LinearLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<LinearLayout>(Resource.Id.ClickSurface));
            public TextView LastUpdatedLabel => _lastUpdatedLabel ?? (_lastUpdatedLabel = _view.FindViewById<TextView>(Resource.Id.LastUpdatedLabel));
        }


        class FeedChangeGroupHolder : RecyclerView.ViewHolder
        {
            private readonly View _view;

            public FeedChangeGroupHolder(View view) : base(view)
            {
                _view = view;
            }
            private TextView _label;

            public TextView Label => _label ?? (_label = _view.FindViewById<TextView>(Resource.Id.Label));
        }



    }
}