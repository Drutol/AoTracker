using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;

namespace AoTracker.Domain.Messaging
{
    public class PageTitleMessage
    {
        public string NewTitle { get; }
        public PageIndex Page { get; }

        public PageTitleMessage(PageIndex page, string title)
        {
            Page = page;
            NewTitle = title;
        }
    }
}
