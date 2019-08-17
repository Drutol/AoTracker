﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Sites.Mercari
{
    public class MercariSource : TypedSource<MercariSourceParameters, VolatileParametersBase>
    {
        private const string FormatString = "https://www.mercari.com/jp/search/?keyword={0}";

        private readonly IHttpClientProvider _httpClientProvider;

        public MercariSource(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        protected override Task<string> ObtainSource(MercariSourceParameters parameters,
            VolatileParametersBase volatileParameters)
        {
            return _httpClientProvider.HttpClient.GetStringAsync(string.Format(FormatString, parameters.SearchQuery));
        }
    }
}
