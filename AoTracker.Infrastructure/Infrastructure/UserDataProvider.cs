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
        public List<CrawlingSet> CrawlingSets { get; } = new List<CrawlingSet>()
        {
            new CrawlingSet
            {
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
