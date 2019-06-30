using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Infrastructure
{
    public class VolatileParametersBase : ICrawlerVolatileParameters
    {
        public int Page { get; set; }
    }
}
