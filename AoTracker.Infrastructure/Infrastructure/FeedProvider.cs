using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AoLibs.Utilities.Shared;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Models;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.Infrastructure
{
    public class FeedProvider : IFeedProvider
    {
        private readonly IUserDataProvider _userDataProvider;
        private readonly AppVariables _appVariables;
        private readonly ICrawlerManager _crawlerManager;
        private bool _isAggregating;

        public event EventHandler<IEnumerable<ICrawlerResultItem>> NewCrawlerBatch;
        public event EventHandler Finished;

        public List<ICrawlerResultItem> CachedFeed { get; } = new List<ICrawlerResultItem>();

        public FeedProvider(
            ICrawlerManagerProvider crawlerManagerProvider,
            IUserDataProvider userDataProvider,
            AppVariables appVariables)
        {
            _userDataProvider = userDataProvider;
            _appVariables = appVariables;
            _crawlerManager = crawlerManagerProvider.Manager;
        }

        public async void StartAggregating(CancellationToken feedCtsToken, bool force)
        {
            if(_isAggregating)
                return;

            _isAggregating = true;

            try
            {
                if (!force && CachedFeed.Any())
                {
                    NewCrawlerBatch?.Invoke(this, CachedFeed);
                    return;
                }

                var volatileParameters = new VolatileParametersBase
                {
                    Page = 1,
                    UseCache = !force
                };

                foreach (var crawlingSet in _userDataProvider.CrawlingSets)
                {
                    foreach (var descriptor in crawlingSet.Descriptors)
                    {
                        var crawler = _crawlerManager.GetCrawler(descriptor.CrawlerDomain);

                        var result = await crawler.Crawl(new CrawlerParameters
                        {
                            Parameters = descriptor.CrawlerSourceParameters,
                            VolatileParameters = volatileParameters
                        });

                        if (result.Success)
                        {
                            NewCrawlerBatch?.Invoke(this, result.Results);
                            CachedFeed.AddRange(result.Results);
                        }
                    }
                }

                Finished?.Invoke(this, EventArgs.Empty);
            }
            finally
            {
                _isAggregating = false;
            }
        }
    }
}
