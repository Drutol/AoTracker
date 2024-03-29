﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Infrastructure;
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

        public Task<ICrawlerResultList<SurugayaItem>> Parse(string data, CrawlerParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<ICrawlerResultSingle<SurugayaItem>> ParseDetail(string data, string id)
        {
            throw new NotImplementedException();
        }
    }
}
