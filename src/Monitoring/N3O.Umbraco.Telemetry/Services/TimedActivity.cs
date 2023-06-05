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

        return this;
    }

    public void Dispose() {
        _activity?.AddEvent(new ActivityEvent(_eventName, tags : new ActivityTagsCollection(_tags)));
        _activity?.Stop();
        _activity?.Dispose();
    }
}
