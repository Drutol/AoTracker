using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;

namespace AoTracker.Interfaces
{
    public interface ITelemetryProvider
    {
        void TrackEvent(TelemetryEvent ev);
        void TrackEvent(TelemetryEvent ev, string value);
        void TrackEvent(TelemetryEvent ev, string key, string value);
        void TrackEvent(TelemetryEvent ev, Dictionary<string, string> metadata);
    }
}
