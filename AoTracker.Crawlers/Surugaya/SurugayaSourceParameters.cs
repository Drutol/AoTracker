using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Surugaya
{
    public class SurugayaSourceParameters : ICrawlerSourceParameters
    {
        public string SearchQuery { get; set; }
        public int Page { get; set; }
    }
}
