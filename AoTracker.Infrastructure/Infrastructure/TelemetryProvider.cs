using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Interfaces;
using Microsoft.AppCenter.Analytics;

namespace AoTracker.Infrastructure.Infrastructure
{
    public class TelemetryProvider : ITelemetryProvider
    {
        private const string ValueKey = "Value";

        public void TrackEvent(TelemetryEvent ev)
        {
            TrackEvent(ev, (Dictionary<string, string>) null);
        }

        public void TrackEvent(TelemetryEvent ev, string value)
        {
            TrackEvent(ev, ValueKey, value);
        }

        public void TrackEvent(TelemetryEvent ev, string key, string value)
        {
            TrackEvent(ev, new Dictionary<string, string>
            {
                {key, value}
            });
        }

        public void TrackEvent(TelemetryEvent ev, Dictionary<string, string> metadata)
        {
            Analytics.TrackEvent(ev.ToString(), metadata);
        }
    }
}
