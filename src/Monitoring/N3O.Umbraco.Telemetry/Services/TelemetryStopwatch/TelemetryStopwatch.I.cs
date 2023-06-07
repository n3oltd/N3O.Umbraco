using System.Collections.Generic;

namespace N3O.Umbraco.Telemetry; 

public interface ITelemetryStopwatch {
    void Start();
    IEnumerable<KeyValuePair<string, object>> Stop();
}