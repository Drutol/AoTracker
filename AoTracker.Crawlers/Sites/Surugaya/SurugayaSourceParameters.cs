using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Surugaya
{
    public class SurugayaSourceParameters : SourceParametersBase
    {
        public bool TrimJapaneseQuotationMarks { get; set; }
    }
}
