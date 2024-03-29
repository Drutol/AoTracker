﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Utilities.Shared;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Messaging;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Infrastructure;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.Models.Messages;
using AoTracker.Infrastructure.Statics;
using AoTracker.Infrastructure.Util;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
using Autofac;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Feed
{
    public class FeedTabViewModel : ViewModelBase
    {
        private readonly IVersionProvider _versionProvider;
        private readonly IFeedProvider _feedProvider;
        private readonly IFeedHistoryProvider _feedHistoryProvider;
        private readonly IIgnoredItemsManager _ignoredItemsManager;
        private readonly ISettings _settings;
        private readonly ILifetimeScope _lifetimeScope;
        private CancellationTokenSource _feedCts;
        private bool _isLoading;
        private List<HistoryFeedEntry> _feedHistory;
        private readonly List<FeedItemViewModel> _aggregatedFeed = new List<FeedItemViewModel>();
        private int _expectedBatches;
        private int _receivedBatches;
        private int _feedGenerationProgress;
        private FeedTabEntry _tabEntry;
        private bool _isPreparing;
        private string _progressLabel;
        private bool _awaitingManualLoad;
        private string _searchQuery;
        private DateTime _batchGenerationTime;

        public override PageIndex PageIdentifier { get; }

        public FeedTabEntry TabEntry
        {
            get => _tabEntry;
            set
            {
                if (_tabEntry != null)
                {
                    _tabEntry.CrawlerSetsChanged -= TabEntryOnCrawlerSetsChanged;
                    _tabEntry.Disposed -= TabEntryOnDisposed;
                }
                _tabEntry = value;
                _tabEntry.CrawlerSetsChanged += TabEntryOnCrawlerSetsChanged;
                _tabEntry.Disposed += TabEntryOnDisposed;
            }
        }

        private void TabEntryOnDisposed(object sender, EventArgs e)
        {
            MessengerInstance.Unregister<SearchQueryMessage>(this, OnSearchQueryMessage);
            MessengerInstance.Unregister<ToolbarActionMessage>(this, OnToolbarAction);
        }

        private void TabEntryOnCrawlerSetsChanged(object sender, EventArgs e)
        {
            Feed.Clear();
            if (!_settings.AutoLoadFeedTab)
                AwaitingManualLoad = true;
        }

        public SmartObservableCollection<IMerchItem> Feed { get; } =
            new SmartObservableCollection<IMerchItem>();

        public FeedTabViewModel(
            IVersionProvider versionProvider,
            IFeedProvider feedProvider,
            IFeedHistoryProvider feedHistoryProvider,
            IIgnoredItemsManager ignoredItemsManager,
            ISettings settings)
        {
            _versionProvider = versionProvider;
            _feedProvider = feedProvider;
            _feedHistoryProvider = feedHistoryProvider;
            _ignoredItemsManager = ignoredItemsManager;
            _settings = settings;
            _lifetimeScope = ResourceLocator.ObtainScope();
            _feedProvider.NewCrawlerBatch += FeedProviderOnNewCrawlerBatch;
            _feedProvider.Finished += FeedProviderOnFinished;

            MessengerInstance.Register<SearchQueryMessage>(this, OnSearchQueryMessage);
            MessengerInstance.Register<ToolbarActionMessage>(this, OnToolbarAction);
        }

        private void OnToolbarAction(ToolbarActionMessage action)
        {
            if(action == ToolbarActionMessage.CollapsedSearchQuery)
            {
                if (!string.IsNullOrEmpty(_searchQuery))
                {
                    _searchQuery = null;
                    BuildFeed();
                }
            }
            //else if(action == ToolbarActionMessage.ExpandedSearchQuery)
            //{
            //    if (!string.IsNullOrEmpty(_previousSearchQueryBeforeCollapse))
            //    {
            //        _searchQuery = _previousSearchQueryBeforeCollapse;
            //        BuildFeed();
            //    }
            //}
        }

        private void OnSearchQueryMessage(SearchQueryMessage message)
        {
            if (!message.Query.Equals(_searchQuery))
            {
                //_previousSearchQueryBeforeCollapse = message.Query;
                _searchQuery = message.Query;
                BuildFeed();
            }
        }

        private async void FeedProviderOnFinished(object sender, EventArgs e)
        {
            IsLoading = false;
            BuildFeed();

            foreach (var group in _aggregatedFeed.GroupBy(model => model.SetOfOrigin))
            {
                await _feedHistoryProvider.UpdateHistory(
                    group.Key,
                    group.Select(model => model.BuildHistoryEntry()).ToList());
            }
        }

        private void BuildFeed()
        {
            if(!_aggregatedFeed?.Any() ?? true)
                return;

            Feed.Clear();

            var items = new List<IMerchItem>();

            IEnumerable<FeedItemViewModel> finalFeedSource = _aggregatedFeed;

            if (!string.IsNullOrEmpty(_searchQuery))
                finalFeedSource = finalFeedSource.Where(model => model.Item.Name.Contains(_searchQuery));

            var groups = finalFeedSource
                .GroupBy(model => model.LastChanged, new MinuteDateTimeEqualityComparer())
                .OrderByDescending(g => g.Key);
            foreach (var group in groups)
            {
                items.Add(new FeedChangeGroupItem(group.Key, group));
                items.AddRange(group);
            }

            Feed.PlatformAddRange(items, _versionProvider.Platform);
        }

        private void FeedProviderOnNewCrawlerBatch(object sender, FeedBatch e)
        {
            if (IsPreparing)
            {
                _batchGenerationTime = DateTime.UtcNow;
            }

            IsPreparing = false;
            

            if (!e.CrawlerResult.IsCached)
            {
                if (e.CrawlerResult.HasMore)
                    _expectedBatches++;
                _receivedBatches++;
                FeedGenerationProgress = (int)((_receivedBatches * 100d) / _expectedBatches);
                ProgressLabel = $"{_receivedBatches}/{_expectedBatches}";
            }

            if (e.CrawlerResult.Success)
            {
                var viewModels = new List<FeedItemViewModel>();
                foreach (var crawlerResultItem in e.CrawlerResult.Results.Where(item =>
                    !_ignoredItemsManager.IsItemIgnored(item)))
                {
                    var vmType = crawlerResultItem.Domain == CrawlerDomain.Yahoo
                        ? typeof(FeedItemViewModel<YahooItem>)
                        : typeof(FeedItemViewModel);

                    var vm = (FeedItemViewModel)_lifetimeScope.Resolve(
                        vmType,
                        new TypedParameter(typeof(ICrawlerResultItem), crawlerResultItem),
                        new TypedParameter(typeof(FeedTabViewModel), this));

                    vm.WithHistory(_feedHistory, e.SetOfOrigin, _batchGenerationTime);
                    viewModels.Add(vm);
                }

                _aggregatedFeed.AddRange(viewModels);
            }
        }

        public async void RefreshFeed(bool force = false)
        {
            AwaitingManualLoad = false;
            _aggregatedFeed.Clear();
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
            _expectedBatches = _feedProvider.StartAggregating(TabEntry.CrawlerSets, _feedCts.Token, force);
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
