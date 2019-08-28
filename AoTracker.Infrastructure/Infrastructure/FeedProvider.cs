using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AoLibs.Utilities.Shared;
using AoTracker.Crawlers.Enums;
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
        private readonly ConcurrentDictionary<CrawlerDomain, SemaphoreSlim> _domainSemaphores =
            new ConcurrentDictionary<CrawlerDomain, SemaphoreSlim>();
        private readonly List<Task> _branchedTasks = new List<Task>();

        public event EventHandler<FeedBatch> NewCrawlerBatch;
        public event EventHandler Finished;

        private static VolatileParametersBase _cacheCheckingVolatileParameters = new VolatileParametersBase
        {
            Page = 1,
            UseCache = true
        };

        public FeedProvider(
            ICrawlerManagerProvider crawlerManagerProvider,
            IUserDataProvider userDataProvider,
            AppVariables appVariables)
        {
            _userDataProvider = userDataProvider;
            _appVariables = appVariables;
            _crawlerManager = crawlerManagerProvider.Manager;

            foreach (CrawlerDomain domain in Enum.GetValues(typeof(CrawlerDomain)))
            {
                _domainSemaphores[domain] = new SemaphoreSlim(2);
            }
        }

        public void StartAggregating(List<CrawlerSet> sets, CancellationToken feedCtsToken, bool force, ref int expectedBatches)
        {
            if (force)
            {
                // we have to check every descriptor
                expectedBatches = sets.Sum(set => set.Descriptors.Count);
            }
            else
            {
                // we have to check only these descriptors that don't have cache
                expectedBatches = sets.Sum(set => set.Descriptors.Count(descriptor =>
                    !_crawlerManager.GetCrawler(descriptor.CrawlerDomain).IsCached(
                        new CrawlerParameters(descriptor.CrawlerSourceParameters, _cacheCheckingVolatileParameters))));
            }

            if (!_isAggregating)
                AggregateFeed(sets, feedCtsToken, force);
        }

        public bool CheckCache(List<CrawlerSet> sets)
        {
            return sets.All(set => set.Descriptors.All(descriptor =>
                _crawlerManager.GetCrawler(descriptor.CrawlerDomain)
                    .IsCached(new CrawlerParameters(descriptor.CrawlerSourceParameters, _cacheCheckingVolatileParameters))));
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
                    tasks.Add(CrawlDescriptor(
                        crawlingSet.Descriptors,
                        crawlingSet,
                        volatileParameters));
                }

                await Task.WhenAll(tasks);
                await Task.WhenAll(_branchedTasks);
                _branchedTasks.Clear();
            }
            finally
            {
                _isAggregating = false;
            }

            Finished?.Invoke(this, EventArgs.Empty);
        }

        private async Task CrawlDescriptor(
            List<CrawlerDescriptor> descriptors,
            CrawlerSet crawlingSet,
            VolatileParametersBase volatileParameters)
        { 
            foreach (var descriptor in descriptors)
            {
                var semaphore = _domainSemaphores[descriptor.CrawlerDomain];
                try
                {
                    await semaphore.WaitAsync();
                    var crawler = _crawlerManager.GetCrawler(descriptor.CrawlerDomain);

                    var result = await Task.Run(async () => 
                        await crawler.Crawl(
                        new CrawlerParameters(
                            descriptor.CrawlerSourceParameters,
                            volatileParameters)));

                    if (result.Success)
                    {
                        NewCrawlerBatch?.Invoke(this, new FeedBatch
                        {
                            CrawlerResult = result,
                            SetOfOrigin = crawlingSet
                        });
                    }

                    if (result.HasMore)
                    {
                        var task = CrawlDescriptor(new List<CrawlerDescriptor>
                        {
                            descriptor
                        }, crawlingSet, new VolatileParametersBase
                        {
                            Page = volatileParameters.Page + 1,
                            UseCache = volatileParameters.UseCache
                        });
                        _branchedTasks.Add(task);
#pragma warning disable 4014
                        Task.Run(async () => await task);
#pragma warning restore 4014

                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }
        }
    }
}