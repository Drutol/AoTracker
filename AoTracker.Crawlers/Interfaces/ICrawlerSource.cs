using System.IO;
using System.Threading.Tasks;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerSource
    {
        Task<string> ObtainSource(ICrawlerSourceParameters parameters);
    }
}
