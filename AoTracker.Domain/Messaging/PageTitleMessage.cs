using System;
using System.Collections.Generic;
using System.Text;

namespace AoTracker.Domain.Messaging
{
    public class PageTitleMessage
    {
        public string NewTitle { get; }

        public PageTitleMessage(string title)
        {
            NewTitle = title;
        }
    }
}
