using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;

namespace AoTracker.Interfaces
{
    public interface ISettings
    {
        bool PassedWelcome { get; set; }
        AppTheme AppTheme { get; set; }
        bool AutoLoadFeedTab { get; set; }
        bool GenerateFeedAggregate { get; set; }
        bool UsePriceIncreaseProxyPresets { get; set; }
        ProxyDomain ProxyDomain { get; set; }
        bool FeedUpdateJobScheduled { get; set; }
    }
}
