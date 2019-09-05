using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.UWP.Utils
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private HttpClient _httpClient;

        public HttpClient HttpClient
        {
            get
            {
                if (_httpClient != null)
                    return _httpClient;

                var handler = new HttpClientHandler
                {
                    UseCookies = true,
                    CookieContainer = new CookieContainer()
                };
                handler.CookieContainer.Add(new Cookie("adult", "1", "/", "www.suruga-ya.jp"));
                _httpClient = new HttpClient(handler);

                return _httpClient;
            }
        }
    }
}
