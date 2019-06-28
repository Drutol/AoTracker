using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;

namespace AoTracker.Domain.Models
{
    public class UserData
    {
        public UserKind UserKind { get; set; }

        public List<CrawlingSet> CrawlingSets { get; set; }
    }
}
