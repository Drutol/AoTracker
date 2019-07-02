using System;
using System.Collections.Generic;
using System.Text;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerSourceParameters
    {
        string SearchQuery { get; }

        double PercentageIncrease { get; }
        double OffsetIncrease { get; }
    }
}
