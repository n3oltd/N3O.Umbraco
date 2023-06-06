using NodaTime.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace N3O.Umbraco.Telemetry;

public class TimedActivity : IDisposable {
    private readonly IActivityDurationBucketer _activityDurationBucketer;
    private readonly string _eventName;
    private readonly Activity _activity;
    private readonly List<KeyValuePair<string, object>> _tags = new();
    private readonly Stopwatch _stopWatch = new();

    public TimedActivity(ActivitySource source,
                         string eventName,
                         IActivityDurationBucketer activityDurationBucketer = null) {
        _eventName = eventName;
        _activity = source.StartActivity();
        _activityDurationBucketer = activityDurationBucketer ?? new DefaultActivityDurationBucketer();
    }
    
    public TimedActivity AddEvent(string name, IReadOnlyList<KeyValuePair<string, object>> tags) {
        _activity?.AddEvent(new ActivityEvent(name, tags : new ActivityTagsCollection(tags)));

        return this;
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

        AddTag("DurationNanoseconds", duration.TotalNanoseconds);
        AddTag("DurationBucket", _activityDurationBucketer.GetBucket(duration));

        _activity?.Stop();

        return this;
    }

    public void Dispose() {
        Stop();
        
        _activity?.AddEvent(new ActivityEvent(_eventName, tags : new ActivityTagsCollection(_tags)));
        _activity?.Dispose();
    }
}
