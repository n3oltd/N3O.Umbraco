using NodaTime.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace N3O.Umbraco.Telemetry;

public class TimedActivity : IDisposable {
    private const string DurationNanoSeconds = nameof(DurationNanoSeconds);
    private const string DurationWeight = nameof(DurationWeight);
    
    private readonly IDurationWeightFinder _weightFinder;
    private readonly string _eventName;
    private readonly Activity _activity;
    private readonly List<KeyValuePair<string, object>> _tags = new();
    private readonly Stopwatch _stopWatch = new();

    public TimedActivity(IDurationWeightFinder weightFinder, ActivitySource source, string eventName, string eventCategory) {
        _weightFinder = weightFinder;
        _eventName = eventName;
        _activity = source.StartActivity();
    }

    public TimedActivity AddTag(string key, object value) {
        _tags.Add(new KeyValuePair<string, object>(key, value));

        return this;
    }

    public TimedActivity Start() {
        _stopWatch.Start();

        return this;
    }
    
    public TimedActivity Stop() {
        var duration = _stopWatch.ElapsedDuration();

        AddTag(DurationNanoSeconds, duration.TotalNanoseconds);

        return AddTag(DurationWeight, _weightFinder.GetWeight(duration));
    }

    public void Dispose() {
        _activity?.AddEvent(new ActivityEvent(_eventName, tags : new ActivityTagsCollection(_tags)));
        
        _activity?.Dispose();
    }
}
