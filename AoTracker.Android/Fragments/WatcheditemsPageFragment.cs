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
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Utilities.Android;
using AoTracker.Android.Utils;
using AoTracker.Android.ViewHolders;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
using GalaSoft.MvvmLight.Helpers;

namespace AoTracker.Android.Fragments
{
    [NavigationPage(PageIndex.WatchedItems)]
    public class WatchedItemsPageFragment : CustomFragmentBase<WatchedItemsViewModel>
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

            RecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
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
        }

        #region Views

        private RecyclerView _recyclerView;

        public RecyclerView RecyclerView => _recyclerView ?? (_recyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView));

        #endregion

        class WatchedItemHolder : BindingViewHolderBase<WatchedItemViewModel>, IMerchItemHolderGeneral
        {
            private readonly View _view;

            public WatchedItemHolder(View view) : base(view)
            {
                _view = view;
            }

            protected override void SetBindings()
            {
                Bindings.Add(this.SetBinding(() => ViewModel.Price).WhenSourceChanges(() =>
                {
                    PriceSection.Visibility = 
                        Math.Abs(ViewModel.Price - CrawlerConstants.InvalidPrice) < 0.0001 
                        ? ViewStates.Gone 
                        : ViewStates.Visible;
                }));
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
            private LinearLayout _priceSection;
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
            public LinearLayout PriceSection => _priceSection ?? (_priceSection = _view.FindViewById<LinearLayout>(Resource.Id.PriceSection));
            public LinearLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<LinearLayout>(Resource.Id.ClickSurface));
        }

    }

    class WatchedItemYahooHolder : BindingViewHolderBase<WatchedItemViewModel<YahooItem>>, IMerchItemHolderYahoo
    {
        private readonly View _view;

        public WatchedItemYahooHolder(View view) : base(view)
        {
            _view = view;
        }

        protected override void SetBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Price).WhenSourceChanges(() =>
            {
                PriceSection.Visibility =
                    Math.Abs(ViewModel.Price - CrawlerConstants.InvalidPrice) < 0.0001
                        ? ViewStates.Gone
                        : ViewStates.Visible;
            }));
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
        private LinearLayout _priceSection;

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
        public LinearLayout PriceSection => _priceSection ?? (_priceSection = _view.FindViewById<LinearLayout>(Resource.Id.PriceSection));
    }
}