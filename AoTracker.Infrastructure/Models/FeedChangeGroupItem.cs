using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.Models
{
    public class FeedChangeGroupItem : IFeedItem
    {
        public FeedChangeGroupItem(DateTime lastChanged)
        {
            LastChanged = lastChanged;
        }

        public DateTime LastChanged { get; }
    }
}
