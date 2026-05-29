using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Monitoring.Sentry.Configuration;
using Sentry;
using Sentry.Extensibility;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Monitoring.Sentry;

public class SentryInitializer : IHostedService {
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public SentryInitializer(IServiceProvider serviceProvider, IConfiguration configuration) {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        if (!Composer.WebHostEnvironment.IsProduction()) {
            return Task.CompletedTask;
        }

        var config = new SentryConfiguration();

        _configuration.GetSection("Sentry").Bind(config);

        var httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
        var rootProvider = _serviceProvider;

        SentrySdk.Init(opt => {
            opt.Dsn = config.Dsn;
            opt.AttachStacktrace = true;
            opt.ReportAssembliesMode = ReportAssembliesMode.InformationalVersion;
            opt.SendDefaultPii = false;
            opt.Environment = Composer.WebHostEnvironment.EnvironmentName;
            opt.Release = EnvironmentData.GetOurValue(EnvironmentVariables.Version);
            opt.DiagnosticLevel = SentryLevel.Error;
            opt.TracesSampleRate = 1.0f;
            opt.SetBeforeSend(SentryEventRateLimiter.BeforeSend);

            opt.AddEventProcessorProvider(() => ResolveEventProcessors(httpContextAccessor, rootProvider));
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static IEnumerable<ISentryEventProcessor> ResolveEventProcessors(IHttpContextAccessor httpContextAccessor,
                                                                             IServiceProvider rootProvider) {
        var scope = httpContextAccessor.HttpContext?.RequestServices ?? rootProvider;

        return scope.GetServices<ISentryEventProcessor>();
    }
}
