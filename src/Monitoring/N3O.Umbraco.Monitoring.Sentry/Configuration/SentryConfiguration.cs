namespace N3O.Umbraco.Monitoring.Sentry.Configuration;

public class SentryConfiguration {
    public string Dsn { get; set; }
    public double TracesSampleRate { get; set; } = 0.0;
    public string[] TracesIgnorePaths { get; set; } = ["/health", "/live", "/ready", "/metrics"];
}