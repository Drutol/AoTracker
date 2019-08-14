using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoLibs.Navigation.Android.Navigation;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoTracker.Android.Utils;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
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
            Bindings.Add(
                this.SetBinding(() => ViewModel.IsLoading,
                    () => ProgressSpinner.Visibility).ConvertSourceToTarget(BindingConverters.BoolToVisibility));

            RecyclerView.SetAdapter(new RecyclerViewAdapterBuilder<IFeedItem, RecyclerView.ViewHolder>()
                .WithItems(ViewModel.Feed)
                .WithContentStretching()
                .WithMultipleViews()
                .WithGroup<FeedItemViewModel, FeedItemHolder>(builder =>
                {
                    builder.WithResourceId(LayoutInflater, Resource.Layout.item_feed);
                    builder.WithDataTemplate(FeedItemDataTemplate);
                })
                .Build());


            RecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
        }

        private void FeedItemDataTemplate(FeedItemViewModel item, FeedItemHolder holder, int position)
        {
            if (item.BackingModel is SurugayaItem surugayaItem)
            {
                holder.Title.Text = surugayaItem.Category;
                holder.Detail.Text = surugayaItem.Name;
            }
 
            holder.Price.Text = item.BackingModel.Price.ToString();
            ImageService.Instance.LoadUrl(item.BackingModel.ImageUrl).Into(holder.ImageLeft);
        }

        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }

        #region Views

        private RecyclerView _recyclerView;
        private ProgressBar _progressSpinner;

        public RecyclerView RecyclerView => _recyclerView ?? (_recyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView));
        public ProgressBar ProgressSpinner => _progressSpinner ?? (_progressSpinner = FindViewById<ProgressBar>(Resource.Id.ProgressSpinner));

        #endregion

        class FeedItemHolder : RecyclerView.ViewHolder
        {
            private readonly View _view;

            public FeedItemHolder(View view) : base(view)
            {
                _view = view;
            }
            private ImageView _imageLeft;
            private FrameLayout _newAlertSection;
            private TextView _title;
            private ImageView _storeIcon;
            private TextView _detail;
            private TextView _subtitle;
            private ImageView _priceTrendIcon;
            private TextView _price;
            private LinearLayout _clickSurface;

            public ImageView ImageLeft => _imageLeft ?? (_imageLeft = _view.FindViewById<ImageView>(Resource.Id.ImageLeft));
            public FrameLayout NewAlertSection => _newAlertSection ?? (_newAlertSection = _view.FindViewById<FrameLayout>(Resource.Id.NewAlertSection));
            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));
            public ImageView StoreIcon => _storeIcon ?? (_storeIcon = _view.FindViewById<ImageView>(Resource.Id.StoreIcon));
            public TextView Detail => _detail ?? (_detail = _view.FindViewById<TextView>(Resource.Id.Detail));
            public TextView Subtitle => _subtitle ?? (_subtitle = _view.FindViewById<TextView>(Resource.Id.Subtitle));
            public ImageView PriceTrendIcon => _priceTrendIcon ?? (_priceTrendIcon = _view.FindViewById<ImageView>(Resource.Id.PriceTrendIcon));
            public TextView Price => _price ?? (_price = _view.FindViewById<TextView>(Resource.Id.Price));
            public LinearLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<LinearLayout>(Resource.Id.ClickSurface));
        }

    }
}