using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AoLibs.Utilities.Shared;
using AoTracker.Crawlers.Interfaces;
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
        private readonly ILifetimeScope _lifetimeScope;
        private readonly AppVariables _appVariables;
        private CancellationTokenSource _feedCts;
        private bool _isLoading;
        private List<HistoryFeedEntry> _feedHistory;
        private List<FeedItemViewModel> _aggregatedFeed = new List<FeedItemViewModel>();

        public FeedTabEntry TabEntry { get; set; }

        public SmartObservableCollection<IFeedItem> Feed { get; } =
            new SmartObservableCollection<IFeedItem>();


        public FeedTabViewModel(
            IFeedProvider feedProvider,
            AppVariables appVariables)
        {
            _feedProvider = feedProvider;
            _lifetimeScope = ResourceLocator.ObtainScope();
            _appVariables = appVariables;
            _feedProvider.NewCrawlerBatch += FeedProviderOnNewCrawlerBatch;
            _feedProvider.Finished += FeedProviderOnFinished;

            Title = "Feed";
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
                items.Add(new FeedChangeGroupItem(group.Key));
                items.AddRange(group);
            }
            Feed.AddRange(items);
            _feedHistory = Feed.OfType<FeedItemViewModel>().Select(model => model.BuildHistoryEntry()).ToList();
            await _appVariables.FeedHistory.SetAsync(_feedHistory);
            _aggregatedFeed.Clear();
        }

        private void FeedProviderOnNewCrawlerBatch(object sender, IEnumerable<ICrawlerResultItem> e)
        {
            var viewModels = new List<FeedItemViewModel>();
            foreach (var crawlerResultItem in e)
            {
                var vm = _lifetimeScope.Resolve<FeedItemViewModel>(
                    new TypedParameter(typeof(ICrawlerResultItem), crawlerResultItem),
                    new TypedParameter(typeof(FeedTabViewModel), this));
                vm.WithHistory(_feedHistory);
                viewModels.Add(vm);
            }

            _aggregatedFeed.AddRange(viewModels);
        }

        public async void RefreshFeed(bool force = false)
        {
            Feed.Clear();
            IsLoading = true;
            _feedCts = new CancellationTokenSource();
            _feedProvider.StartAggregating(TabEntry.CrawlerSets, _feedCts.Token, force);
        }

        public void NavigatedTo()
        {
            if(!Feed.Any())
                RefreshFeed();
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        public RelayCommand<ICrawlerResultItem> SelectFeedItemCommand =>
            new RelayCommand<ICrawlerResultItem>(item => { });

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
    }
}
