using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace N3O.Umbraco.Telemetry; 

public class TimedEvent : IDisposable {
    private readonly ITelemetryStopwatch _stopwatch;
    private readonly Activity _activity;
    private readonly string _name;
    private readonly Dictionary<string, object> _tags = new(StringComparer.InvariantCultureIgnoreCase);

    public TimedEvent(ITelemetryStopwatch stopwatch, Activity activity, string name) {
        _stopwatch = stopwatch;
        _activity = activity;
        _name = name;
    }

    public TimedEvent AddTag(string key, object value) {
        _tags[key] = value;

        return this;
    }

    public TimedEvent Start() {
        _stopwatch.Start();

        return this;
    }
    
    public TimedEvent Stop() {
        foreach (var (key, value) in _stopwatch.Stop()) {
            AddTag(key, value);
        }

        return this;
    }

    public void Dispose() {
        Stop();

        _activity?.AddEvent(new ActivityEvent(_name, tags: new ActivityTagsCollection(_tags)));
    }
}