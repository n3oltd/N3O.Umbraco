using NodaTime;

namespace N3O.Umbraco.Telemetry;

public class DefaultActivityDurationBucketer : IActivityDurationBucketer {
    public string GetBucket(Duration duration) {
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
