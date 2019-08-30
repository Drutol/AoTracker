using System;
using System.Collections.Generic;
using System.Text;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerSourceParameters
    {
        string SearchQuery { get; set; }

        double PercentageIncrease { get; set; }
        double OffsetIncrease { get; set; }
        List<string> ExcludedKeywords { get; set; }
    }
}
