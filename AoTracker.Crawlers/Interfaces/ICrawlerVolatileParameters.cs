using System;
using System.Collections.Generic;
using System.Text;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerVolatileParameters
    {
        int Page { get; }
        bool UseCache { get; }
    }
}
