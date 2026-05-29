using N3O.Umbraco.Logging;
using System;
using System.Linq;
using System.Collections.Generic;

namespace N3O.Umbraco.Telemetry.Logging;

public class TelemetryLogEnricher : LogEnricher {
    private readonly ITelemetryData _telemetryData;

    public TelemetryLogEnricher(ITelemetryData telemetryData) {
        _telemetryData = telemetryData;
    }

    public override IReadOnlyDictionary<string, string> GetTags() {
        var data = new Dictionary<string, string>();

        var extensionsVersion = _telemetryData.GetExtensionsVersion();

        if (extensionsVersion.HasValue()) {
            data["extensionsVersion"] = extensionsVersion;
        }

        return data;
    }
}