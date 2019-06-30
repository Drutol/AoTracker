using System.IO;
using System.Threading.Tasks;
using AoTracker.Crawlers.Infrastructure;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerSource
    {
        Task<string> ObtainSource(CrawlerParameters parameters);
    }
}
