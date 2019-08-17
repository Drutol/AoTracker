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
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using AoLibs.Navigation.Android.Navigation;
using AoLibs.Utilities.Android.Views;
using AoTracker.Android.Themes;
using AoTracker.Android.Utils;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Feed;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
using AoTracker.Resources;
using FFImageLoading;
using GalaSoft.MvvmLight.Helpers;
using Java.Lang;

namespace AoTracker.Android.Fragments.Feed
{
    public class FeedPageTabFragment : FragmentBase<FeedTabViewModel>
    {
        private readonly FeedTabEntry _tabEntry;

        public override int LayoutResourceId { get; } = Resource.Layout.page_feed_tab;

        public FeedPageTabFragment(FeedTabEntry tabEntry)
        {
            _tabEntry = tabEntry;
            ViewModel.TabEntry = tabEntry;
        }

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
                .WithGroup<FeedItemViewModel<YahooItem>, FeedItemYahooHolder>(builder =>
                {
                    builder.WithResourceId(LayoutInflater, Resource.Layout.item_feed_yahoo);
                    builder.WithDataTemplate(FeedItemYahooDataTemplate);
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
                var changedDiff = TimeDiffToString(diff);

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
            CommonFeedItemTemplate(item, holder);

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
        }

        private void FeedItemYahooDataTemplate(FeedItemViewModel<YahooItem> item, FeedItemYahooHolder holder, int position)
        {
            CommonFeedItemTemplate(item, holder);

            holder.Title.Text = item.Item.Name;
            holder.DetailBids.SetText(GetYahooItemLabel("Bids:", item.Item.BidsCount.ToString()),
                TextView.BufferType.Spannable);
            holder.DetailEndsIn.SetText(GetYahooItemLabel("Ends in:", TimeDiffToString((DateTime.UtcNow - item.Item.EndTime).Duration())),
                TextView.BufferType.Spannable);
            holder.DetailCondition.SetText(GetYahooItemLabel("Condition:", item.Item.Condition.ToString()),
                TextView.BufferType.Spannable);

            if (item.Item.Tax == 0)
            {
                holder.DetailsTax.Visibility = ViewStates.Gone;
            }
            else
            {
                holder.DetailsTax.Visibility = ViewStates.Visible;
                holder.DetailsTax.SetText(GetYahooItemLabel("Tax:", $"+{item.Item.Tax}%"),
                    TextView.BufferType.Spannable);
            }

            holder.DetailShipping.Visibility = BindingConverters.BoolToVisibility(item.Item.IsShippingFree);
        }



        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }

        private ICharSequence GetYahooItemLabel(string note, string value)
        {
            var spannable = new SpannableString($"{note} {value}");

            spannable.SetSpan(
                new TypefaceSpan(
                    Activity.Resources.GetString(Resource.String.font_family_light)),
                0,
                note.Length,
                SpanTypes.ExclusiveInclusive);
            spannable.SetSpan(
                new TypefaceSpan(
                    Activity.Resources.GetString(Resource.String.font_family_medium)),
                note.Length,
                note.Length + value.Length,
                SpanTypes.ExclusiveInclusive);

            return spannable;
        }

        private static string TimeDiffToString(TimeSpan diff)
        {
            var changedDiff = string.Empty;
            if (diff.TotalDays > 1)
                changedDiff += $"{diff.Days}d ";
            if (diff.TotalHours > 1)
                changedDiff += $"{diff.Hours}h ";
            changedDiff += $"{diff.Minutes}m";
            return changedDiff;
        }

        private static void CommonFeedItemTemplate(FeedItemViewModel item, IFeedItemHolder holder)
        {
            holder.Price.Text = item.BackingModel.Price + "¥";
            holder.NewAlertSection.Visibility = BindingConverters.BoolToVisibility(item.IsNew);
            ImageService.Instance.LoadUrl(item.BackingModel.ImageUrl).Into(holder.ImageLeft);

            switch (item.PriceChange)
            {
                case PriceChange.Stale:
                    holder.PriceTrendIcon.Visibility = ViewStates.Gone;
                    holder.PriceSubtitle.Visibility = ViewStates.Gone;
                    break;
                case PriceChange.Decrease:
                    holder.PriceSubtitle.Text = $"({item.PriceDifference:N0}¥)";
                    holder.PriceTrendIcon.Visibility = ViewStates.Visible;
                    holder.PriceSubtitle.Visibility = ViewStates.Visible;
                    holder.PriceTrendIcon.SetImageResource(Resource.Drawable.icon_chevron_triple_down);
                    holder.PriceTrendIcon.ImageTintList = ColorStateList.ValueOf(ThemeManager.LimeColour);
                    break;
                case PriceChange.Increase:
                    holder.PriceSubtitle.Text = $"(+{item.PriceDifference:N0}¥)";
                    holder.PriceTrendIcon.Visibility = ViewStates.Visible;
                    holder.PriceSubtitle.Visibility = ViewStates.Visible;
                    holder.PriceTrendIcon.SetImageResource(Resource.Drawable.icon_chevron_triple_up);
                    holder.PriceTrendIcon.ImageTintList = ColorStateList.ValueOf(ThemeManager.RedColour);
                    break;
            }
        }

