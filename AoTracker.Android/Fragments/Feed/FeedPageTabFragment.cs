using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
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
using AoTracker.Crawlers.Sites.Lashinbang;
using AoTracker.Crawlers.Sites.Mercari;
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
using OperationCanceledException = System.OperationCanceledException;
using Orientation = Android.Content.Res.Orientation;

namespace AoTracker.Android.Fragments.Feed
{
    public partial class FeedPageTabFragment : FragmentBase<FeedTabViewModel>
    {
        private bool _pendingProgressBarAnimation;
        private CancellationTokenSource _smoothProgressCts;

        public override int LayoutResourceId { get; } = Resource.Layout.page_feed_tab;

        public FeedTabEntry TabEntry { get; }

        public FeedPageTabFragment(FeedTabEntry tabEntry)
        {
            TabEntry = tabEntry;
            ViewModel.TabEntry = tabEntry;
        }

        public override void OnPause()
        {
            base.OnPause();
        }

        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.IsLoading).WhenSourceChanges(async () =>
            {
                SwipeToRefreshLayout.Refreshing = ViewModel.IsLoading;
                if (ViewModel.IsLoading)
                {
                    //make sure it wasn't all cached after all
                    await Task.Delay(200);
                    if (ViewModel.IsLoading)
                    {
                        ProgressSpinner.Alpha = 1;
                        ProgressSpinner.Visibility = ViewStates.Visible;
                    }
                }
            }));

            Bindings.Add(this.SetBinding(() => ViewModel.FeedGenerationProgress).WhenSourceChanges(() =>
            {
                SwipeToRefreshLayout.Refreshing = false;
                SmoothSetProgress(ViewModel.FeedGenerationProgress);
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

            ProgressSpinner.ShowText = true;
            RecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
        }

        private void SmoothSetProgress(int progress)
        {
            if (progress == 0)
            {
                ProgressSpinner.Progress = 0;
                return;
            }

            _smoothProgressCts?.Cancel();
            _smoothProgressCts = new CancellationTokenSource();
            SmoothSetProgressLoop(progress, _smoothProgressCts.Token);
        }

        private async void SmoothSetProgressLoop(int progress, CancellationToken token)
        {
            if (progress != 100)
            {
                var diff = progress - ProgressSpinner.Progress;
                var step = diff / 10;
                var delay = TimeSpan.FromSeconds(1) / 10;
                for (int i = 0; i < 10; i++)
                {
                    if (token.IsCancellationRequested)
                        return;

                    ProgressSpinner.Progress += (float)step;
                    try
                    {
                        await Task.Delay(delay, token);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                }
            }
            else
            {
                ProgressSpinner.Progress = 100;
            }


            if (!ViewModel.IsLoading || ViewModel.FeedGenerationProgress == 100)
            {
                ProgressSpinner
                    .Animate()
                    .Alpha(0)
                    .SetDuration(250)
                    .WithEndAction(new Runnable(() => ProgressSpinner.Visibility = ViewStates.Gone))
                    .Start();
            }
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
            else if (item.BackingModel is MandarakeItem mandarakeItem)
            {
                holder.Title.Text = mandarakeItem.Name;
                holder.Detail.Visibility = ViewStates.Gone;
                holder.Subtitle.Text = mandarakeItem.Shop;
                holder.StoreIcon.SetImageResource(Resource.Drawable.mandarake);
            }
            else if (item.BackingModel is MercariItem mercariItem)
            {
                holder.Title.Text = mercariItem.Name;
                holder.Detail.Visibility = ViewStates.Gone;
                holder.Subtitle.Text = string.Empty;
                holder.StoreIcon.SetImageResource(Resource.Drawable.mercari);
            }
            else if (item.BackingModel is LashinbangItem lashinbangItem)
            {
                holder.Title.Text = lashinbangItem.Name;
                holder.Detail.Visibility = ViewStates.Gone;
                holder.Subtitle.Text = string.Empty;
                holder.StoreIcon.SetImageResource(Resource.Drawable.lashinbang);
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

            //if (item.Item.BuyoutPrice != 0)
            //{
            //    holder.PriceSubtitle.Text = $"{item.Item.BuyoutPrice}¥";
            //}

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
                note.Length + value.Length + 1,
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
    }
}