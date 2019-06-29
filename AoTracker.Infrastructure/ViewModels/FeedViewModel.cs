using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.ViewModels
{
    public class FeedViewModel : ViewModelBase
    {
        private readonly IFeedProvider _feedProvider;
        private CancellationTokenSource _feedCts;

        public ObservableCollection<ICrawlerResultItem> Feed { get; set; } = new ObservableCollection<ICrawlerResultItem>();

        public FeedViewModel(IFeedProvider feedProvider)
        {
            _feedProvider = feedProvider;
            _feedProvider.NewCrawlerBatch += FeedProviderOnNewCrawlerBatch;
        }

        private void FeedProviderOnNewCrawlerBatch(object sender, IEnumerable<ICrawlerResultItem> e)
        {
            foreach (var crawlerResultItem in e)
            {
                Feed.Add(crawlerResultItem);
            }
        }

        public void NavigatedTo()
        {
            _feedCts = new CancellationTokenSource();
            _feedProvider.StartAggregating(_feedCts.Token);
        }
    }
}
