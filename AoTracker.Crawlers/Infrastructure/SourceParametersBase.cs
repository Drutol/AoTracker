using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Infrastructure
{
    public abstract class SourceParametersBase : ICrawlerSourceParameters
    {
        public string SearchQuery { get; set; }

        public double PercentageIncrease { get; set; }
        public double OffsetIncrease { get; set; }
    }
}
