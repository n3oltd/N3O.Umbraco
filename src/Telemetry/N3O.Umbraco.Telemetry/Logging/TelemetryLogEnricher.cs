using N3O.Umbraco.Logging;
using System.Collections.Generic;

namespace N3O.Umbraco.Telemetry.Logging;

public class TelemetryLogEnricher : LogEnricher {
    private readonly ITelemetryData _telemetryData;

    public TelemetryLogEnricher(ITelemetryData telemetryData) {
        _telemetryData = telemetryData;
    }

    public override IReadOnlyDictionary<string, string> GetContextData() {
        var contextData = new Dictionary<string, string>();

        contextData["ExtensionsVersion"] = _telemetryData.GetExtensionsVersion();
        
        return contextData;
    }
}