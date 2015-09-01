using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using PopcornTime.Services.RunTime;

namespace PopcornTime.Services.Interfaces
{
    internal interface IInsightsService
    {
        TelemetryClient Client { get; }
        InsightsService.InsightsStopwatchEvent TrackTimeEvent(string name, IDictionary<string, string> properties = null);
        void TrackPageView(string name, string parameter);
    }
}