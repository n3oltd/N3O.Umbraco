using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Hosting;

public class HomepageWarmup : BackgroundService {
    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(90);
    private static readonly TimeSpan RetryInterval = TimeSpan.FromSeconds(2);

    private readonly ILogger<HomepageWarmup> _logger;
    private readonly string _url;

    public HomepageWarmup(ILogger<HomepageWarmup> logger) {
        _logger = logger;

        var canonicalDomain = EnvironmentData.GetOurValue(HostingConstants.Environment.Keys.CanonicalDomain);

        if (canonicalDomain.HasValue()) {
            _url = $"https://{canonicalDomain}/";
        } else {
            Ready = true;
            
            _logger.LogInformation("Homepage warmup skipped: no canonical domain configured");
        }
    }

    public bool Ready { get; private set; }
    public string LastError { get; private set; }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
        if (_url == null) {
            return;
        }

        using (var httpClient = new HttpClient()) {
            httpClient.Timeout = RequestTimeout;

            while (!cancellationToken.IsCancellationRequested) {
                try {
                    var response = await httpClient.GetAsync(_url, cancellationToken);

                    if (response.IsSuccessStatusCode) {
                        Ready = true;

                        return;
                    }

                    LastError = $"HTTP {(int) response.StatusCode}";
                } catch (Exception ex) when (ex is not OperationCanceledException) {
                    LastError = ex.Message;
                }

                await Task.Delay(RetryInterval, cancellationToken);
            }
        }
    }
}
