using System.IO;
using System.Threading.Tasks;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerSource
    {
        Task<Stream> ObtainSource(ICrawlerSourceParameters parameters);
    }
}
