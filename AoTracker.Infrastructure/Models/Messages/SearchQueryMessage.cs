using System;
using System.Collections.Generic;
using System.Text;

namespace AoTracker.Infrastructure.Models.Messages
{
    public class SearchQueryMessage
    {
        public SearchQueryMessage(string newText)
        {
            Query = newText;
        }

        public string Query { get; }
    }
}
