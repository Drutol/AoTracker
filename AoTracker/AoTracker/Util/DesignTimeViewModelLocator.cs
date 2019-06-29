using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Infrastructure.ViewModels;

namespace AoTracker.Util
{
    public class DesignTimeViewModelLocator
    {
        public static WelcomeViewModel WelcomeViewModel { get; set; }
        public static FeedViewModel FeedViewModel { get; set; }
        public static MainViewModel MainViewModel { get; set; }
    }
}
