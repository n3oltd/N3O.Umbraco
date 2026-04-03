namespace N3O.Umbraco.Telemetry;

public interface ITelemetryData {
    string GetDeploymentVersion();
    string GetExtensionsVersion();
}
