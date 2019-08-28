using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoLibs.Utilities.Android.Views;
using AoTracker.Android.Themes;
using AoTracker.Android.Utils;
using AoTracker.Android.ViewHolders;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Helpers;

namespace AoTracker.Android.Fragments
{
    public partial class WatchedItemsPageFragment
    {
        #region Views

        private RecyclerView _recyclerView;
        private ScrollableSwipeToRefreshLayout _swipeToRefreshLayout;
        private ImageView _emptyStateIcon;
        private LinearLayout _emptyState;

        public RecyclerView RecyclerView => _recyclerView ?? (_recyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView));
        public ScrollableSwipeToRefreshLayout SwipeToRefreshLayout => _swipeToRefreshLayout ?? (_swipeToRefreshLayout = FindViewById<ScrollableSwipeToRefreshLayout>(Resource.Id.SwipeToRefreshLayout));
        public ImageView EmptyStateIcon => _emptyStateIcon ?? (_emptyStateIcon = FindViewById<ImageView>(Resource.Id.EmptyStateIcon));
        public LinearLayout EmptyState => _emptyState ?? (_emptyState = FindViewById<LinearLayout>(Resource.Id.EmptyState));

        #endregion

        class ItemTouchHelperCallback : ItemTouchHelper.Callback
        {
            private readonly WatchedItemsPageFragment _parent;

            public ItemTouchHelperCallback(WatchedItemsPageFragment parent)
            {
                _parent = parent;
            }

            public override bool IsItemViewSwipeEnabled { get; } = true;

            public override bool IsLongPressDragEnabled { get; } = false;

            public override bool CanDropOver(RecyclerView recyclerView, RecyclerView.ViewHolder current, RecyclerView.ViewHolder target)
            {
                return true;
            }

            public override int GetMovementFlags(RecyclerView p0, RecyclerView.ViewHolder p1)
            {
                int dragFlags = ItemTouchHelper.End | ItemTouchHelper.Start;
                return MakeMovementFlags(0, dragFlags);
            }

            public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
            {
                return false;
            }

            public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
            {
                if (direction == ItemTouchHelper.Start)
                {
                    _parent.ViewModel.ReloadWatchedItemCommand.Execute((viewHolder as BindingViewHolderBase<WatchedItemViewModel>).ViewModel);
                    _parent.RecyclerView.GetAdapter().NotifyItemChanged(viewHolder.AdapterPosition);
                }
                else if (direction == ItemTouchHelper.End)
                {
                    _parent.ViewModel.RemoveWatchedItemCommand.Execute((viewHolder as BindingViewHolderBase<WatchedItemViewModel>).ViewModel);
                }
            }

