using System.Collections.Generic;

namespace N3O.Umbraco.Telemetry.Configuration;

public class TelemetryConfiguration {
    public IEnumerable<string> CustomActivitySources { get; set; }
    public bool Enabled { get; set; }
    public string OtlpExporterUrl { get; set; }
    public string ServiceName { get; set; }
    public bool UseConsoleExporter { get; set; }
}
