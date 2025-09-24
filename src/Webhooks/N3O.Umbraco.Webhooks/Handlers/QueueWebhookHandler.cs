using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Context;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Webhooks.Commands;
using N3O.Umbraco.Webhooks.Models;
using NodaTime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace N3O.Umbraco.Webhooks.Handlers;

public class QueueWebhookHandler : IRequestHandler<QueueWebhookCommand, None, None> {
    private readonly IBackgroundJob _backgroundJob;
    private readonly IClock _clock;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;
    private readonly IMediator _mediator;
    private readonly ILogger<QueueWebhookHandler> _logger;

    public QueueWebhookHandler(IBackgroundJob backgroundJob,
                               IClock clock,
                               IHttpContextAccessor httpContextAccessor,
                               IRemoteIpAddressAccessor remoteIpAddressAccessor,
                               IMediator mediator,
                               ILogger<QueueWebhookHandler> logger) {
        _backgroundJob = backgroundJob;
        _clock = clock;
        _httpContextAccessor = httpContextAccessor;
        _remoteIpAddressAccessor = remoteIpAddressAccessor;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<None> Handle(QueueWebhookCommand req, CancellationToken cancellationToken) {
        var payload = CreatePayload(req.HookId.Value, req.HookRoute.Value);

        if (payload.HeaderData.ContainsKey("N3O-Foreground-Job")) {
            try {
                await _mediator.SendAsync<ProcessWebhookCommand, WebhookPayload>(payload);

                return None.Empty;
            } catch (Exception e) {
                _logger.LogError(e, "There was an error processing webhook {HookId} : {Error}. ", req.HookId.Value, e.Message);
            }
        } else {
            var jobName = $"PWH {payload.HookId} from {payload.RemoteIp}";
            
            _backgroundJob.Enqueue<ProcessWebhookCommand, WebhookPayload>(jobName, payload);
        }
        
        return None.Empty;
    }

    private WebhookPayload CreatePayload(string hookId, string route) {
        var httpRequest = _httpContextAccessor.HttpContext.Request;

        var timestamp = _clock.GetCurrentInstant();
        var remoteIp = _remoteIpAddressAccessor.GetRemoteIpAddress();
        var routeSegments = HttpUtility.UrlDecode(route ?? "")
                                       .Split("/",
                                              StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var headerData = new Dictionary<string, string>();
        foreach (var header in httpRequest.Headers) {
            headerData.Add(header.Key, header.Value.First());
        }

        var queryData = new Dictionary<string, string>();
        foreach (var queryParameter in httpRequest.Query) {
            queryData.Add(queryParameter.Key, queryParameter.Value.First());
        }

        var postData = new Dictionary<string, string>();
        if (httpRequest.HasFormContentType) {
            foreach (var postParameter in httpRequest.Form) {
                postData.Add(postParameter.Key, postParameter.Value.First());
            }
        }

        string body;
        using (var reader = new StreamReader(httpRequest.Body)) {
            body = reader.ReadToEnd();
        }

        return new WebhookPayload(hookId, timestamp, remoteIp, headerData, postData, queryData, routeSegments, body);
    }
}
