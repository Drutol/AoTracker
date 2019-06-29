using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.Infrastructure
{
    public class FeedProvider : IFeedProvider
    {
        private readonly IUserDataProvider _userDataProvider;
        private ICrawlerManager _crawlerManager;

        public event EventHandler<IEnumerable<ICrawlerResultItem>> NewCrawlerBatch;

        public FeedProvider(
            ICrawlerManagerProvider crawlerManagerProvider,
            IUserDataProvider userDataProvider)
        {
            _userDataProvider = userDataProvider;
            _crawlerManager = crawlerManagerProvider.Manager;
        }

        public async void StartAggregating(CancellationToken feedCtsToken)
        {
            foreach (var crawlingSet in _userDataProvider.CrawlingSets)
            {
                foreach (var descriptor in crawlingSet.Descriptors)
                {
                    var crawler = _crawlerManager.GetCrawler(descriptor.CrawlerDomain);

                    var result = await crawler.Crawl(descriptor.CrawlerSourceParameters);

                    NewCrawlerBatch?.Invoke(this, result.Results);
                }
            }
        }
    }
}
