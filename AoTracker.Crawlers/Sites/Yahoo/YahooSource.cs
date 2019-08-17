using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Sites.Yahoo
{
    public class YahooSource : TypedSource<YahooSourceParameters, VolatileParametersBase>
    {
        private const string FormatString =
            "https://www.fromjapan.co.jp/sites/yahooauction/search?exhibitType=0&condition=0&hits=120&keyword={0}&sort=end&category=All";

        private readonly IHttpClientProvider _clientProvider;

        public YahooSource(IHttpClientProvider clientProvider)
        {
            _clientProvider = clientProvider;
        }

        protected override Task<string> ObtainSource(YahooSourceParameters parameters,
            VolatileParametersBase volatileParameters)
        {
            return _clientProvider.HttpClient.GetStringAsync(string.Format(FormatString, parameters.SearchQuery));
        }
    }
}
