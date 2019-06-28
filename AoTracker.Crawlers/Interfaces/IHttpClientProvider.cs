using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AoTracker.Crawlers.Interfaces
{
    public interface IHttpClientProvider
    {
        HttpClient HttpClient { get; }
    }
}
