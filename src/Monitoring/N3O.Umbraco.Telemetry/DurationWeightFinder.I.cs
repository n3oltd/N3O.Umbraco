using NodaTime;

namespace N3O.Umbraco.Telemetry; 

public interface IDurationWeightFinder {
    string GetWeight(Duration duration);
}