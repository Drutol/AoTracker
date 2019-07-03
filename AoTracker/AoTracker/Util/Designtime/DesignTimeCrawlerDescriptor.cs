using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Item;

namespace AoTracker.Util
{
    public class DesignTimeCrawlerDescriptorViewModel<T> : ItemViewModelBase<T> where T : class
    {
        public DesignTimeCrawlerDescriptorViewModel(T item) : base(item)
        {
        }
    };

    public class DesignTimeCrawlerDescriptor<T> where T : ICrawlerSourceParameters
    {
        public CrawlerDomain CrawlerDomain { get; set; }
        public T CrawlerSourceParameters { get; set; }
    };
}
