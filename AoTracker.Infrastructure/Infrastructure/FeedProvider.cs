using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private SemaphoreSlim _semaphore = new SemaphoreSlim(3);

        public event EventHandler<FeedBatch> NewCrawlerBatch;
        public event EventHandler Finished;

        public FeedProvider(
            ICrawlerManagerProvider crawlerManagerProvider,
            IUserDataProvider userDataProvider,
            AppVariables appVariables)
        {
            _userDataProvider = userDataProvider;
            _appVariables = appVariables;
            _crawlerManager = crawlerManagerProvider.Manager;
        }

        public void StartAggregating(List<CrawlerSet> sets, CancellationToken feedCtsToken, bool force, ref int expectedBatches)
        {
            expectedBatches = sets.Sum(set => set.Descriptors.Count);

            if (!_isAggregating)
                AggregateFeed(sets, feedCtsToken, force);
        }

        private async void AggregateFeed(List<CrawlerSet> sets, CancellationToken feedCtsToken, bool force)
        {
            try
            {
                var volatileParameters = new VolatileParametersBase
                {
                    Page = 1,
                    UseCache = !force
                };

                var tasks = new List<Task>();
                foreach (var crawlingSet in sets)
                {
                    tasks.Add(CrawlDescriptor(crawlingSet, volatileParameters));
                }

                await Task.WhenAll(tasks);
            }
            finally
            {
                _isAggregating = false;
            }

            Finished?.Invoke(this, EventArgs.Empty);
        }

        private async Task CrawlDescriptor(CrawlerSet crawlingSet, VolatileParametersBase volatileParameters)
        {
            foreach (var descriptor in crawlingSet.Descriptors)
            {
                try
                {
                    await _semaphore.WaitAsync();

                    var crawler = _crawlerManager.GetCrawler(descriptor.CrawlerDomain);

                    var result = await crawler.Crawl(new CrawlerParameters
                    {
                        Parameters = descriptor.CrawlerSourceParameters,
                        VolatileParameters = volatileParameters
                    });

                    if (result.Success)
                    {
                        NewCrawlerBatch?.Invoke(this, new FeedBatch
                        {
                            CrawlerResult = result,
                            SetOfOrigin = crawlingSet
                        });
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }
    }
}