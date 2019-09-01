using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Sites.Mercari
{
    public class MercariSource : TypedSource<MercariSourceParameters, VolatileParametersBase>
    {
        private const string FormatString = "https://www.mercari.com/jp/search/?keyword={0}&category_root=&brand_name=&brand_id=&size_group=&price_min=&price_max=&status_on_sale=1";
        private const string FormatStringDetail = "https://item.mercari.com/jp/{0}";

        private readonly IHttpClientProvider _httpClientProvider;

        public MercariSource(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        protected override async Task<string> ObtainSource(
            MercariSourceParameters parameters,
            VolatileParametersBase volatileParameters,
            CancellationToken token)
        {
            var result = await _httpClientProvider.HttpClient.GetAsync(string.Format(FormatString, parameters.SearchQuery), token);
            return await result.Content.ReadAsStringAsync();
        }

        public override Task<string> ObtainSource(string id)
        {
            return _httpClientProvider.HttpClient.GetStringAsync(string.Format(FormatStringDetail, id));
        }
    }
}
