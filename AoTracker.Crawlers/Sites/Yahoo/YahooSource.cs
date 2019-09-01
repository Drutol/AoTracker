using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Sites.Yahoo
{
    public class YahooSource : TypedSource<YahooSourceParameters, VolatileParametersBase>
    {
        public static int ItemsPerRequest { get; } = 90;

        private const string FormatString =
            "https://www.fromjapan.co.jp/sites/yahooauction/search?exhibitType=0&condition=0&hits={2}&keyword={0}&sort=end&category=All&page={1}";

        private const string FormatStringDetail =
            "https://www.fromjapan.co.jp/en/auction/yahoo/input/{0}";

        private readonly IHttpClientProvider _clientProvider;

        public YahooSource(IHttpClientProvider clientProvider)
        {
            _clientProvider = clientProvider;
        }

        protected override async Task<string> ObtainSource(
            YahooSourceParameters parameters,
            VolatileParametersBase volatileParameters,
            CancellationToken token)
        {
            var result = await _clientProvider.HttpClient.GetAsync(string.Format(FormatString, parameters.SearchQuery,
                volatileParameters.Page, ItemsPerRequest), token);
            return await result.Content.ReadAsStringAsync();
        }

        public override Task<string> ObtainSource(string id)
        {
            return _clientProvider.HttpClient.GetStringAsync(string.Format(FormatStringDetail, id));
        }
    }
}
