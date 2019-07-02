using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Abstract
{
    public abstract class TypedParser<T, TParameters> : ICrawlerParser<T>
        where T : ICrawlerResultItem
        where TParameters : ICrawlerSourceParameters
    {
        public Task<ICrawlerResult<T>> Parse(string data, CrawlerParameters parameters)
        {
            return Parse(data, (TParameters) parameters.Parameters);
        }

        protected abstract Task<ICrawlerResult<T>> Parse(string data, TParameters parameters);
    }
}
