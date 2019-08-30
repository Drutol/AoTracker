using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ICrawlerResultList<T>> Parse(string data, CrawlerParameters parameters)
        {
            var result = await Parse(data, (TParameters) parameters.Parameters);

            foreach (var crawlerResultItem in result.Results)
            {
                ApplyPriceOffsets(crawlerResultItem, parameters.Parameters);
            }

            return result;
        }

        private void ApplyPriceOffsets(T item, ICrawlerSourceParameters parameters)
        {
            item.Price += (float)parameters.OffsetIncrease;
            item.Price += (float)(item.Price * parameters.PercentageIncrease / 100);
        }

        protected bool IsItemExcluded(string itemName, TParameters parameters)
        {
            if (parameters.ExcludedKeywords == null)
                return false;

            return parameters.ExcludedKeywords.Any(itemName.Contains);
        }

        protected abstract Task<ICrawlerResultList<T>> Parse(string data, TParameters parameters);
        public abstract Task<ICrawlerResultSingle<T>> ParseDetail(string data, string id);
    }
}
