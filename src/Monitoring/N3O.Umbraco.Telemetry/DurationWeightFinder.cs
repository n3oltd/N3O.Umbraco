using NodaTime;

namespace N3O.Umbraco.Telemetry;

public class DurationWeightFinder : IDurationWeightFinder {
    private const string Short = nameof(Short);
    private const string Medium = nameof(Medium);
    private const string Long  = nameof(Long);
    
    public string GetWeight(Duration duration) {
        if (duration.TotalMilliseconds < 5) {
            return Short;
        } else if (duration.TotalMilliseconds < 10) {
            return Medium;
        } else {
            return Long;
        } 
    }
}
