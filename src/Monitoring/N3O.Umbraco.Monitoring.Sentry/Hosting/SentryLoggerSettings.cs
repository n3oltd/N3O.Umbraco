using N3O.Umbraco.Monitoring.Sentry.Configuration;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace N3O.Umbraco.Monitoring.Sentry;

public class SentryLoggerSettings : ILoggerSettings {
    private readonly SentryConfiguration _configuration;
    private readonly string _environment;

    public SentryLoggerSettings(SentryConfiguration configuration, string environment) {
        _configuration = configuration;
        _environment = environment;
    }

    public void Configure(LoggerConfiguration loggerConfiguration) {
        loggerConfiguration.WriteTo.Sentry(opt => {
            opt.InitializeSdk = false;
            opt.Dsn = _configuration.Dsn;
            opt.Environment = _environment;
            opt.MinimumEventLevel = LogEventLevel.Error;
        });
    }
}
