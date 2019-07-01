using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Models;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.Infrastructure
{
    public class UserDataProvider : IUserDataProvider
    {
        public List<CrawlerSet> CrawlingSets { get; } = new List<CrawlerSet>()
        {
            new CrawlerSet
            {
                Name = "Test",
                Descriptors = new List<CrawlerDescriptor>
                {
                    new CrawlerDescriptor
                    {
                        CrawlerDomain = CrawlerDomain.Surugaya,
                        CrawlerSourceParameters = new SurugayaSourceParameters
                        {
                            SearchQuery = "蒼の彼方　タペストリ"
                        }
                    }
                }
            }
        };
    }
}
