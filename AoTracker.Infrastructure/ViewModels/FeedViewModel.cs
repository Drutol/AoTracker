using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using AoTracker.Crawlers.Interfaces;
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
        private CancellationTokenSource _feedCts;
        private bool _isLoading;

        public ObservableCollection<IFeedItem> Feed { get; set; } =
            new ObservableCollection<IFeedItem>();


        public FeedViewModel(IFeedProvider feedProvider, ILifetimeScope lifetimeScope)
        {
            _feedProvider = feedProvider;
            _lifetimeScope = lifetimeScope;
            _feedProvider.NewCrawlerBatch += FeedProviderOnNewCrawlerBatch;
            _feedProvider.Finished += FeedProviderOnFinished;

            Title = "Feed";
        }

        private void FeedProviderOnFinished(object sender, EventArgs e)
        {

        }

        private void FeedProviderOnNewCrawlerBatch(object sender, IEnumerable<ICrawlerResultItem> e)
        {
            foreach (var crawlerResultItem in e)
            {
                var vm = _lifetimeScope.Resolve<FeedItemViewModel>(new TypedParameter(typeof(ICrawlerResultItem),
                    crawlerResultItem));
                Feed.Add(vm);
            }
        }

        public void NavigatedTo()
        {
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