            public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState,
                bool isCurrentlyActive)
            {
                new RecyclerViewSwipeDecorator.Builder(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive)
                    .AddSwipeRightBackgroundColor(ThemeManager.RedColour)
                    .AddSwipeRightActionIcon(Resource.Drawable.icon_delete)
                    .SetSwipeRightActionIconTint(Color.White)    
                    .AddSwipeLeftBackgroundColor(ThemeManager.AccentColour)
                    .AddSwipeLeftActionIcon(Resource.Drawable.icon_reload)
                    .SetSwipeLeftActionIconTint(Color.White)
                    .Create()
                    .Decorate();

                base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
            }
        }

        class WatchedItemHolder : BindingViewHolderBase<WatchedItemViewModel>, IMerchItemHolderGeneral
        {
            private readonly View _view;

            public WatchedItemHolder(View view) : base(view)
            {
                _view = view;
            }

            protected override void SetBindings()
            {
                Bindings.Add(this.SetBinding(() => ViewModel.IsLoading).WhenSourceChanges(() =>
                {
                    LoadingSpinner.Visibility = BindingConverters.BoolToVisibility(ViewModel.IsLoading, ViewStates.Invisible);
                    DetailSection.Visibility = BottomDetailSection.Visibility =
                        BindingConverters.BoolToVisibilityInverted(ViewModel.IsLoading, ViewStates.Invisible);

                    if (!ViewModel.IsLoading)
                    {
                        MerchItemHolderTemplate.DataTemplate(ViewModel, this, AdapterPosition);
                        if (Math.Abs(ViewModel.Price - CrawlerConstants.InvalidPrice) < 0.0001)
                        {
                            Price.Text = AppResources.Item_WatchedItem_SoldOut;
                            Price.SetTextColor(ThemeManager.RedColour);
                        }
                    }
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
            private LinearLayout _bottomDetailSection;
            private LinearLayout _detailSectionContainer;
            private ProgressBar _loadingSpinner;
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
            public LinearLayout BottomDetailSection => _bottomDetailSection ?? (_bottomDetailSection = _view.FindViewById<LinearLayout>(Resource.Id.BottomDetailSection));
            public LinearLayout DetailSectionContainer => _detailSectionContainer ?? (_detailSectionContainer = _view.FindViewById<LinearLayout>(Resource.Id.DetailSectionContainer));
            public ProgressBar LoadingSpinner => _loadingSpinner ?? (_loadingSpinner = _view.FindViewById<ProgressBar>(Resource.Id.LoadingSpinner));
            public LinearLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<LinearLayout>(Resource.Id.ClickSurface));
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
                Bindings.Add(this.SetBinding(() => ViewModel.IsLoading).WhenSourceChanges(() =>
                {
                    LoadingSpinner.Visibility = BindingConverters.BoolToVisibility(ViewModel.IsLoading, ViewStates.Invisible);
                    DetailSection.Visibility = BindingConverters.BoolToVisibilityInverted(ViewModel.IsLoading, ViewStates.Invisible);

                    if (!ViewModel.IsLoading)
                    {
                        MerchItemYahooHolderTemplate.DataTemplate(ViewModel, this, AdapterPosition);
                        if (Math.Abs(ViewModel.Price - CrawlerConstants.InvalidPrice) < 0.0001)
                        {
                            Price.Text = AppResources.Item_WatchedItem_AuctionEnded;
                            Price.SetTextColor(ThemeManager.RedColour);
                        }
                    }
                }));
            }

            private ImageView _imageLeft;
            private FloatingActionButton _newAlertSection;
            private TextView _title;
            private ImageView _storeIcon;
            private TextView _detailBids;
            private TextView _detailEndsIn;
            private TextView _detailCondition;
            private TextView _detailShipping;
            private TextView _detailsTax;
            private ImageView _priceTrendIcon;
            private TextView _price;
            private TextView _priceSubtitle;
            private LinearLayout _priceSection;
            private FrameLayout _detailSection;
            private ProgressBar _loadingSpinner;
            private LinearLayout _clickSurface;

            public ImageView ImageLeft => _imageLeft ?? (_imageLeft = _view.FindViewById<ImageView>(Resource.Id.ImageLeft));
            public FloatingActionButton NewAlertSection => _newAlertSection ?? (_newAlertSection = _view.FindViewById<FloatingActionButton>(Resource.Id.NewAlertSection));
            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));
            public ImageView StoreIcon => _storeIcon ?? (_storeIcon = _view.FindViewById<ImageView>(Resource.Id.StoreIcon));
            public TextView DetailBids => _detailBids ?? (_detailBids = _view.FindViewById<TextView>(Resource.Id.DetailBids));
            public TextView DetailEndsIn => _detailEndsIn ?? (_detailEndsIn = _view.FindViewById<TextView>(Resource.Id.DetailEndsIn));
            public TextView DetailCondition => _detailCondition ?? (_detailCondition = _view.FindViewById<TextView>(Resource.Id.DetailCondition));
            public TextView DetailShipping => _detailShipping ?? (_detailShipping = _view.FindViewById<TextView>(Resource.Id.DetailShipping));
            public TextView DetailsTax => _detailsTax ?? (_detailsTax = _view.FindViewById<TextView>(Resource.Id.DetailsTax));
            public ImageView PriceTrendIcon => _priceTrendIcon ?? (_priceTrendIcon = _view.FindViewById<ImageView>(Resource.Id.PriceTrendIcon));
            public TextView Price => _price ?? (_price = _view.FindViewById<TextView>(Resource.Id.Price));
            public TextView PriceSubtitle => _priceSubtitle ?? (_priceSubtitle = _view.FindViewById<TextView>(Resource.Id.PriceSubtitle));
            public LinearLayout PriceSection => _priceSection ?? (_priceSection = _view.FindViewById<LinearLayout>(Resource.Id.PriceSection));
            public FrameLayout DetailSection => _detailSection ?? (_detailSection = _view.FindViewById<FrameLayout>(Resource.Id.DetailSection));
            public ProgressBar LoadingSpinner => _loadingSpinner ?? (_loadingSpinner = _view.FindViewById<ProgressBar>(Resource.Id.LoadingSpinner));
            public LinearLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<LinearLayout>(Resource.Id.ClickSurface));
        }
    }
}