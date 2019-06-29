using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Surugaya;

namespace AoTracker.Infrastructure.Crawling.CloudParser
{
    public class SurugayaCloudParser : ICrawlerParser<SurugayaItem>
    {
        private readonly IHttpClientProvider _httpClientProvider;

        public SurugayaCloudParser(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public Task<ICrawlerResult<SurugayaItem>> Parse(string data)
        {
            throw new NotImplementedException();
        }
    }
}
