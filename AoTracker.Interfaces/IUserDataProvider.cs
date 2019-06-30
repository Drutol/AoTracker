using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Models;

namespace AoTracker.Interfaces
{
    public interface IUserDataProvider
    {
        List<CrawlerSet> CrawlingSets { get; }
    }
}
