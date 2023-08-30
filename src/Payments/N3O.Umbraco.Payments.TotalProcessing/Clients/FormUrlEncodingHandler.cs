using N3O.Umbraco.Extensions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing.Clients;

public class FormUrlEncodingHandler : DelegatingHandler {
    public FormUrlEncodingHandler(HttpMessageHandler innerHandler = null) {
        InnerHandler = innerHandler ?? new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken) {
        if (request.Content != null) {
            var content = await request.Content.ReadAsStringAsync(cancellationToken);

            request.Content = ConvertToFormUrlEncoded(content);

            if (request.Method != HttpMethod.Get) {
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private FormUrlEncodedContent ConvertToFormUrlEncoded(string content) {
        var properties = GetProperties(JObject.Parse(content));

        var formUrlEncodedContent = new FormUrlEncodedContent(properties);

        return formUrlEncodedContent;
    }

    private IEnumerable<KeyValuePair<string, string>> GetProperties(JObject jObject, string prefix = null) {
        var properties = new List<KeyValuePair<string, string>>();

        foreach (var (key, value) in jObject) {
            var propertyKey = prefix.HasValue() ? $"{prefix}.{key}" : key;

            properties.AddRange(GetJObjectProperties(propertyKey, value));
        }

        return properties;
    }

    private IReadOnlyList<KeyValuePair<string, string>> GetJArrayProperties(string propertyKey, JToken jToken) {
        var res = new List<KeyValuePair<string, string>>();

        foreach (var (item, index) in jToken.SelectWithIndex()) {
            propertyKey += $"[{index}]";
            res.AddRange(GetJObjectProperties(propertyKey, item));
        }

        return res;
    }

    private IEnumerable<KeyValuePair<string, string>> GetJObjectProperties(string key, JToken jToken) {
        var properties = new List<KeyValuePair<string, string>>();
        
        switch (jToken.Type) {
            case JTokenType.Array:
                properties.AddRange(GetJArrayProperties(key, jToken));
                break;
            
            case JTokenType.Object:
                properties.AddRange(GetProperties(jToken.ToObject<JObject>(), key).ToList());
                break;
            
            case JTokenType.Null:
                break;
            default: {
                properties.Add(new KeyValuePair<string, string>(key, jToken.ToString()));
                break;
            }
        }

        return properties;
    }
}
