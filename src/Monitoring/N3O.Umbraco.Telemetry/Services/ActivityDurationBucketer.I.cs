using NodaTime;

namespace N3O.Umbraco.Telemetry; 

public interface IActivityDurationBucketer {
    string GetBucket(Duration duration);
}