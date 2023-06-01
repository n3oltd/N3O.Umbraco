using System.Diagnostics;

namespace N3O.Umbraco.Telemetry;

public static class ActivitySourceExtensions {
    private const string Category = nameof(Category);

    public static TimedActivity StartTimedActivity(this ActivitySource activitySource,
                                                   IDurationWeightFinder weightFinder,
                                                   string eventName,
                                                   string eventCategory) {
        if (activitySource != null) {
            var timedActivity = new TimedActivity(weightFinder, activitySource, eventName, eventCategory);

            timedActivity.AddTag(Category, eventCategory).Start();

            return timedActivity;
        }

        return null;
    }
}
