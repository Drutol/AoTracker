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

        public async void StartAggregating(List<CrawlerSet> sets, CancellationToken feedCtsToken, bool force)
        {
            if (_isAggregating)
                return;
            try
            {
                var volatileParameters = new VolatileParametersBase
                {
                    Page = 1,
                    UseCache = !force
                };

                foreach (var crawlingSet in sets)
                {
                    await CrawlDescriptor(crawlingSet, volatileParameters);
                }
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
        }
    }
}
