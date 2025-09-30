using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Scheduler.Models;
using N3O.Umbraco.Utilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Scheduler;

public class JobTrigger {
    private readonly IJsonProvider _jsonProvider;
    private readonly IUrlBuilder _urlBuilder;
    private readonly IUmbracoContextFactory _umbracoContextFactory;

    public JobTrigger(IJsonProvider jsonProvider, IUrlBuilder urlBuilder, IUmbracoContextFactory umbracoContextFactory) {
        _jsonProvider = jsonProvider;
        _urlBuilder = urlBuilder;
        _umbracoContextFactory = umbracoContextFactory;
    }

    [DisplayName("{0}")]
    public async Task TriggerAsync(string jobName,
                                   string triggerKey,
                                   string modelJson,
                                   IReadOnlyDictionary<string, string> parameterData) {
        var req = GetProxyReq(triggerKey, modelJson, parameterData);
        
        parameterData.TryGetValue(SchedulerConstants.Parameters.Culture, out var culture);

        var baseUrl = GetBasUrl();
        
        using (var httpClient = new HttpClient()) {
            var reqStr = _jsonProvider.SerializeObject(req);
            
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/umbraco/api/Test/executeProxied");
            request.Content = new StringContent(reqStr, null, "application/json");
            
            request.Headers.Add("accept", "*/*");

            if (culture.HasValue()) {
                request.Headers.Add("Accept-Language", culture);
            }

            var response = await httpClient.SendAsync(request);
            
            response.EnsureSuccessStatusCode();
        }
    }

    private string GetBasUrl() {
        using (_umbracoContextFactory.EnsureUmbracoContext()) {
            return _urlBuilder.Root().ToString().TrimEnd('/');
        }
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
        req.ParameterData = parameterData.ToDictionary();
        
        return req;
    }
}