        interface IFeedItemHolder
        {
            ImageView PriceTrendIcon { get; }
            ImageView ImageLeft { get; }
            FloatingActionButton NewAlertSection { get; }
            TextView Price { get; }
            TextView PriceSubtitle { get; }
        }

        #region Views

        private RecyclerView _recyclerView;
        private ScrollableSwipeToRefreshLayout _swipeToRefreshLayout;
        public RecyclerView RecyclerView => _recyclerView ?? (_recyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView));
        public ScrollableSwipeToRefreshLayout SwipeToRefreshLayout => _swipeToRefreshLayout ?? (_swipeToRefreshLayout = FindViewById<ScrollableSwipeToRefreshLayout>(Resource.Id.SwipeToRefreshLayout));

        #endregion

        class FeedItemHolder : RecyclerView.ViewHolder, IFeedItemHolder
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
            private TextView _priceSubtitle;
            private LinearLayout _clickSurface;

            public ImageView ImageLeft => _imageLeft ?? (_imageLeft = _view.FindViewById<ImageView>(Resource.Id.ImageLeft));
            public FloatingActionButton NewAlertSection => _newAlertSection ?? (_newAlertSection = _view.FindViewById<FloatingActionButton>(Resource.Id.NewAlertSection));
            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));
            public ImageView StoreIcon => _storeIcon ?? (_storeIcon = _view.FindViewById<ImageView>(Resource.Id.StoreIcon));
            public TextView Detail => _detail ?? (_detail = _view.FindViewById<TextView>(Resource.Id.Detail));
            public FrameLayout DetailSection => _detailSection ?? (_detailSection = _view.FindViewById<FrameLayout>(Resource.Id.DetailSection));
            public TextView Subtitle => _subtitle ?? (_subtitle = _view.FindViewById<TextView>(Resource.Id.Subtitle));
            public ImageView PriceTrendIcon => _priceTrendIcon ?? (_priceTrendIcon = _view.FindViewById<ImageView>(Resource.Id.PriceTrendIcon));
            public TextView Price => _price ?? (_price = _view.FindViewById<TextView>(Resource.Id.Price));
            public TextView PriceSubtitle => _priceSubtitle ?? (_priceSubtitle = _view.FindViewById<TextView>(Resource.Id.PriceSubtitle));
            public LinearLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<LinearLayout>(Resource.Id.ClickSurface));
        }

        class FeedItemYahooHolder : RecyclerView.ViewHolder, IFeedItemHolder
        {
            private readonly View _view;

            public FeedItemYahooHolder(View view) : base(view)
            {
                _view = view;
            }
            private ImageView _imageLeft;
            private FloatingActionButton _newAlertSection;
            private TextView _title;
            private ImageView _storeIcon;
            private TextView _detailBids;
            private TextView _detailEndsIn;
            private TextView _detailCondition;
            private LinearLayout _detailSection;
            private TextView _detailShipping;
            private TextView _detailsTax;
            private ImageView _priceTrendIcon;
            private TextView _price;
            private TextView _priceSubtitle;
            private LinearLayout _clickSurface;

            public ImageView ImageLeft => _imageLeft ?? (_imageLeft = _view.FindViewById<ImageView>(Resource.Id.ImageLeft));
            public FloatingActionButton NewAlertSection => _newAlertSection ?? (_newAlertSection = _view.FindViewById<FloatingActionButton>(Resource.Id.NewAlertSection));
            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));
            public ImageView StoreIcon => _storeIcon ?? (_storeIcon = _view.FindViewById<ImageView>(Resource.Id.StoreIcon));
            public TextView DetailBids => _detailBids ?? (_detailBids = _view.FindViewById<TextView>(Resource.Id.DetailBids));
            public TextView DetailEndsIn => _detailEndsIn ?? (_detailEndsIn = _view.FindViewById<TextView>(Resource.Id.DetailEndsIn));
            public TextView DetailCondition => _detailCondition ?? (_detailCondition = _view.FindViewById<TextView>(Resource.Id.DetailCondition));
            public LinearLayout DetailSection => _detailSection ?? (_detailSection = _view.FindViewById<LinearLayout>(Resource.Id.DetailSection));
            public TextView DetailShipping => _detailShipping ?? (_detailShipping = _view.FindViewById<TextView>(Resource.Id.DetailShipping));
            public TextView DetailsTax => _detailsTax ?? (_detailsTax = _view.FindViewById<TextView>(Resource.Id.DetailsTax));
            public ImageView PriceTrendIcon => _priceTrendIcon ?? (_priceTrendIcon = _view.FindViewById<ImageView>(Resource.Id.PriceTrendIcon));
            public TextView Price => _price ?? (_price = _view.FindViewById<TextView>(Resource.Id.Price));
            public TextView PriceSubtitle => _priceSubtitle ?? (_priceSubtitle = _view.FindViewById<TextView>(Resource.Id.PriceSubtitle));
            public LinearLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<LinearLayout>(Resource.Id.ClickSurface));
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