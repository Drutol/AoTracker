using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace AoTracker.Domain.Models
{
    public class CrawlerSet
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<CrawlerDescriptor> Descriptors { get; set; } = new List<CrawlerDescriptor>();

        [JsonIgnore] public bool IsValid => !string.IsNullOrEmpty(Name) && Descriptors.Any();
    }
}
