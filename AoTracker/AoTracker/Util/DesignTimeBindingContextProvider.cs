using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.ViewModels;

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

        //models
        public static CrawlerSet CrawlerSet { get; set; }
        public static CrawlerSetDetailsViewModel.CrawlerEntry CrawlerEntry { get; set; }
    }
}
