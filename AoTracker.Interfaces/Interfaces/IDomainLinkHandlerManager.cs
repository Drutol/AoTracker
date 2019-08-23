using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Interfaces
{
    public interface IDomainLinkHandlerManager
    {
        string GenerateWebsiteLink(ICrawlerResultItem item);
    }
}
