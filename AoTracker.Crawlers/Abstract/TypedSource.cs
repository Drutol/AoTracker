using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Abstract
{
    public abstract class TypedSource<TParams, TVolatile> : ICrawlerSource
        where TParams : ICrawlerSourceParameters
        where TVolatile : ICrawlerVolatileParameters
    {
        protected abstract Task<string> ObtainSource(
            TParams parameters,
            TVolatile volatileParameters,
            CancellationToken token);

        public abstract Task<string> ObtainSource(string id);

        public Task<string> ObtainSource(CrawlerParameters parameters, CancellationToken token)
        {
            return ObtainSource((TParams) parameters.Parameters, (TVolatile) parameters.VolatileParameters, token);
        }
    }
}
