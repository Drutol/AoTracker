using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Models;

namespace AoTracker.Infrastructure.Models.Messages
{
    public class ConfigureCrawlerResultMessage
    {
        public const string MessageKey = nameof(ConfigureCrawlerResultMessage);

        public enum ActionType
        {
            Add,
            Edit
        }

        public ActionType Action { get; set; }
        public CrawlerDescriptor CrawlerDescriptor { get; set; }
    }
}
