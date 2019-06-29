using System.IO;
using System.Threading.Tasks;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerParser<T> where T : ICrawlerResultItem
    {
        Task<ICrawlerResult<T>> Parse(string data);
    }
}
