using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Webhooks.Attributes;
using N3O.Umbraco.Webhooks.Extensions;
using N3O.Umbraco.Webhooks.Models;
using N3O.Umbraco.Webhooks.Receivers;
using Newtonsoft.Json.Linq;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Extensions;
using static N3O.Umbraco.Data.DataConstants.Webhooks;

namespace N3O.Umbraco.Data.Webhooks; 

[WebhookReceiver(HookIds.Import)]
public class ImportReceiver : WebhookReceiver {
    private readonly IClock _clock;
    private readonly IJsonProvider _jsonProvider;
    private readonly IImportQueue _importQueue;

    public ImportReceiver(IClock clock, IJsonProvider jsonProvider, IImportQueue importQueue) {
        _clock = clock;
        _jsonProvider = jsonProvider;
        _importQueue = importQueue;
    }
    
    protected override async Task ProcessAsync(WebhookPayload payload, CancellationToken cancellationToken) {
        var storageFolderName = _clock.GetCurrentInstant().ToUnixTimeMilliseconds().ToString();
        
        var (containerId, contentTypeAlias) = GetRouteParameters(payload.RouteSegments.OrEmpty().ToList());
        var sourceValues = GetSourceValues(payload);
        var contentId = payload.GetHeader(Headers.ContentId).TryParseAs<Guid>();
        var name = payload.GetHeader(Headers.Name);
        var replacesCriteria = payload.GetHeader(Headers.Replaces);

        await _importQueue.AppendAsync(containerId,
                                       contentTypeAlias,
                                       "webhook.json",
                                       null,
                                       payload.Timestamp,
                                       null,
                                       DatePatterns.YearMonthDay,
                                       storageFolderName,
                                       contentId,
                                       replacesCriteria,
                                       name,
                                       false,
                                       sourceValues,
                                       cancellationToken);

        await _importQueue.CommitAsync();
    }
    
    private (Guid ContainerId, string ContentTypeAlias) GetRouteParameters(IReadOnlyList<string> routeSegments) {
        if (routeSegments.Count != 2) {
            throw new Exception("Container ID and content type must both be specified");
        }

        if (!Guid.TryParse(routeSegments[0], out var containerId)) {
            throw new Exception("Invalid container ID specified");
        }

        var contentTypeAlias = routeSegments[1];

        if (!contentTypeAlias.HasValue()) {
            throw new Exception("Invalid content type specified");
        }

        return (containerId, contentTypeAlias);
    }

    private IReadOnlyDictionary<string, string> GetSourceValues(WebhookPayload payload) {
        var jObject = payload.GetBody<JObject>(_jsonProvider);
        var dict = new Dictionary<string, string>();

        foreach (var (key, value) in jObject) {
            switch (value.Type) {
                case JTokenType.Boolean:
                    dict[key] = ((bool) value).ToString(CultureInfo.InvariantCulture);
                    break;
                
                case JTokenType.Date:
                    dict[key] = ((DateTime) value).ToIsoString();
                    break;
                
                case JTokenType.Float:
                    dict[key] = ((float) value).ToString(CultureInfo.InvariantCulture);
                    break;
                
                case JTokenType.Guid:
                    dict[key] = ((Guid) value).ToString();
                    break;
                
                case JTokenType.Integer:
                    dict[key] = ((int) value).ToString(CultureInfo.InvariantCulture);
                    break;
                
                case JTokenType.Null:
                    dict[key] = null;
                    break;
                
                case JTokenType.String:
                    dict[key] = (string) value;
                    break;
                
                default:
                    throw UnrecognisedValueException.For(value.Type);
            }
        }

        return dict;
    }
}