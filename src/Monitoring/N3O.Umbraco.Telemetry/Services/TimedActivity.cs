using System;
using System.Diagnostics;

namespace N3O.Umbraco.Telemetry;

public class TimedActivity : IDisposable {
    private readonly ITelemetryStopwatch _stopwatch;
    private readonly Activity _activity;

    public TimedActivity(ActivitySource source, string name, ITelemetryStopwatch stopwatch = null) {
        _activity = source.StartActivity(name);
        _stopwatch = stopwatch ?? new TelemetryStopwatch();
    }
    
    public TimedEvent BeginEvent(string name) {
        var timedEvent = new TimedEvent(_stopwatch, _activity, name);

        timedEvent.Start();

        return timedEvent;
    }
    
    public TimedActivity AddBaggage(string key, string value) {
        _activity.AddBaggage(key, value);

        return this;
    }

    public TimedActivity AddTag(string key, object value) {
        _activity.AddTag(key, value);

        return this;
    }

    public TimedActivity Start() {
        _stopwatch.Start();

        return this;
    }
    
    public TimedActivity Stop() {
        foreach (var (key, value) in _activity.Baggage) {
            _activity.AddTag(key, value);
        }

        foreach (var (key, value) in _stopwatch.Stop()) {
            _activity.AddTag(key, value);    
        }

        _activity.Stop();

        return this;
    }

    public void Dispose() {
        Stop();
        
        _activity?.Dispose();
    }
}