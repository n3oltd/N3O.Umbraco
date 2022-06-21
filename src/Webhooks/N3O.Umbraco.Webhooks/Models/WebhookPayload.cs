using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using NodaTime;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace N3O.Umbraco.Webhooks.Models;

public class WebhookPayload : Value {
    public WebhookPayload(string hookId,
                          Instant timestamp,
                          IPAddress remoteIp,
                          IReadOnlyDictionary<string, string> headerData,
                          IReadOnlyDictionary<string, string> postData,
                          IReadOnlyDictionary<string, string> queryData,
                          IEnumerable<string> routeSegments,
                          string body) {
        HookId = hookId;
        Timestamp = timestamp;
        RemoteIp = remoteIp;
        HeaderData = headerData;
        PostData = postData;
        QueryData = queryData;
        RouteSegments = routeSegments;
        Body = body;
    }

    public string HookId { get; }
    public Instant Timestamp { get; }
    public IPAddress RemoteIp { get; }
    public IReadOnlyDictionary<string, string> HeaderData { get; }
    public IReadOnlyDictionary<string, string> PostData { get; }
    public IReadOnlyDictionary<string, string> QueryData { get; }
    public IEnumerable<string> RouteSegments { get; }
    public string Body { get; }

    [JsonIgnore]
    public Dictionary<string, string> Data {
        get {
            var data = HeaderData.Concat(QueryData).Concat(PostData).ToDictionary(x => x.Key, x => x.Value);

            return data;
        }
    }

    public string GetData(string key) => GetByKey(Data, key);
    public string GetHeader(string key) => GetByKey(HeaderData, key);
    public string GetPost(string key) => GetByKey(PostData, key);
    public string GetQuery(string key) => GetByKey(QueryData, key);

    private string GetByKey(IReadOnlyDictionary<string, string> data, string key) {
        var dataKey = data.Keys.SingleOrDefault(x => x.EqualsInvariant(key));
        var res = dataKey == null ? null : data[dataKey];

        return res;
    }
}
