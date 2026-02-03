using Flurl;
using N3O.Umbraco.Json;
using N3O.Umbraco.Scheduler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace N3O.Umbraco.Scheduler;

public class JobTrigger {
    private readonly IJsonProvider _jsonProvider;
    private readonly IJobUrlProvider _jobUrlProvider;

    public JobTrigger(IJsonProvider jsonProvider, IJobUrlProvider jobUrlProvider) {
        _jsonProvider = jsonProvider;
        _jobUrlProvider = jobUrlProvider;
    }

    [DisplayName("{0}")]
    public async Task TriggerAsync(string jobName,
                                   string triggerKey,
                                   string modelJson,
                                   IReadOnlyDictionary<string, string> parameterData) {
        using (var httpClient = new HttpClient()) {
            var req = GetProxyReq(triggerKey, modelJson, parameterData);
        
            var url = GetUrl();
            
            httpClient.Timeout = TimeSpan.FromMinutes(30);
            
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
        req.ParameterData = parameterData?.ToDictionary();
        
        return req;
    }
    
    private string GetUrl() {
        var baseUrl = _jobUrlProvider.GetBaseUrl();
        var url = new Url(baseUrl).AppendPathSegment("/umbraco/api/JobProxy/executeProxied");

        return url;
    }
}
