using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Item;

namespace AoTracker.Util
{
    public class DesignTimeBindingContextProvider
    {
        //view models
        public static WelcomeViewModel WelcomeViewModel { get; set; }
        public static FeedViewModel FeedViewModel { get; set; }
        public static MainViewModel MainViewModel { get; set; }
        public static CrawlerSetsViewModel CrawlerSetsViewModel { get; set; }
        public static CrawlerSetDetailsViewModel CrawlerSetDetailsViewModel { get; set; }
        public static ConfigureSurugayaCrawlerViewModel ConfigureSurugayaCrawlerViewModel { get; set; }

        //models
        public static CrawlerSet CrawlerSet { get; set; }

        //items
        public static CrawlerEntryViewModel CrawlerEntryViewModel { get; set; }
        public static DesignTimeCrawlerDescriptor<SurugayaSourceParameters> SurugayaCrawlerDescriptor { get; set; }
    }
}
