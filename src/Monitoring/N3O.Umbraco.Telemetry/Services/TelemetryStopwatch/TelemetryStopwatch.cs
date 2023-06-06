using NodaTime;
using NodaTime.Extensions;
using System.Collections.Generic;
using System.Diagnostics;

namespace N3O.Umbraco.Telemetry;

public class TelemetryStopwatch : ITelemetryStopwatch {
    private readonly Stopwatch _stopwatch;

    public TelemetryStopwatch() {
        _stopwatch = new Stopwatch();
    }
    
    public void Start() {
        _stopwatch.Start();
    }

    public IEnumerable<KeyValuePair<string, object>> Stop() {
        _stopwatch.Stop();
        
        var duration = _stopwatch.ElapsedDuration();

        yield return new KeyValuePair<string, object>("duration.nanoseconds", duration.TotalNanoseconds);
        yield return new KeyValuePair<string, object>("duration.bucket", GetBucket(duration));
    }
    
    protected virtual string GetBucket(Duration duration) {
        if (duration.TotalMilliseconds < 5) {
            return "xs";
        } else if (duration.TotalMilliseconds < 10) {
            return "s";
        } else if (duration.TotalMilliseconds < 50) {
            return "m";
        } else if (duration.TotalMilliseconds < 500) {
            return "l";
        } else {
            return "xl";
        }
    }
}
