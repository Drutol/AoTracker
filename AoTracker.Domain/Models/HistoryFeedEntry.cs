﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AoTracker.Domain.Models
{
    public class HistoryFeedEntry
    {
        public string InternalId { get; set; }
        public float PreviousPrice { get; set; }
    }
}
