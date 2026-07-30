using Flurl;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Scheduler.Models;
using NodaTime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace N3O.Umbraco.Scheduler;

public class JobTrigger {
    private const int MaxDeferAttempts = 20;

    private static readonly Duration DeferInterval = Duration.FromSeconds(30);

    private static readonly IReadOnlyList<string> InternalParameters = [
        SchedulerConstants.Parameters.Attempt,
        SchedulerConstants.Parameters.Queue,
        SchedulerConstants.Parameters.Signature
    ];

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IJsonProvider _jsonProvider;
    private readonly IJobUrlProvider _jobUrlProvider;
    private readonly IJobSignatureProvider _jobSignatureProvider;
    private readonly SchedulerSettings _settings;
    private readonly IClock _clock;
    private readonly ILogger<JobTrigger> _logger;

    public JobTrigger(IHttpClientFactory httpClientFactory,
                      IJsonProvider jsonProvider,
                      IJobUrlProvider jobUrlProvider,
                      IJobSignatureProvider jobSignatureProvider,
                      SchedulerSettings settings,
                      IClock clock,
                      ILogger<JobTrigger> logger) {
        _httpClientFactory = httpClientFactory;
        _jsonProvider = jsonProvider;
        _jobUrlProvider = jobUrlProvider;
        _jobSignatureProvider = jobSignatureProvider;
        _settings = settings;
        _clock = clock;
        _logger = logger;
    }

    [DisplayName("{0}")]
    public async Task TriggerAsync(string jobName,
                                   string triggerKey,
                                   string modelJson,
                                   IReadOnlyDictionary<string, string> parameterData) {
        var scheduledSignature = GetParameter(parameterData, SchedulerConstants.Parameters.Signature);
        var currentSignature = _jobSignatureProvider.GetSignature();

        if (IsForAnotherRuntime(scheduledSignature, currentSignature)) {
            var deferred = TryDefer(jobName,
                                    triggerKey,
                                    modelJson,
                                    parameterData,
                                    scheduledSignature,
                                    currentSignature);

            if (deferred) {
                return;
            }
        }

        var httpClient = _httpClientFactory.CreateClient();
        httpClient.Timeout = TimeSpan.FromMinutes(_settings.JobTimeoutMinutes);
        var req = GetProxyReq(triggerKey, modelJson, parameterData);
        var url = GetUrl();
        var reqStr = _jsonProvider.SerializeObject(req);

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new StringContent(reqStr, null, "application/json");

        request.Headers.Add("accept", "*/*");
        request.Headers.Add("X-Api-Key", TriggerKey.ApiSecurityKey);

        if (parameterData?.ContainsKey(SchedulerConstants.Parameters.Culture) == true) {
            request.Headers.Add("Accept-Language", parameterData[SchedulerConstants.Parameters.Culture]);
        }

        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode) {
            var content = await response.Content.ReadAsStringAsync();

            throw new Exception(content);
        }
    }

    private bool TryDefer(string jobName,
                          string triggerKey,
                          string modelJson,
                          IReadOnlyDictionary<string, string> parameterData,
                          string scheduledSignature,
                          string currentSignature) {
        var attempt = GetAttempt(parameterData);

        if (attempt >= MaxDeferAttempts) {
            _logger.LogWarning("Running job {JobName} for {ScheduledSignature} after {Attempts} deferrals",
                               jobName,
                               scheduledSignature,
                               attempt);

            return false;
        }

        _logger.LogInformation("Deferring job {JobName} for {ScheduledSignature}, current is {CurrentSignature}",
                               jobName,
                               scheduledSignature,
                               currentSignature);

        var queue = GetParameter(parameterData, SchedulerConstants.Parameters.Queue);

        if (!queue.HasValue()) {
            queue = SchedulerConstants.Queues.Default;
        }

        var deferredParameterData = parameterData.OrEmpty().ToDictionary(x => x.Key, x => x.Value);
        deferredParameterData[SchedulerConstants.Parameters.Attempt] = (attempt + 1).ToString();

        var enqueueAt = _clock.GetCurrentInstant().Plus(DeferInterval).ToDateTimeOffset();

        Hangfire.BackgroundJob.Schedule<JobTrigger>(queue,
                                                   j => j.TriggerAsync(jobName,
                                                                       triggerKey,
                                                                       modelJson,
                                                                       deferredParameterData),
                                                   enqueueAt);

        return true;
    }

    private ProxyReq GetProxyReq(string triggerKey,
                                 string modelJson,
                                 IReadOnlyDictionary<string, string> parameterData) {
        var requestType = TriggerKey.ParseRequestType(triggerKey);
        var modelType = TriggerKey.ParseModelType(triggerKey);

        var req = new ProxyReq();
        req.CommandType = requestType;
        req.RequestType = modelType;
        req.RequestBody = modelJson;
        req.ParameterData = parameterData.OrEmpty()
                                         .Where(x => !InternalParameters.Contains(x.Key))
                                         .ToDictionary(x => x.Key, x => x.Value);

        return req;
    }

    private string GetUrl() {
        var baseUrl = _jobUrlProvider.GetBaseUrl();
        var url = new Url(baseUrl).AppendPathSegment("/umbraco/api/JobProxy/executeProxied");

        return url;
    }

    private static int GetAttempt(IReadOnlyDictionary<string, string> parameterData) {
        var attempt = GetParameter(parameterData, SchedulerConstants.Parameters.Attempt);

        if (int.TryParse(attempt, out var parsedAttempt)) {
            return parsedAttempt;
        }

        return 0;
    }

    private static string GetParameter(IReadOnlyDictionary<string, string> parameterData, string name) {
        if (parameterData == null) {
            return null;
        }

        return parameterData.GetValueOrDefault(name);
    }

    private static bool IsForAnotherRuntime(string scheduledSignature, string currentSignature) {
        return scheduledSignature.HasValue() &&
               currentSignature.HasValue() &&
               scheduledSignature != currentSignature;
    }
}