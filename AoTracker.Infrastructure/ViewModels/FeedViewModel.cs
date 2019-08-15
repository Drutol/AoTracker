using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using AoLibs.Utilities.Shared;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Infrastructure;
using AoTracker.Infrastructure.Util;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
using Autofac;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels
{
    public class FeedViewModel : ViewModelBase
    {
        private readonly IFeedProvider _feedProvider;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly AppVariables _appVariables;
        private CancellationTokenSource _feedCts;
        private bool _isLoading;
        private List<HistoryFeedEntry> _feedHistory;
        private List<FeedItemViewModel> _aggregatedFeed = new List<FeedItemViewModel>();

        public ObservableCollection<IFeedItem> Feed { get; set; } =
            new ObservableCollection<IFeedItem>();


        public FeedViewModel(
            IFeedProvider feedProvider,
            ILifetimeScope lifetimeScope,
            AppVariables appVariables)
        {
            _feedProvider = feedProvider;
            _lifetimeScope = lifetimeScope;
            _appVariables = appVariables;
            _feedProvider.NewCrawlerBatch += FeedProviderOnNewCrawlerBatch;
            _feedProvider.Finished += FeedProviderOnFinished;

            Title = "Feed";
        }

        private async void FeedProviderOnFinished(object sender, EventArgs e)
        {
            IsLoading = false;

            Feed = new ObservableCollection<IFeedItem>(
                _aggregatedFeed.OrderByDescending(model => model.LastChanged));
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
                    new TypedParameter(typeof(ICrawlerResultItem), crawlerResultItem));
                vm.WithHistory(_feedHistory);
                viewModels.Add(vm);
            }

            _aggregatedFeed.AddRange(viewModels);
        }

        public async void NavigatedTo()
        {
            _feedHistory = await _appVariables.FeedHistory.GetAsync();
            _feedCts = new CancellationTokenSource();
            IsLoading = true;
            _feedProvider.StartAggregating(_feedCts.Token);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        public RelayCommand<ICrawlerResultItem> SelectFeedItemCommand =>
            new RelayCommand<ICrawlerResultItem>(item => { });
    }
}
