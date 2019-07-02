using System.IO;
using System.Threading.Tasks;
using AoTracker.Crawlers.Infrastructure;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerParser<T> where T : ICrawlerResultItem
    {
        Task<ICrawlerResult<T>> Parse(string data, CrawlerParameters parameters);
    }
}
