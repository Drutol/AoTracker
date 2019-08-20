using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;

namespace AoTracker.Infrastructure.Models
{
    public class SettingsIndexEntry
    {
        public PageIndex Page { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
    }
}
