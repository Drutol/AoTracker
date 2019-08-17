using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Crawlers.Surugaya;

namespace AoTracker.Crawlers.Mandarake
{
    public class MandarakeSource : TypedSource<MandarakeSourceParameters, VolatileParametersBase>
    {
        private const string FormatString
            = "https://order.mandarake.co.jp/order/ListPage/list?dispCount=48&layout=2&soldOut=1&keyword={0}&lang=ja";

        private readonly IHttpClientProvider _httpClientProvider;

        public MandarakeSource(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        protected override Task<string> ObtainSource(MandarakeSourceParameters parameters, VolatileParametersBase volatileParameters)
        {
            return _httpClientProvider.HttpClient.GetStringAsync(string.Format(FormatString,
                parameters.SearchQuery));
        }
    }
}
