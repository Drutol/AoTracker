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
using Android.Util;
using Android.Views;
using Android.Widget;
using AoLibs.Utilities.Android.Views;
using AoTracker.Android.ViewHolders;
using Com.App.Adprogressbarlib;

namespace AoTracker.Android.Fragments.Feed
{
    public partial class FeedPageTabFragment
    {
        #region Views

        private RecyclerView _recyclerView;
        private ScrollableSwipeToRefreshLayout _swipeToRefreshLayout;
        private AdCircleProgress _progressBar;
        private ProgressBar _progressSpinner;
        private TextView _progressLabel;
        private FrameLayout _loadingLayout;
        private FloatingActionButton _manualLoadButton;
        private FrameLayout _manualLoadLayout;

        public RecyclerView RecyclerView => _recyclerView ?? (_recyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView));
        public ScrollableSwipeToRefreshLayout SwipeToRefreshLayout => _swipeToRefreshLayout ?? (_swipeToRefreshLayout = FindViewById<ScrollableSwipeToRefreshLayout>(Resource.Id.SwipeToRefreshLayout));
        public AdCircleProgress ProgressBar => _progressBar ?? (_progressBar = FindViewById<AdCircleProgress>(Resource.Id.ProgressBar));
        public ProgressBar ProgressSpinner => _progressSpinner ?? (_progressSpinner = FindViewById<ProgressBar>(Resource.Id.ProgressSpinner));
        public TextView ProgressLabel => _progressLabel ?? (_progressLabel = FindViewById<TextView>(Resource.Id.ProgressLabel));
        public FrameLayout LoadingLayout => _loadingLayout ?? (_loadingLayout = FindViewById<FrameLayout>(Resource.Id.LoadingLayout));
        public FloatingActionButton ManualLoadButton => _manualLoadButton ?? (_manualLoadButton = FindViewById<FloatingActionButton>(Resource.Id.ManualLoadButton));
        public FrameLayout ManualLoadLayout => _manualLoadLayout ?? (_manualLoadLayout = FindViewById<FrameLayout>(Resource.Id.ManualLoadLayout));

        #endregion

        class FeedItemHolder : RecyclerView.ViewHolder, IMerchItemHolderGeneral
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
            private LinearLayout _priceSection;

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
            public LinearLayout PriceSection => _priceSection ?? (_priceSection = _view.FindViewById<LinearLayout>(Resource.Id.PriceSection));
        }

        class FeedItemYahooHolder : RecyclerView.ViewHolder, IMerchItemHolderYahoo
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

        class ScrollListener : RecyclerView.OnScrollListener
        {
            private readonly FeedPageTabFragment _parent;

            private bool? _lastRequest;

            public ScrollListener(FeedPageTabFragment parent)
            {
                _parent = parent;
            }

            public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
            {
                bool request = !(dy > 0);

                if (_lastRequest != request)
                {
                    _parent.ViewModel.RequestJumpToFabVisibilityChangeCommand.Execute(request);
                    _lastRequest = request;
                }

                base.OnScrolled(recyclerView, dx, dy);
            }
        }
    }
}