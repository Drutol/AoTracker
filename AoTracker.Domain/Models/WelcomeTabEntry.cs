using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;

namespace AoTracker.Domain.Models
{
    public class WelcomeTabEntry
    {
        public WelcomeStage WelcomeStage { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
    }
}
