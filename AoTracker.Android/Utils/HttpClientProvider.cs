using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoTracker.Crawlers.Interfaces;
using ModernHttpClient;

namespace AoTracker.Android.Utils
{
    public class HttpClientProvider : IHttpClientProvider
    {
        public HttpClientProvider()
        {
            var cookieHandler = new NativeCookieHandler();
            cookieHandler.SetCookies(new Cookie[]
            {
                new Cookie("adult", "1", "/", "www.suruga-ya.jp"),
            });


            HttpClient = new HttpClient(new NativeMessageHandler(
                throwOnCaptiveNetwork: false,
                new TLSConfig
                {
                    //DangerousAcceptAnyServerCertificateValidator = true,
                    //DangerousAllowInsecureHTTPLoads = true
                },
                cookieHandler: cookieHandler)
            {
                UseCookies = true
            });
        }

        public HttpClient HttpClient { get; } 
    }
}