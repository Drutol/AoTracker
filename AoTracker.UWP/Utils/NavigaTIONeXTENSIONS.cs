using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Domain.Enums;
using AoTracker.UWP.Models;

namespace AoTracker.UWP.Utils
{
    public static class NavigationExtensions
    {
        public static NavigationStack GetAssociatedStack(this PageIndex pageIndex)
        {
            switch (pageIndex)
            {
                case PageIndex.Welcome:
                case PageIndex.Feed:
                case PageIndex.CrawlerSets:
                case PageIndex.IgnoredItems:
                case PageIndex.WatchedItems:
                case PageIndex.CrawlerSetDetails:
                    return NavigationStack.MainStack;
                case PageIndex.ConfigureSurugaya:
                case PageIndex.ConfigureMandarake:
                case PageIndex.ConfigureYahoo:
                case PageIndex.ConfigureMercari:
                case PageIndex.ConfigureLashinbang:
                case PageIndex.SettingsIndex:
                case PageIndex.SettingsGeneral:
                case PageIndex.SettingsAbout:
                    return NavigationStack.OffStack;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pageIndex), pageIndex, null);
            }
        }
    }
}
