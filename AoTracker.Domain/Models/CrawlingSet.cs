using System;
using System.Collections.Generic;
using System.Text;

namespace AoTracker.Domain.Models
{
    public class CrawlingSet
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<CrawlerDescriptor> Descriptors { get; set; }
    }
}
