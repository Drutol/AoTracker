using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Surugaya
{
    public class SurugayaSource : TypedSource<SurugayaSourceParameters, VolatileParametersBase>
    {
        private const string FormatString
            = "https://suruga-ya.jp/search?category=10&search_word={0}&adult_s=1&rankBy=modificationTime%3Adescending";
        private const string FormatStringDetail 
            = "https://www.suruga-ya.jp/product/detail/{0}";
        private readonly IHttpClientProvider _httpClientProvider;

        public SurugayaSource(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        protected override Task<string> ObtainSource(SurugayaSourceParameters parameters, VolatileParametersBase volatileParameters)
        {
            return _httpClientProvider.HttpClient.GetStringAsync(string.Format(FormatString,
                parameters.SearchQuery));
        }

        public override Task<string> ObtainSource(string id)
        {
            return _httpClientProvider.HttpClient.GetStringAsync(string.Format(FormatStringDetail, id));
        }
    }
}
