using System.Diagnostics;

namespace N3O.Umbraco.Telemetry.Extensions;

public static class ActivitySourceExtensions {
    public static TimedActivity StartTimedActivity(this ActivitySource activitySource,
                                                   string eventName,
                                                   string eventCategory,
                                                   IActivityDurationBucketer activityDurationBucketer = null) {
        if (activitySource != null) {
            var timedActivity = new TimedActivity(activitySource, eventName, activityDurationBucketer);

            timedActivity.AddTag("Category", eventCategory).Start();

            return timedActivity;
        }

        return null;
    }
}
