using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Adapters.Core;
using AoLibs.Adapters.Core.Interfaces;
using AoTracker.Domain.Models;

namespace AoTracker.Infrastructure.Infrastructure
{
    public class AppVariables : AppVariablesBase
    {
        public AppVariables(ISettingsProvider settingsProvider, IDataCache dataCache = null) : base(settingsProvider, dataCache)
        {
        }

        [Variable]
        public Holder<List<CrawlerSet>> CrawlerSets { get; set; }
    }
}
