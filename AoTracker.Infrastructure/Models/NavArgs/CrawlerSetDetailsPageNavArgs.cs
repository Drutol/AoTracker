using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Models;

namespace AoTracker.Infrastructure.Models.NavArgs
{
    public class CrawlerSetDetailsPageNavArgs
    {
        public static CrawlerSetDetailsPageNavArgs AddNew => new CrawlerSetDetailsPageNavArgs
        {
            AddingNew = true
        };

        public CrawlerSetDetailsPageNavArgs(CrawlerSet set)
        {
            CrawlerSet = set;
        }

        private CrawlerSetDetailsPageNavArgs()
        {

        }

        public bool AddingNew { get; private set; }
        public CrawlerSet CrawlerSet { get;  }
    }
}
