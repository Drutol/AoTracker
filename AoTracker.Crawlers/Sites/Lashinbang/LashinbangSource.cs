using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Sites.Lashinbang
{
    class LashinbangSource : TypedSource<LashinbangSourceParameters, VolatileParametersBase>
    {
        private readonly IHttpClientProvider _httpClientProvider;

        private const string FormatString =
            "https://lashinbang-f-s.snva.jp/?q={0}&s6o=1&pl=1&sort=Number7&limit=60&o=0&n6l=1&callback=callback&controller=lashinbang_front&callback=callback";

        private const string FormatStringDetail =
            "https://shop.lashinbang.com/products/detail/{0}";

        public LashinbangSource(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        protected override Task<string> ObtainSource(LashinbangSourceParameters parameters, VolatileParametersBase volatileParameters)
        {
            return _httpClientProvider.HttpClient.GetStringAsync(string.Format(FormatString, parameters.SearchQuery));
        }

        public override Task<string> ObtainSource(string id)
        {
            return _httpClientProvider.HttpClient.GetStringAsync(string.Format(FormatStringDetail, id));
        }
    }
}
