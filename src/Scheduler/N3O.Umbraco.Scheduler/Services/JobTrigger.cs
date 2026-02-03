using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Json;
using N3O.Umbraco.Scheduler.Models;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Scheduler;

public class JobTrigger {
    private readonly IJsonProvider _jsonProvider;

    public JobTrigger(IJsonProvider jsonProvider) {
        _jsonProvider = jsonProvider;
    }

    [DisplayName("{0}")]
    public async Task TriggerAsync(string jobName,
                                   string triggerKey,
                                   string modelJson,
                                   IReadOnlyDictionary<string, string> parameterData) {
        var req = GetProxyReq(triggerKey, modelJson, parameterData);
        
        var baseUrl = $"https://{EnvironmentData.GetOurValue("Canonical_Domain")}";
        
        using (var httpClient = new HttpClient()) {
            httpClient.Timeout = TimeSpan.FromMinutes(30);
            
            var reqStr = _jsonProvider.SerializeObject(req);
            
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/umbraco/api/JobProxy/executeProxied");
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
}
