using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Enums;

namespace AoTracker.Interfaces
{
    public interface IDomainLinkHandler
    {
        ProxyDomain HandlingDomain { get; }

        string GenerateWebsiteLink(ICrawlerResultItem item);
    }
}
