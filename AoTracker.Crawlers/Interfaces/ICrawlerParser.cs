using System.IO;
using System.Threading.Tasks;
using AoTracker.Crawlers.Infrastructure;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerParser<T> where T : ICrawlerResultItem
    {
        Task<ICrawlerResultList<T>> Parse(string data, CrawlerParameters parameters);
        Task<ICrawlerResultSingle<T>> ParseDetail(string data, string id);
    }
}
