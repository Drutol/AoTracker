﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Domain.Models;

namespace AoTracker.Interfaces
{
    public interface IUserDataProvider
    {
        IReadOnlyList<CrawlerSet> CrawlingSets { get; }

        Task Initialize();

        Task AddNewSet(CrawlerSet set);
        Task RemoveSet(CrawlerSet set);
        Task UpdateSet(CrawlerSet set);
    }
}
