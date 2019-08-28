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
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.View.Menu;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using AoLibs.Navigation.Android.Navigation;
using AoLibs.Utilities.Android;
using AoLibs.Utilities.Android.Listeners;
using AoLibs.Utilities.Android.Views;
using AoTracker.Android.Themes;
using AoTracker.Android.Utils;
using AoTracker.Android.ViewHolders;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Crawlers.Sites.Lashinbang;
using AoTracker.Crawlers.Sites.Mercari;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.Util;
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
        private CancellationTokenSource _smoothProgressCts;

        public override int LayoutResourceId { get; } = Resource.Layout.page_feed_tab;

        public FeedTabEntry TabEntry { get; }

        public FeedPageTabFragment(FeedTabEntry tabEntry)
        {
            TabEntry = tabEntry;
            ViewModel.TabEntry = tabEntry;
        }

        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }

        protected override void InitBindings()
        {
            Bindings.Add(
                this.SetBinding(() => ViewModel.IsLoading,
                    () => LoadingLayout.Visibility).ConvertSourceToTarget(BindingConverters.BoolToVisibility));

            Bindings.Add(
                this.SetBinding(() => ViewModel.ProgressLabel,
                    () => ProgressLabel.Text));

            Bindings.Add(
                this.SetBinding(() => ViewModel.IsPreparing,
                    () => ProgressSpinner.Visibility).ConvertSourceToTarget(BindingConverters.BoolToVisibility));

            Bindings.Add(this.SetBinding(() => ViewModel.AwaitingManualLoad).WhenSourceChanges(() =>
            {
                if (ViewModel.AwaitingManualLoad)
                {
                    ManualLoadButton.Show();
                }
                else
                {
                    ManualLoadButton.Hide();
                }
            }));

            Bindings.Add(this.SetBinding(() => ViewModel.FeedGenerationProgress).WhenSourceChanges(() =>
            {
                SmoothSetProgress(ViewModel.FeedGenerationProgress);
            }));

            RecyclerView.SetAdapter(new RecyclerViewAdapterBuilder<IMerchItem, RecyclerView.ViewHolder>()
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

            RecyclerView.AddOnScrollListener(new ScrollListener(this));
            SwipeToRefreshLayout.ScrollingView = RecyclerView;
            SwipeToRefreshLayout.Refresh += SwipeToRefreshLayoutOnRefresh;
            RecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            ManualLoadButton.SetOnClickCommand(ViewModel.RequestManualLoadCommand);
        }

        #region Loader

        private void SmoothSetProgress(int progress)
        {
            if (progress == 0)
            {
                ProgressBar.Progress = 0;
                return;
            }

            _smoothProgressCts?.Cancel();
            _smoothProgressCts = new CancellationTokenSource();
            SmoothSetProgressLoop(progress, _smoothProgressCts.Token);
        }

        private async void SmoothSetProgressLoop(int progress, CancellationToken token)
        {
            const int steps = 20;
            var diff = progress - ProgressBar.Progress;
            var step = diff / steps;
            var delay = TimeSpan.FromSeconds(1) / steps;
            for (int i = 0; i < steps; i++)
            {
                if (token.IsCancellationRequested)
                    return;

                ProgressBar.Progress += (float) step;
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

        #endregion

        private void FeedChangeGroupDataTemplate(FeedChangeGroupItem item, FeedChangeGroupHolder holder, int position)
        {
            var diff = DateTime.UtcNow - item.LastChanged;
            if (diff > TimeSpan.FromMinutes(10))
            {
                var changedDiff = SharedUtil.TimeDiffToString(diff);

                holder.Label.Text = string.Format(AppResources.Item_Feed_LastChanged, changedDiff);
            }
            else
            {
                holder.Label.Text = AppResources.Item_FeedChangeGroup_Recently;
            }
        }

        private void SwipeToRefreshLayoutOnRefresh(object sender, EventArgs e)
        {
            SwipeToRefreshLayout.Refreshing = false;
            ViewModel.RefreshFeed(true);
        }

        private void FeedItemDataTemplate(FeedItemViewModel item, FeedItemHolder holder, int position)
        {
            MerchItemHolderTemplate.DataTemplate(item, holder, position);
            holder.ClickSurface.SetOnLongClickListener(new OnLongClickListener(view => HandlePopupMenu(view, item)));
            holder.ClickSurface.SetOnClickCommand(item.NavigateItemWebsiteCommand);
        }

        private void FeedItemYahooDataTemplate(FeedItemViewModel<YahooItem> item, FeedItemYahooHolder holder,
            int position)
        {
            MerchItemYahooHolderTemplate.DataTemplate(item, holder, position);
            holder.ClickSurface.SetOnLongClickListener(new OnLongClickListener(view => HandlePopupMenu(view, item)));
            holder.ClickSurface.SetOnClickCommand(item.NavigateItemWebsiteCommand);
        }

        private void HandlePopupMenu(View view, FeedItemViewModel viewModel)
        {
            var menuBuilder = new MenuBuilder(Activity);
            menuBuilder.Add(0, 0, 0, AppResources.Item_Feed_AddIgnore).SetIcon(Resource.Drawable.icon_stop);

            if (!viewModel.IsWatched)
                menuBuilder.Add(0, 1, 0, AppResources.Item_Feed_AddWatched).SetIcon(Resource.Drawable.icon_eye);
            else
                menuBuilder.Add(0, 2, 0, AppResources.Item_Feed_RemoveWatched).SetIcon(Resource.Drawable.icon_eye_off);

            menuBuilder.SetCallback(new MenuCallback((sender, menuItem) =>
            {
                if (menuItem.ItemId == 0)
                {
                    viewModel.IgnoreItemCommand.Execute(null);
                }
                else if (menuItem.ItemId == 1)
                {
                    viewModel.WatchItemCommand.Execute(null);
                }
                else if(menuItem.ItemId == 2)
                {
                    viewModel.UnwatchItemCommand.Execute(null);
                }
            }));
            var menuPopupHelper = new MenuPopupHelper(Context, menuBuilder);
            menuPopupHelper.SetAnchorView(view);
            menuPopupHelper.SetForceShowIcon(true);
            menuPopupHelper.Show();
        }
    }
}