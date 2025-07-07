using System.Diagnostics;

namespace N3O.Umbraco.Telemetry.Extensions;

public static class ActivitySourceExtensions {
    public static TimedActivity StartTimedActivity(this ActivitySource activitySource,
                                                   string name,
                                                   string category,
                                                   ITelemetryStopwatch stopwatch = null) {
        var timedActivity = default(TimedActivity);
        
        if (activitySource != null) {
            timedActivity = new TimedActivity(activitySource, name, stopwatch);

            timedActivity.AddTag("category", category);

            timedActivity.Start();
        }

        return timedActivity;
    }
}
