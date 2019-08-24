using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AoLibs.Utilities.Shared;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Infrastructure;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.Statics;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
using Autofac;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Feed
{
    public class FeedTabViewModel : ViewModelBase
    {
        private readonly IFeedProvider _feedProvider;
        private readonly IFeedHistoryProvider _feedHistoryProvider;
        private readonly IIgnoredItemsManager _ignoredItemsManager;
        private readonly ISettings _settings;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly AppVariables _appVariables;
        private CancellationTokenSource _feedCts;
        private bool _isLoading;
        private List<HistoryFeedEntry> _feedHistory;
        private List<FeedItemViewModel> _aggregatedFeed = new List<FeedItemViewModel>();
        private int _expectedBatches;
        private int _receivedBatches;
        private int _feedGenerationProgress;
        private FeedTabEntry _tabEntry;
        private bool _isPreparing;
        private string _progressLabel;
        private bool _awaitingManualLoad;

        public FeedTabEntry TabEntry
        {
            get => _tabEntry;
            set
            {
                if(_tabEntry != null)
                    _tabEntry.CrawlerSetsChanged -= TabEntryOnCrawlerSetsChanged;
                _tabEntry = value;
                _tabEntry.CrawlerSetsChanged += TabEntryOnCrawlerSetsChanged;
            }
        }

        private void TabEntryOnCrawlerSetsChanged(object sender, EventArgs e)
        {
            Feed.Clear();
            if (!_settings.AutoLoadFeedTab)
                AwaitingManualLoad = true;
        }

        public SmartObservableCollection<IFeedItem> Feed { get; } =
            new SmartObservableCollection<IFeedItem>();


        public FeedTabViewModel(
            IFeedProvider feedProvider,
            IFeedHistoryProvider feedHistoryProvider,
            IIgnoredItemsManager ignoredItemsManager,
            ISettings settings,
            AppVariables appVariables)
        {
            _feedProvider = feedProvider;
            _feedHistoryProvider = feedHistoryProvider;
            _ignoredItemsManager = ignoredItemsManager;
            _settings = settings;
            _lifetimeScope = ResourceLocator.ObtainScope();
            _appVariables = appVariables;
            _feedProvider.NewCrawlerBatch += FeedProviderOnNewCrawlerBatch;
            _feedProvider.Finished += FeedProviderOnFinished;
        }

        private async void FeedProviderOnFinished(object sender, EventArgs e)
        {
            IsLoading = false;
            var items = new List<IFeedItem>();
            var groups = _aggregatedFeed
                .GroupBy(model => model.LastChanged, new MinuteDateTimeEqualityComparer())
                .OrderByDescending(g => g.Key);
            foreach (var group in groups)
            {
                items.Add(new FeedChangeGroupItem(group.Key, group));
                items.AddRange(group);
            }
            Feed.AddRange(items);

            foreach (var group in _aggregatedFeed.GroupBy(model => model.SetOfOrigin))
            {
                await _feedHistoryProvider.UpdateHistory(group.Key,
                    group.Select(model => model.BuildHistoryEntry()).ToList());
            }

            _aggregatedFeed.Clear();
        }

        private void FeedProviderOnNewCrawlerBatch(object sender, FeedBatch e)
        {
            IsPreparing = false;

            if (!e.CrawlerResult.IsCached)
            {
                _receivedBatches++;
                FeedGenerationProgress = (int)((_receivedBatches * 100d) / _expectedBatches);
                ProgressLabel = $"{_receivedBatches}/{_expectedBatches}";
            }

            var viewModels = new List<FeedItemViewModel>();
            foreach (var crawlerResultItem in e.CrawlerResult.Results.Where(item =>
                !_ignoredItemsManager.IsItemIgnored(item)))
            {
                var vmType = crawlerResultItem.Domain == CrawlerDomain.Yahoo
                    ? typeof(FeedItemViewModel<YahooItem>)
                    : typeof(FeedItemViewModel);

                var vm = (FeedItemViewModel) _lifetimeScope.Resolve(
                    vmType,
                    new TypedParameter(typeof(ICrawlerResultItem), crawlerResultItem),
                    new TypedParameter(typeof(FeedTabViewModel), this));

                vm.WithHistory(_feedHistory, e.SetOfOrigin);
                viewModels.Add(vm);
            }

            _aggregatedFeed.AddRange(viewModels);
        }

        public async void RefreshFeed(bool force = false)
        {
            AwaitingManualLoad = false;
            Feed.Clear();
            var historyTasks = TabEntry.CrawlerSets
                .Select(set => _feedHistoryProvider.GetHistory(set))
                .ToList();
            await Task.WhenAll(historyTasks);
            if (!force && _feedProvider.CheckCache(TabEntry.CrawlerSets))
            {
                IsLoading = false;
                IsPreparing = false;
            }
            else
            {
                IsLoading = true;
                IsPreparing = true;
                FeedGenerationProgress = 0;
                _receivedBatches = 0;
                _expectedBatches = 0;
                ProgressLabel = string.Empty;
            }

            _feedHistory = historyTasks
                .SelectMany(task => task.Result ?? Enumerable.Empty<HistoryFeedEntry>())
                .ToList();
            _feedCts = new CancellationTokenSource();
            _feedProvider.StartAggregating(TabEntry.CrawlerSets, _feedCts.Token, force, ref _expectedBatches);
        }

        public int FeedGenerationProgress
        {
            get => _feedGenerationProgress;
            set => Set(ref _feedGenerationProgress, value);
        }

        public void NavigatedTo()
        {
            MessengerInstance.Send(FeedViewModel.Message.ShowJumpToActionButton);

            if(IsLoading)
                return;

            if (!Feed.Any())
            {
                if (_settings.AutoLoadFeedTab)
                {
                    RefreshFeed();
                }
                else
                {
                    AwaitingManualLoad = true;
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        public bool IsPreparing
        {
            get => _isPreparing;
            set => Set(ref _isPreparing, value);
        }

        public string ProgressLabel
        {
            get => _progressLabel;
            set => Set(ref _progressLabel, value);
        }

        public bool AwaitingManualLoad
        {
            get => _awaitingManualLoad;
            set => Set(ref _awaitingManualLoad, value);
        }

        public RelayCommand RequestManualLoadCommand => new RelayCommand(() =>
        {
            RefreshFeed();
        });

        public RelayCommand<ICrawlerResultItem> SelectFeedItemCommand =>
            new RelayCommand<ICrawlerResultItem>(item => { });

        public RelayCommand<bool> RequestJumpToFabVisibilityChangeCommand => new RelayCommand<bool>(visibility =>
        {
            if(visibility)
                MessengerInstance.Send(FeedViewModel.Message.ShowJumpToActionButton);
            else
                MessengerInstance.Send(FeedViewModel.Message.HideJumpToActionButton);
        });

        public override void UpdatePageTitle()
        {
            //don't update on inner fragments
        }

        class MinuteDateTimeEqualityComparer : IEqualityComparer<DateTime>
        {
            public bool Equals(DateTime x, DateTime y)
            {
                return x.Day == y.Day && x.Hour == y.Hour && x.Minute == y.Minute;
            }

            public int GetHashCode(DateTime obj)
            {
                unchecked
                {
                    var hashCode = obj.Day;
                    hashCode = (hashCode * 397) ^ obj.Minute;
                    hashCode = (hashCode * 397) ^ obj.Hour;
                    return hashCode;
                }
            }
        }

        public void RemoveItem(FeedItemViewModel feedItemViewModel)
        {
            Feed.Remove(feedItemViewModel);
            var affectedGroupHeader = Feed.OfType<FeedChangeGroupItem>().First(item =>
                item.GroupedItems.Contains(feedItemViewModel.BackingModel.InternalId));
            affectedGroupHeader.GroupedItems.Remove(feedItemViewModel.BackingModel.InternalId);
            if (!affectedGroupHeader.GroupedItems.Any())
                Feed.Remove(affectedGroupHeader);
        }
    }
}
